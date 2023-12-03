using System.Diagnostics;

namespace AdventOfCode.Day3;

[DebuggerDisplay("{Value} | {X}/{Y}")]
public struct Number
{
    public long Value { get; }
    public int X { get; }
    public int Y { get; }

    public Number(string value, int x, int y)
    {
        Value = long.Parse(value);
        X = x;
        Y = y;
    }
}
