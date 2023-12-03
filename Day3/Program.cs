using System.Text;
using AdventOfCode.Day3;

// var sourceFilePath = "example.txt";
var sourceFilePath = "source.txt";
var fileContent = File.ReadAllText(sourceFilePath);

var numbers = new List<Number>();
var symbols = new List<SpecialSymbol>();

int y = 0;
foreach (var line in fileContent.Split(new[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries))
{
    var numberBuilder = new StringBuilder();
    var numberStartX = 0;
    for (int x = 0; x < line.Length; x++)
    {
        var character = line[x];
        if (char.IsDigit(character))
        {
            if (numberBuilder.Length == 0)
                numberStartX = x;
            numberBuilder.Append(character);
        }
        else
        {
            if (numberBuilder.Length > 0)
            {
                numbers.Add(new Number(numberBuilder.ToString(), numberStartX, y));
                numberBuilder.Clear();
            }
            if (character != '.')
                symbols.Add(new SpecialSymbol(character, x, y));
        }
    }

    if (numberBuilder.Length > 0)
    {
        numbers.Add(new Number(numberBuilder.ToString(), numberStartX, y));
        numberBuilder.Clear();
    }
    y++;
}

bool IsPartNumber(Number number)
{
    var result = symbols.Any(symbol => IsPartNumberAdjacentForSymbol(number, symbol));
    return result;
}

bool IsPartNumberAdjacentForSymbol(Number number, SpecialSymbol symbol)
{
    int lowerX = number.X - 1;
    int upperX = number.X + 1 + (number.Value.ToString().Length - 1);
    int lowerY = number.Y - 1;
    int upperY = number.Y + 1;
    
    return symbol.X >= lowerX && symbol.X <= upperX && symbol.Y >= lowerY && symbol.Y <= upperY;
}

long SecondPart()
{
    var gearSymbols = symbols.Where(symbol => symbol.Value == '*');
    long sum = 0;
    foreach (var symbol in gearSymbols)
    {
        var adjacentNumbers = numbers.Where(number => IsPartNumberAdjacentForSymbol(number, symbol)).ToList();
        if (adjacentNumbers.Count > 1)
            sum += adjacentNumbers.Aggregate((long)1, (x, y) => x * y.Value);
    }

    return sum;
}

Console.WriteLine("First part: " + numbers.Where(IsPartNumber).Select(number => number.Value).Sum());

Console.WriteLine("Second part: " + SecondPart());