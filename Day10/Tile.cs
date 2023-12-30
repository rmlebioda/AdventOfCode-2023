namespace Day10;

public enum Tile
{
    /// <summary>
    /// Doesn't exist
    /// </summary>
    None,
    Ground,
    HorizontalPipe,
    VerticalPipe,
    NorthEastPipe,
    NorthWestPipe,
    SouthWestPipe,
    SouthEastPipe,
    /// <summary>
    /// Treat this as + pipe
    /// </summary>
    StartingPosition
}