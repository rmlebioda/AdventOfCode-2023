namespace Library;

public static class IEnumerableExtensions
{
    public static IEnumerable<IEnumerable<TValue>> Chunk<TValue>(
        this IEnumerable<TValue> values, 
        int chunkSize)
    {
        return values
            .Select((v, i) => new {v, groupIndex = i / chunkSize})
            .GroupBy(x => x.groupIndex)
            .Select(g => g.Select(x => x.v));
    }

    public static IEnumerable<int> FindIndexes<T>(this IList<T> list, Predicate<T> match)
    {
        if (list is null)
        {
            throw new ArgumentNullException(nameof(list));
        }

        if (match is null)
        {
            throw new ArgumentNullException(nameof(match));
        }
        
        for (int i = 0; i < list.Count; i++)
        {
            if (match(list[i]))
            {
                yield return i;
            }
        }
    }
}