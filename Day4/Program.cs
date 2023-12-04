using System.Diagnostics.Contracts;

var filePath = "source.txt";
var fileContent = File.ReadAllText(filePath);

long points = 0;
foreach (var line in fileContent.Split(new string[] {"\r", "\n"}, StringSplitOptions.RemoveEmptyEntries))
{
    // var cardTextWithId = line.Split(':')[0];
    var winsWithResults = line.Split(':')[1];
    var wins = winsWithResults.Split('|')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse)
        .ToList();
    var hits = winsWithResults.Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse)
        .ToList();
    Contract.Requires(wins.Count == wins.Distinct().Count(), $"There wins are duplicates in line: {line}");
    Contract.Requires(hits.Count == hits.Distinct().Count(), $"There hits are duplicates in line: {line}");

    var winHits = wins.Intersect(hits).ToList();
    if (winHits.Any())
        points += (long) Math.Pow(2, winHits.Count - 1);
}

Console.WriteLine("Part 1: points: " + points);

// ----------- part 2
var cardsIdCopyCount = new Dictionary<long, long>();

void Add(Dictionary<long, long> dict, long key, long value)
{
    if (dict.ContainsKey(key))
        dict[key] += value;
    else
        dict[key] = value;
}

long scratchcards = 0;
foreach (var line in fileContent.Split(new string[] {"\r", "\n"}, StringSplitOptions.RemoveEmptyEntries))
{
    var cardTextWithId = line.Split(':')[0];
    var cardId = long.Parse(cardTextWithId.Split(' ', StringSplitOptions.RemoveEmptyEntries)[1]);
    var winsWithResults = line.Split(':')[1];
    var wins = winsWithResults.Split('|')[0].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse)
        .ToList();
    var hits = winsWithResults.Split('|')[1].Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse)
        .ToList();
    Contract.Requires(wins.Count == wins.Distinct().Count(), $"There wins are duplicates in line: {line}");
    Contract.Requires(hits.Count == hits.Distinct().Count(), $"There hits are duplicates in line: {line}");

    var noOfScratchcards = 1 + (cardsIdCopyCount.TryGetValue(cardId, out var value) ? value : 0);
    var winHits = wins.Intersect(hits).ToList();
    for (long i = 1; i <= winHits.Count; i++)
        Add(cardsIdCopyCount, cardId + i, noOfScratchcards);

    scratchcards += noOfScratchcards;
}

Console.WriteLine("Part 2: scratchcards: " + scratchcards);
