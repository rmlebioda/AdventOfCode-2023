using Day8;

// var filePath = "example.txt";
var filePath = "source.txt";
var fileContent = File.ReadAllText(filePath);

var lines = fileContent.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

string instructions = lines[0];
var networks = lines.Skip(1).Select(line => new Network(line)).ToList();

const string finalIdentifier = "ZZZ";

void FirstPart()
{
    int currentIndex = 0, iterations = 0;
    string currentIdentifier = "AAA";
    while (currentIdentifier != finalIdentifier)
    {
        var network = networks.First(network => network.Identifier == currentIdentifier);
        if (instructions[currentIndex] == 'L')
            currentIdentifier = network.Left;
        else if (instructions[currentIndex] == 'R')
            currentIdentifier = network.Right;
        else
            throw new InvalidProgramException($"Invalid instruction: `{instructions[currentIndex]}`");

        currentIndex = (currentIndex + 1) % instructions.Length;
        iterations++;
    }

    Console.WriteLine($"First part: `{finalIdentifier}` reached after {iterations} iterations");
}

// FirstPart();

void SecondPartBruteForce()
{
    // this executes forever, abandoning tactic...
    int currentIndex = 0, iterations = 0;
    var allIdentifiers = networks.Where(network => network.Identifier.EndsWith('A'))
        .Select(network => network.Identifier).ToList();
    while (allIdentifiers.Any(identifier => !identifier.EndsWith('Z')))
    {
        for (int i = 0; i < allIdentifiers.Count; i++)
        {
            var currentIdentifier = allIdentifiers[i];
            var network = networks.First(network => network.Identifier == currentIdentifier);
            if (instructions[currentIndex] == 'L')
                currentIdentifier = network.Left;
            else if (instructions[currentIndex] == 'R')
                currentIdentifier = network.Right;
            else
                throw new InvalidProgramException($"Invalid instruction: `{instructions[currentIndex]}`");

            allIdentifiers[i] = currentIdentifier;
        }
        
        currentIndex = (currentIndex + 1) % instructions.Length;
        iterations++;
    }
    Console.WriteLine($"Second part: `{finalIdentifier}` reached after {iterations} iterations");
}

static long gcf(long a, long b)
{
    while (b != 0)
    {
        long temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

static long lcm(long a, long b)
{
    return (a / gcf(a, b)) * b;
}

static long lcmRange(IEnumerable<long> numbers)
{
    var enumerator = numbers.GetEnumerator();
    if (!enumerator.MoveNext())
        throw new ArgumentException();
    var result = enumerator.Current;
    while (enumerator.MoveNext())
    {
        result = lcm(result, enumerator.Current);
    }

    return result;
}

void SecondPart()
{
    var allIdentifiers = networks.Where(network => network.Identifier.EndsWith('A'))
        .Select(network => network.Identifier).ToList();
    var results = new List<long>();
    while (allIdentifiers.Any())
    {
        var currentIdentifier = allIdentifiers[0];
        allIdentifiers.RemoveAt(0);
        
        int currentIndex = 0, iterations = 0;
        while (!currentIdentifier.EndsWith('Z'))
        {
            var network = networks.First(network => network.Identifier == currentIdentifier);
            if (instructions[currentIndex] == 'L')
                currentIdentifier = network.Left;
            else if (instructions[currentIndex] == 'R')
                currentIdentifier = network.Right;
            else
                throw new InvalidProgramException($"Invalid instruction: `{instructions[currentIndex]}`");

            currentIndex = (currentIndex + 1) % instructions.Length;
            iterations++;
        }
        results.Add(iterations);
    }
    Console.WriteLine($"Second part: `{finalIdentifier}` reached after {lcmRange(results)} iterations");
}

SecondPart();
