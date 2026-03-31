namespace JiraTools.Extensions;

internal static class ParallelExtensions
{
    public static async Task WhenAll(
        this IEnumerable<Task> tasks,
        int? maxDegreeOfParallelism = null)
    {
        if (maxDegreeOfParallelism is null)
        {
            await Task.WhenAll(tasks);
            return;
        }

        foreach (var chunk in tasks.Chunk(maxDegreeOfParallelism.Value))
        {
            await Task.WhenAll(chunk);
        }
    }

    public static async Task<T[]> WhenAll<T>(
        this IEnumerable<Task<T>> tasks,
        int? maxDegreeOfParallelism = null)
    {
        if (maxDegreeOfParallelism is null)
        {
            return await Task.WhenAll(tasks);
        }

        var results = new List<T>();
        foreach (var chunk in tasks.Chunk(maxDegreeOfParallelism.Value))
        {
            var chunkResults = await Task.WhenAll(chunk);
            results.AddRange(chunkResults);
        }

        return [.. results];
    }
}