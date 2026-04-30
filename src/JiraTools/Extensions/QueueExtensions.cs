namespace JiraTools.Extensions;

internal static class QueueExtensions
{
    extension <T>(Queue<T> queue)
    {
        public IEnumerable<T> DequeueRange(int count)
        {
            for (int i = 0; i < count && queue.Count > 0; i++)
            {
                if (queue.TryDequeue(out T? item))
                {
                    yield return item;
                }
                else
                {
                    break;
                }
            }
        }
    }
}