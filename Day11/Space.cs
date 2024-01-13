using System.Drawing;
using Library;

namespace Day11;

public class Space
{
    public char[][] Map { get; }
    public int Y { get; }
    public int X { get; }

    private const char GalaxySign = '#';

    public Space(string galaxy)
    {
        var lines = galaxy.Split(["\r", "\n"], StringSplitOptions.RemoveEmptyEntries).ToList();
        var additionalRows = lines.FindIndexes(line => line.All(c => c == '.')).ToList();
        var additionalColumns =
            Enumerable.Range(0, lines[0].Length).Where(index => lines.All(line => line[index] == '.')).ToList();
        
        // we have to increment each additional row and column by X, where X is number of elements preceding given index
        // the reasoning for that is because, galaxy is expanding with each element, so every next element must be
        // one higher index compared to previous one
        additionalRows = additionalRows.Select((num, i) => num + i).ToList();
        additionalColumns = additionalColumns.Select((num, i) => num + i).ToList();
        
        Y = lines.Count + additionalRows.Count;
        X = lines[0].Length + additionalColumns.Count;
        Map = Create2DArray<char>(Y, X);

        for (int y = 0; y < Y; y++)
        {
            var yOffset = additionalRows.Count(row => y > row);
            for (int x = 0; x < X; x++)
            {
                var xOffset = additionalColumns.Count(row => x > row);
                Map[y][x] = lines[y - yOffset][x - xOffset];
            }
        }
    }
    
    public Space(string galaxy, int expansionSize)
    {
        var lines = galaxy.Split(["\r", "\n"], StringSplitOptions.RemoveEmptyEntries).ToList();
        var additionalRows = lines.FindIndexes(line => line.All(c => c == '.')).ToList();
        var additionalColumns =
            Enumerable.Range(0, lines[0].Length).Where(index => lines.All(line => line[index] == '.')).ToList();
        
        Y = lines.Count + (additionalRows.Count * expansionSize);
        X = lines[0].Length + (additionalColumns.Count * expansionSize);
        Map = Create2DArray<char>(Y, X);

        for (int y = 0; y < lines.Count; y++)
        {
            var yRepetitions = (additionalRows.Contains(y) ? expansionSize : 0) + 1;
            var yOffset = additionalRows.Count(row => y > row) * expansionSize;
            for (int yRepetition = 0; yRepetition < yRepetitions; yRepetition++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {
                    var xRepetitions = (additionalColumns.Contains(x) ? expansionSize : 0) + 1;
                    var xOffset = additionalColumns.Count(column => x > column) * expansionSize;
                    for (int xRepetition = 0; xRepetition < xRepetitions; xRepetition++)
                    {
                        Map[y + yRepetition + yOffset][x + xRepetition + xOffset] = lines[y][x];
                    }
                }
            }
        }
    }

    private static T[][] Create2DArray<T>(int Y, int X)
    {
        var array = new T[Y][];
        for (int y = 0; y < Y; y++)
            array[y] = new T[X];
        return array;
    }

    private IEnumerable<Point> GetGalaxies()
    {
        for (int y = 0; y < Y; y++)
        {
            for (int x = 0; x < X; x++)
            {
                if (Map[y][x] == GalaxySign)
                    yield return new Point(x, y);
            }
        }
    }

    public long GetSumOfShortestPathsBetweenGalaxies()
    {
        long distance = 0;
        foreach (var permutationPair in GetPermutationsUnordered(GetGalaxies(), 2))
        {
            var galaxyPair = permutationPair.ToList();
            distance += long.Abs(galaxyPair[1].Y - galaxyPair[0].Y) + long.Abs(galaxyPair[1].X - galaxyPair[0].X);
        }

        return distance;
    }
    
    private static IEnumerable<IEnumerable<T>> GetPermutationsUnordered<T>(IEnumerable<T> items, int count)
    {
        int i = 0;
        var itemsList = items.ToList();
        foreach (var item in itemsList)
        {
            if (count == 1)
                yield return new T[] { item };
            else
            {
                foreach (var result in GetPermutationsUnordered(itemsList.Skip(i + 1), count - 1))
                    yield return new T[] { item }.Concat(result);
            }

            ++i;
        }
    }
}