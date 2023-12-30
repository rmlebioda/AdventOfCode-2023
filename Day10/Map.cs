using System.Drawing;

namespace Day10;

public class Map
{
    public int X { get; }
    public int Y { get; }

    /// <summary>
    /// Map tiles with indexes [Y][X]
    /// </summary>
    public Tile[][] Tiles { get; }

    public Map(string map)
    {
        var lines = map.Split(["\r", "\n"], StringSplitOptions.RemoveEmptyEntries);
        X = lines.Max(line => line.Length);
        Y = lines.Length;
        Tiles = CreateTiles(Y, X);

        for (int y = 0; y < lines.Length; y++)
        {
            var line = lines[y];
            for (int x = 0; x < line.Length; x++)
            {
                Tiles[y][x] = GetTile(line, x);
            }
        }
    }

    private static Tile[][] CreateTiles(int Y, int X)
    {
        return Create2DArray<Tile>(Y, X);
    }

    private static T[][] Create2DArray<T>(int Y, int X)
    {
        var array = new T[Y][];
        for (int y = 0; y < Y; y++)
            array[y] = new T[X];
        return array;
    }

    private static void Fill2DArray<T>(T[][] array, T value)
    {
        for (int y = 0; y < array.Length; y++)
        {
            for (int x = 0; x < array[y].Length; x++)
            {
                array[y][x] = value;
            }
        }
    }

    private Tile GetTile(string line, int x)
    {
        var value = x < line.Length ? line[x] : ' ';
        return value switch
        {
            '|' => Tile.VerticalPipe,
            '-' => Tile.HorizontalPipe,
            'J' => Tile.NorthWestPipe,
            'L' => Tile.NorthEastPipe,
            '7' => Tile.SouthWestPipe,
            'F' => Tile.SouthEastPipe,
            'S' => Tile.StartingPosition,
            '.' => Tile.Ground,
            ' ' => Tile.None,
            _ => throw new ArgumentException()
        };
    }

    private const nint NotVisitedTileValue = -1;

    /// <summary>
    /// Finds furthest point from start using BFS algorithm
    /// </summary>
    public nint GetFurthestPointFromStart()
    {
        var visitedTiles = Create2DArray<nint>(Y, X);
        Fill2DArray(visitedTiles, NotVisitedTileValue);
        var startingPoint = GetStartingPosition();
        var tilesToVisit = new List<Point> { startingPoint };
        nint turns = 0;
        visitedTiles[startingPoint.Y][startingPoint.X] = turns;

        while (tilesToVisit.Any())
        {
            tilesToVisit = Visit(tilesToVisit, visitedTiles, turns).ToList();

            if (tilesToVisit.Any())
                turns++;
        }

        return turns;
    }

    public Point GetStartingPosition()
    {
        for (int y = 0; y < Y; y++)
        {
            for (int x = 0; x < X; x++)
            {
                if (Tiles[y][x] == Tile.StartingPosition)
                    return new Point(x, y);
            }
        }

        throw new InvalidOperationException("Map does not contain starting point");
    }

    /// <summary>
    /// Visits sets of points and returns new points to be visited on next turn
    /// </summary>
    /// <param name="points">Points to visit</param>
    /// <param name="visitedTiles">Array containing visited points</param>
    /// <param name="fillValue">Fill value for visiting points in array</param>
    /// <returns>New points to visit on next turn</returns>
    public IEnumerable<Point> Visit(IEnumerable<Point> points, IntPtr[][] visitedTiles, IntPtr fillValue)
    {
        foreach (var point in points)
        {
            // we are checking, if pipes on west, south, north or east are connected with current point pipe
            if (IsConnected(point, VisitDirection.Left) && visitedTiles[point.Y][point.X - 1] != NotVisitedTileValue)
            {
                var newPoint = new Point(point.X - 1, point.Y);
                visitedTiles[newPoint.Y][newPoint.X] = fillValue;
                yield return newPoint;
            }

            if (IsConnected(point, VisitDirection.Right) && visitedTiles[point.Y][point.X + 1] != NotVisitedTileValue)
            {
                var newPoint = new Point(point.X + 1, point.Y);
                visitedTiles[newPoint.Y][newPoint.X] = fillValue;
                yield return newPoint;
            }

            if (IsConnected(point, VisitDirection.North) && visitedTiles[point.Y + 1][point.X] != NotVisitedTileValue)
            {
                var newPoint = new Point(point.X, point.Y + 1);
                visitedTiles[newPoint.Y][newPoint.X] = fillValue;
                yield return newPoint;
            }

            if (IsConnected(point, VisitDirection.South) && visitedTiles[point.Y - 1][point.X] != NotVisitedTileValue)
            {
                var newPoint = new Point(point.X, point.Y + 1);
                visitedTiles[newPoint.Y][newPoint.X] = fillValue;
                yield return newPoint;
            }
        }
    }

    private bool IsConnected(Point point, VisitDirection visitDirection)
    {
        if (visitDirection == VisitDirection.Left)
        {
            return IsConnectedToLeft(point);
        }

        if (visitDirection == VisitDirection.Right)
        {
            return IsConnectedToRight(point);
        }

        if (visitDirection == VisitDirection.North)
        {
            return IsConnectedToUp(point);
        }

        if (visitDirection == VisitDirection.South)
        {
            return IsConnectedToDown(point);
        }
        

        throw new ArgumentException("Invalid visit direction: " + visitDirection);
    }

    private bool IsConnectedToLeft(Point point)
    {
        if (point.X <= 0)
            return false;

        if (((Tile[]) [Tile.StartingPosition, Tile.HorizontalPipe, Tile.NorthWestPipe, Tile.SouthWestPipe])
            .Contains(Tiles[point.Y][point.X]) &&
            ((Tile[]) [Tile.StartingPosition, Tile.HorizontalPipe, Tile.NorthEastPipe, Tile.SouthEastPipe]).Contains(
                Tiles[point.Y][point.X - 1]))
            return true;

        return false;
    }
    
    private bool IsConnectedToRight(Point point)
    {
        if (point.X >= (X - 1))
            return false;

        if (((Tile[]) [Tile.StartingPosition, Tile.HorizontalPipe, Tile.NorthEastPipe, Tile.SouthEastPipe])
            .Contains(Tiles[point.Y][point.X]) &&
            ((Tile[]) [Tile.StartingPosition, Tile.HorizontalPipe, Tile.NorthWestPipe, Tile.SouthWestPipe]).Contains(
                Tiles[point.Y][point.X + 1]))
            return true;

        return false;
    }
    
    private bool IsConnectedToUp(Point point)
    {
        if (point.Y <= 0)
            return false;

        if (((Tile[]) [Tile.StartingPosition, Tile.VerticalPipe, Tile.NorthEastPipe, Tile.NorthWestPipe])
            .Contains(Tiles[point.Y][point.X]) &&
            ((Tile[]) [Tile.StartingPosition, Tile.VerticalPipe, Tile.SouthEastPipe, Tile.SouthWestPipe]).Contains(
                Tiles[point.Y - 1][point.X]))
            return true;

        return false;
    }
    
    private bool IsConnectedToDown(Point point)
    {
        if (point.Y >= (Y - 1))
            return false;

        if (((Tile[]) [Tile.StartingPosition, Tile.VerticalPipe, Tile.SouthEastPipe, Tile.SouthWestPipe])
            .Contains(Tiles[point.Y][point.X]) &&
            ((Tile[]) [Tile.StartingPosition, Tile.VerticalPipe, Tile.NorthEastPipe, Tile.NorthWestPipe]).Contains(
                Tiles[point.Y + 1][point.X]))
            return true;

        return false;
    }
}