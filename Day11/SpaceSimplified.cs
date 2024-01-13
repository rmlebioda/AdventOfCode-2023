using System.Drawing;
using Library;

namespace Day11;

public class SpaceSimplified
{
    public List<Point> Galaxies = new List<Point>();
    
    private const char GalaxySign = '#';
    
    public SpaceSimplified(string galaxy, int expansionSize)
    {
        var lines = galaxy.Split(["\r", "\n"], StringSplitOptions.RemoveEmptyEntries).ToList();
        var additionalRows = lines.FindIndexes(line => line.All(c => c == '.')).ToList();
        var additionalColumns =
            Enumerable.Range(0, lines[0].Length).Where(index => lines.All(line => line[index] == '.')).ToList();

        for (int y = 0; y < lines.Count; y++)
        {
            for (int x = 0; x < lines[y].Length; x++)
            {
                if (lines[y][x] == GalaxySign)
                {
                    var yOffset = additionalRows.Count(row => y > row) * expansionSize;
                    var xOffset = additionalColumns.Count(column => x > column) * expansionSize;
                    var galaxyY = yOffset + y;
                    var galaxyX = xOffset + x;
                    Galaxies.Add(new Point(galaxyX, galaxyY));
                }
            }
        }
    }
    
    public long GetSumOfShortestPathsBetweenGalaxies()
    {
        long distance = 0;
        foreach (var permutationPair in GetPermutationsUnordered(Galaxies, 2))
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