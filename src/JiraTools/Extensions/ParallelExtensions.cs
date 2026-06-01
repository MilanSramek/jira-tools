using System.Diagnostics.CodeAnalysis;

namespace JiraTools.Extensions;

internal static class ParallelExtensions
{
    extension<T>(IEnumerable<Func<Task<T>>> tasks)
    {
        public async Task<T[]> WhenAll(int? maxDegreeOfParallelism = null)
        {
            if (maxDegreeOfParallelism is null)
            {
                return await Task.WhenAll(tasks.Select(taskFactory => taskFactory()));
            }
            if (maxDegreeOfParallelism <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxDegreeOfParallelism), "Max degree of parallelism must be greater than 0.");
            }

            var tasksQueue = new Queue<Func<Task<T>>>(tasks);
            var runningTasks = new HashSet<Task<T>>(maxDegreeOfParallelism.Value);
            var results = new T[tasksQueue.Count];
            Dictionary<Task<T>, int> tasksOrder = [];
            int taskIndex = 0;
            List<Exception>? exceptions = null;

            while (tasksQueue.Count > 0 && runningTasks.Count < maxDegreeOfParallelism.Value)
            {
                if (TryCreateTask(tasksQueue.Dequeue(), out Task<T>? task, out Exception? exception))
                {
                    runningTasks.Add(task);
                    tasksOrder.Add(task, taskIndex++);
                }
                else
                {
                    exceptions ??= [];
                    exceptions.Add(exception);
                }
            }

            while (runningTasks.Count > 0)
            {
                var completedTask = await Task.WhenAny(runningTasks);
                runningTasks.Remove(completedTask);

                switch (completedTask.Status)
                {
                    case TaskStatus.RanToCompletion:
                        var completedTaskIndex = tasksOrder[completedTask];
                        results[completedTaskIndex] = completedTask.Result; 
                        break;
                    case TaskStatus.Faulted:
                        exceptions ??= [];
                        if (completedTask.Exception is AggregateException aggregateException)
                        {
                            exceptions.AddRange(aggregateException.InnerExceptions);
                        }
                        else if (completedTask.Exception is { })
                        {
                            exceptions.Add(completedTask.Exception);
                        }
                        break;
                    case TaskStatus.Canceled:
                        exceptions ??= [];
                        exceptions.Add(new TaskCanceledException(completedTask));
                        break;
                }
                
                if (tasksQueue.TryDequeue(out Func<Task<T>>? nextTaskFactory))
                {
                    if (TryCreateTask(nextTaskFactory, out Task<T>? nextTask, out Exception? exception))
                    {
                        runningTasks.Add(nextTask);
                        tasksOrder.Add(nextTask, taskIndex++);
                    }
                    else
                    {
                        exceptions ??= [];
                        exceptions.Add(exception);
                    }
                }
            }

            return exceptions is null 
                ? results
                : throw new AggregateException(exceptions);

            static bool TryCreateTask(
                Func<Task<T>> taskFactory,
                [NotNullWhen(true)] out Task<T>? task,
                [NotNullWhen(false)] out Exception? exception)
            {
                try
                {
                    task = taskFactory();
                    exception = null;
                    return true;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    task = null;
                    return false;
                }
            }
        }
    }

    extension(IEnumerable<Func<Task>> tasks)
    {
        public async Task WhenAll(int? maxDegreeOfParallelism = null)
        {
            if (maxDegreeOfParallelism is null)
            {
                await Task.WhenAll(tasks.Select(taskFactory => taskFactory()));
                return;
            }
            if (maxDegreeOfParallelism <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxDegreeOfParallelism), "Max degree of parallelism must be greater than 0.");
            }

            var tasksQueue = new Queue<Func<Task>>(tasks);
            var runningTasks = new HashSet<Task>(maxDegreeOfParallelism.Value);
            List<Exception>? exceptions = null;

            while (tasksQueue.Count > 0 && runningTasks.Count < maxDegreeOfParallelism.Value)
            {
                if (TryCreateTask(tasksQueue.Dequeue(), out Task? task, out Exception? exception))
                {
                    runningTasks.Add(task);
                }
                else
                {
                    exceptions ??= [];
                    exceptions.Add(exception);
                }
            }

            while (runningTasks.Count > 0)
            {
                var completedTask = await Task.WhenAny(runningTasks);
                runningTasks.Remove(completedTask);

                switch (completedTask.Status)
                {
                    case TaskStatus.RanToCompletion:
                        break;
                    case TaskStatus.Faulted:
                        exceptions ??= [];
                        if (completedTask.Exception is AggregateException aggregateException)
                        {
                            exceptions.AddRange(aggregateException.InnerExceptions);
                        }
                        else if (completedTask.Exception is { })
                        {
                            exceptions.Add(completedTask.Exception);
                        }
                        break;
                    case TaskStatus.Canceled:
                        exceptions ??= [];
                        exceptions.Add(new TaskCanceledException(completedTask));
                        break;
                }
                
                if (tasksQueue.TryDequeue(out Func<Task>? nextTaskFactory))
                {
                    if (TryCreateTask(nextTaskFactory, out Task? nextTask, out Exception? exception))
                    {
                        runningTasks.Add(nextTask);
                    }
                    else
                    {
                        exceptions ??= [];
                        exceptions.Add(exception);
                    }
                }
            }

            if (exceptions is null)
            {
                return;
            }
            
            throw new AggregateException(exceptions);

            static bool TryCreateTask(
                Func<Task> taskFactory,
                [NotNullWhen(true)] out Task? task,
                [NotNullWhen(false)] out Exception? exception)
            {
                try
                {
                    task = taskFactory();
                    exception = null;
                    return true;
                }
                catch (Exception ex)
                {
                    exception = ex;
                    task = null;
                    return false;
                }
            }
        }
    }
}