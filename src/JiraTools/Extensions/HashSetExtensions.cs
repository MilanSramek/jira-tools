namespace JiraTools.Extensions;

internal static class HashSetExtensions
{
    extension<T>(HashSet<T> set)
    {
        public void AddRange(IEnumerable<T> items)
        {
            foreach (T item in items)
            {
                set.Add(item);
            }
        }
    }
}