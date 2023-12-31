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
    public nint[][]? LoopVisits { get; private set; }

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
        if (LoopVisits is not null)
            return LoopVisits.Max(seq => seq.Max());
        LoopVisits = Create2DArray<nint>(Y, X);
        Fill2DArray(LoopVisits, NotVisitedTileValue);
        var startingPoint = GetStartingPosition();
        var tilesToVisit = new List<Point> { startingPoint };
        nint turns = 0;
        LoopVisits[startingPoint.Y][startingPoint.X] = turns;

        while (tilesToVisit.Any())
        {
            tilesToVisit = Visit(tilesToVisit, turns).ToList();

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
    /// <param name="fillValue">Fill value for visiting points in array</param>
    /// <returns>New points to visit on next turn</returns>
    public IEnumerable<Point> Visit(IEnumerable<Point> points, IntPtr fillValue)
    {
        foreach (var point in points)
        {
            // we are checking, if pipes on west, south, north or east are connected with current point pipe
            if (IsConnected(point, VisitDirection.Left) && LoopVisits![point.Y][point.X - 1] == NotVisitedTileValue)
            {
                var newPoint = new Point(point.X - 1, point.Y);
                LoopVisits![newPoint.Y][newPoint.X] = fillValue;
                yield return newPoint;
            }

            if (IsConnected(point, VisitDirection.Right) && LoopVisits![point.Y][point.X + 1] == NotVisitedTileValue)
            {
                var newPoint = new Point(point.X + 1, point.Y);
                LoopVisits![newPoint.Y][newPoint.X] = fillValue;
                yield return newPoint;
            }

            if (IsConnected(point, VisitDirection.North) && LoopVisits![point.Y - 1][point.X] == NotVisitedTileValue)
            {
                var newPoint = new Point(point.X, point.Y - 1);
                LoopVisits![newPoint.Y][newPoint.X] = fillValue;
                yield return newPoint;
            }

            if (IsConnected(point, VisitDirection.South) && LoopVisits![point.Y + 1][point.X] == NotVisitedTileValue)
            {
                var newPoint = new Point(point.X, point.Y + 1);
                LoopVisits![newPoint.Y][newPoint.X] = fillValue;
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

    private bool IsPartOfLoop(Point p)
    {
        return LoopVisits![p.Y][p.X] != NotVisitedTileValue;
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

    /// <summary>
    /// Retrieves amount of tiles enclosed by loop. Includes non-connected pipes.
    /// </summary>
    public int GetTilesEnclosedByLoop()
    {
        if (LoopVisits == null)
            _ = GetFurthestPointFromStart();
        
        int tiles = 0;
        for (int y = 0; y < Y; y++)
        {
            for (int x = 0; x < X; x++)
            {
                if ((Tiles[y][x] != Tile.None && LoopVisits![y][x] == NotVisitedTileValue) && GetCrossingPointsFromLeftEdge(new Point(x, y)) % 2 == 1)
                    tiles++;
            }
        }

        return tiles;
    }

    private int GetCrossingPointsFromLeftEdge(Point point)
    {
        int result = 0;
        // we store visit direction to know, if we are continuing crossing a wall and from which direction (north/south)
        VisitDirection? wallVisitDirection = null;
        for (int x = 0; x < point.X; x++)
        {
            var cp = new Point(x, point.Y);
            var tileValue = Tiles[point.Y][x];
            if (tileValue == Tile.StartingPosition)
                tileValue = FindValidTileForPoint(new Point(x, point.Y));

            if (tileValue == Tile.VerticalPipe && IsPartOfLoop(cp))
                result++;
            else if (tileValue == Tile.NorthEastPipe && IsPartOfLoop(cp))
                wallVisitDirection = VisitDirection.North;
            else if (tileValue == Tile.SouthEastPipe && IsPartOfLoop(cp))
                wallVisitDirection = VisitDirection.South;
            else if (tileValue == Tile.NorthWestPipe && IsPartOfLoop(cp))
            {
                if (wallVisitDirection == VisitDirection.North)
                    result += 2;
                else
                    result++;
                wallVisitDirection = null;
            }
            else if (tileValue == Tile.SouthWestPipe && IsPartOfLoop(cp))
            {
                if (wallVisitDirection == VisitDirection.South)
                    result += 2;
                else
                    result++;
                wallVisitDirection = null;
            }
        }

        return result;
    }

    private Tile FindValidTileForPoint(Point point)
    {
        if (point.X > 0 &&
            ((Tile[]) [Tile.NorthEastPipe, Tile.SouthEastPipe, Tile.HorizontalPipe])
            .Contains(Tiles[point.Y][point.X - 1]))
        {
            return FindValidTileForPointFromWest(point);
        }
        
        if (point.X < (X - 1) &&
            ((Tile[]) [Tile.NorthWestPipe, Tile.SouthWestPipe, Tile.HorizontalPipe])
            .Contains(Tiles[point.Y][point.X + 1]))
        {
            return FindValidTileForPointFromEast(point);
        }
        
        if (point.Y > 0 &&
            ((Tile[]) [Tile.SouthEastPipe, Tile.SouthWestPipe, Tile.VerticalPipe])
            .Contains(Tiles[point.Y - 1][point.X]))
        {
            return FindValidTileForPointFromNorth(point);
        }
        
        if (point.Y < (Y - 1) &&
            ((Tile[]) [Tile.NorthEastPipe, Tile.NorthWestPipe, Tile.VerticalPipe])
            .Contains(Tiles[point.Y + 1][point.X]))
        {
            return FindValidTileForPointFromSouth(point);
        }
        
        throw new InvalidOperationException();
    }

    private Tile FindValidTileForPointFromWest(Point point)
    {
        if (point.X < (X - 1) && ((Tile[]) [Tile.NorthWestPipe, Tile.SouthWestPipe, Tile.HorizontalPipe])
            .Contains(Tiles[point.Y][point.X + 1]))
        {
            return Tile.HorizontalPipe;
        }

        if (point.Y < (Y - 1) && ((Tile[]) [Tile.NorthEastPipe, Tile.NorthWestPipe, Tile.VerticalPipe])
            .Contains(Tiles[point.Y + 1][point.X]))
        {
            return Tile.SouthWestPipe;
        }

        if (point.Y > 0 && ((Tile[]) [Tile.SouthEastPipe, Tile.SouthWestPipe, Tile.VerticalPipe])
            .Contains(Tiles[point.Y - 1][point.X]))
        {
            return Tile.NorthWestPipe;
        }

        throw new InvalidOperationException();
    }
    
    private Tile FindValidTileForPointFromEast(Point point)
    {
        if (point.X > 0 && ((Tile[]) [Tile.NorthEastPipe, Tile.SouthEastPipe, Tile.HorizontalPipe])
            .Contains(Tiles[point.Y][point.X - 1]))
        {
            return Tile.HorizontalPipe;
        }

        if (point.Y < (Y - 1) && ((Tile[]) [Tile.NorthEastPipe, Tile.NorthWestPipe, Tile.VerticalPipe])
            .Contains(Tiles[point.Y + 1][point.X]))
        {
            return Tile.SouthEastPipe;
        }

        if (point.Y > 0 && ((Tile[]) [Tile.SouthEastPipe, Tile.SouthWestPipe, Tile.VerticalPipe])
            .Contains(Tiles[point.Y - 1][point.X]))
        {
            return Tile.NorthEastPipe;
        }

        throw new InvalidOperationException();
    }

    private Tile FindValidTileForPointFromNorth(Point point)
    {
        if (point.Y < (Y - 1) && ((Tile[]) [Tile.NorthWestPipe, Tile.NorthEastPipe, Tile.VerticalPipe])
            .Contains(Tiles[point.Y + 1][point.X]))
        {
            return Tile.VerticalPipe;
        }

        if (point.X < (X - 1) && ((Tile[]) [Tile.NorthWestPipe, Tile.SouthWestPipe, Tile.HorizontalPipe])
            .Contains(Tiles[point.Y][point.X + 1]))
        {
            return Tile.NorthEastPipe;
        }

        if (point.X > 0 && ((Tile[]) [Tile.NorthEastPipe, Tile.SouthEastPipe, Tile.HorizontalPipe])
            .Contains(Tiles[point.Y][point.X - 1]))
        {
            return Tile.NorthWestPipe;
        }

        throw new InvalidOperationException();
    }
    
    private Tile FindValidTileForPointFromSouth(Point point)
    {
        if (point.Y > 0 && ((Tile[]) [Tile.SouthEastPipe, Tile.SouthWestPipe, Tile.VerticalPipe])
            .Contains(Tiles[point.Y - 1][point.X]))
        {
            return Tile.VerticalPipe;
        }

        if (point.X < (X - 1) && ((Tile[]) [Tile.NorthWestPipe, Tile.SouthWestPipe, Tile.HorizontalPipe])
            .Contains(Tiles[point.Y][point.X + 1]))
        {
            return Tile.SouthEastPipe;
        }

        if (point.X > 0 && ((Tile[]) [Tile.SouthEastPipe, Tile.NorthEastPipe, Tile.HorizontalPipe])
            .Contains(Tiles[point.Y][point.X - 1]))
        {
            return Tile.SouthWestPipe;
        }

        throw new InvalidOperationException();
    }
}
