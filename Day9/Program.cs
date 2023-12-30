// var filePath = "example.txt";

var filePath = "source.txt";
var fileContent = File.ReadAllText(filePath);

var lines = fileContent.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

long ExtrapolateNumberFromRange(IList<long> numbers, bool next)
{
    if (numbers.Count < 2)
        throw new ArgumentException();

    // first step is to generate new sequence as subtraction of each next value from the previous one
    var newSequence = new List<long>();
    for (int i = 1; i < numbers.Count; i++)
        newSequence.Add(numbers[i] - numbers[i - 1]);

    if (newSequence.Any(number => number != 0))
    {
        var extrapolatedValue = ExtrapolateNumberFromRange(newSequence, next);
        if (next)
            return numbers.Last() + extrapolatedValue;
        else
            return numbers.First() - extrapolatedValue;
    }
    else
    {
        return next ? numbers.Last() : numbers.First();
    }
}

void FirstPart()
{
    var input = new List<List<long>>();
    foreach (var line in lines)
        input.Add(line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList());

    Console.WriteLine($"First part: {input.Select(number => ExtrapolateNumberFromRange(number, next: true)).Sum()}");
}

FirstPart();

void SecondPart()
{
    var input = new List<List<long>>();
    foreach (var line in lines)
        input.Add(line.Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList());

    Console.WriteLine($"Second part: {input.Select(number => ExtrapolateNumberFromRange(number, next: false)).Sum()}");
}

SecondPart();
