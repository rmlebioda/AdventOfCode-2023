using System.Diagnostics.Contracts;
using Day5;

// var filePath = "example.txt";
var filePath = "source.txt";
var fileContent = File.ReadAllText(filePath);

var fileLines = fileContent.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
const string seedIdentifier = "seed";
const string seedsListIdentifier = "seeds:";
var conversionsIdentifiers = new List<string>
{
    "seed-to-soil map:", "soil-to-fertilizer map:", "fertilizer-to-water map:", "water-to-light map:",
    "light-to-temperature map:", "temperature-to-humidity map:", "humidity-to-location map:"
};

var seeds = fileLines.First(line => line.StartsWith(seedsListIdentifier, StringComparison.Ordinal)).Split(":")[1]
    .Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(seed => long.Parse(seed)).ToList();


var namingConversions = new Dictionary<string, string>();
var uniqueConversions = new List<SingularData>();

foreach (var conversionIdentifier in conversionsIdentifiers)
{
    var startLine = fileLines.FindIndex(line => line.StartsWith(conversionIdentifier, StringComparison.Ordinal));
    var source = fileLines[startLine].Split("-to-")[0];
    var destination = fileLines[startLine].Split("-to-")[1].Split(' ')[0];
    namingConversions.Add(source, destination);
    for (var i = startLine + 1; i < fileLines.Count; i++)
    {
        var line = fileLines[i];
        if (!char.IsDigit(line.FirstOrDefault()))
            break;
        var values =
            line.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(value => long.Parse(value)).ToList();
        Contract.Requires(values.Count == 3, "Invalid line: " + line);
        var destinationStart = values[0];
        var sourceStart = values[1];
        var length = values[2];
        uniqueConversions.Add(new SingularData(source, sourceStart, destinationStart, length));
    }
}

SingularData? FindMatchingDataForSource(string identifier, long source)
{
    var matchedData = uniqueConversions!
        .Where(data => data.SourceIdentifier == identifier && source >= data.SourceStart &&
                       (data.SourceStart + data.Length - 1) >= source).ToList();

    if (matchedData.Any())
        return matchedData.First();
    return null;
}

const string targetIdentifier = "location";
long lowestTarget = long.MaxValue;
foreach (var seed in seeds)
{
    string currentSourceIdentifier = seedIdentifier;
    long currentSource = seed;
    int antiLoop = 0;
    while (currentSourceIdentifier != targetIdentifier)
    {
        var matchedData = FindMatchingDataForSource(currentSourceIdentifier, currentSource);
        if (matchedData.HasValue)
        {
            currentSource = matchedData.Value.DestinationStart + (currentSource - matchedData.Value.SourceStart);
        }
        
        // we are just changing source name according to task
        currentSourceIdentifier = namingConversions[currentSourceIdentifier];

        antiLoop++;
        Contract.Requires(antiLoop < 100, "Infinite loop, check conversions for seed: " + seed);
    }

    lowestTarget = Math.Min(lowestTarget, currentSource);
}

Contract.Requires(lowestTarget == long.MaxValue, "Failed to find lowest humidity");

Console.WriteLine("Part one: Lowest " + targetIdentifier + ": " + lowestTarget);

// ======================== PART 2 ===========================
// brute forcing takes too much time, so instead I'm changing my tactic
// I'm starting from locations, trying to find the first seed, that is in my input data

SingularData? FindMatchingDataForDestination(string identifier, long destination)
{
    var matchedData = uniqueConversions!
        .Where(data => data.SourceIdentifier == identifier && destination >= data.DestinationStart &&
                       (data.DestinationStart + data.Length - 1) >= destination).ToList();

    if (matchedData.Any())
        return matchedData.First();
    return null;
}
lowestTarget = long.MaxValue;

var seedsData = new List<SeedData>();
foreach (var chunk in seeds.Chunk(2))
    seedsData.Add(new SeedData{ Start = chunk[0], Length = chunk[1] });

bool IsInSeedsData(long value)
{
    return seedsData!.Any(seedData => value >= seedData.Start && (seedData.Start + seedData.Length) >= value);
}

var namingConversionsReversed = namingConversions.ToDictionary(x => x.Value, x => x.Key);
for (long location = 0; true; location++)
{
    string currentDestinationIdentifier = targetIdentifier;
    var currentDestination = location;
    while (currentDestinationIdentifier != seedIdentifier)
    {
        currentDestinationIdentifier = namingConversionsReversed[currentDestinationIdentifier];
        
        var matchedData = FindMatchingDataForDestination(currentDestinationIdentifier, currentDestination);
        if (matchedData.HasValue)
        {
            currentDestination = matchedData.Value.SourceStart + (currentDestination - matchedData.Value.DestinationStart);
        }
    }

    if (IsInSeedsData(currentDestination))
    {
        lowestTarget = location;
        break;
    }
}

Contract.Requires(lowestTarget == long.MaxValue, "Failed to find lowest humidity");

Console.WriteLine("Part two: Lowest " + targetIdentifier + ": " + lowestTarget);