namespace Day8;

public class Network
{
    public string Identifier { get; set; }
    public string Left { get; }
    public string Right { get; }
    
    public Network(string value)
    {
        Identifier = value.Split("=")[0].Trim();
        var leftRight = value.Split("(")[1].Split(")")[0];
        Left = leftRight.Split(",")[0].Trim();
        Right = leftRight.Split(",")[1].Trim();
    }
}