using System.Diagnostics;

namespace AdventOfCode.Day3;

[DebuggerDisplay("{Value} | {X}/{Y}")]
public class SpecialSymbol
{
    public SpecialSymbol(char value, int x, int y)
    {
        Value = value;
        X = x;
        Y = y;
    }

    public char Value { get; }
    public int X { get; }
    public int Y { get; }
}