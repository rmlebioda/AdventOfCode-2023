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
}