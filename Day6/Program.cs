// var filePath = "example.txt";
var filePath = "source.txt";
var fileContent = File.ReadAllText(filePath);

var lines = fileContent.Split(new string[] { "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();
var times = lines.First(line => line.StartsWith("Time:")).Split(":")[1]
    .Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
var distances = lines.First(line => line.StartsWith("Distance:")).Split(":")[1]
    .Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

var beatableRecords = new List<long>();

for (int i = 0; i < times.Count; i++)
{
    var maxTime = times[i];
    var distanceRecord = distances[i];
    var beatableTimesCount = 0;
    for (int chargeTime = 0; chargeTime <= maxTime; chargeTime++)
    {
        var distance = chargeTime * (maxTime - chargeTime);
        if (distance > distanceRecord)
            beatableTimesCount++;
    }
    beatableRecords.Add(beatableTimesCount);
}

Console.WriteLine("Part one: " + beatableRecords.Aggregate((long)1, (acc, num) => acc*num));

// part 2
times = lines.First(line => line.StartsWith("Time:")).Split(":")[1].Replace(" ", "")
    .Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();
distances = lines.First(line => line.StartsWith("Distance:")).Split(":")[1].Replace(" ", "")
    .Split(" ", StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToList();

beatableRecords = new List<long>();

for (int i = 0; i < times.Count; i++)
{
    var maxTime = times[i];
    var distanceRecord = distances[i];
    var beatableTimesCount = 0;
    for (int chargeTime = 0; chargeTime <= maxTime; chargeTime++)
    {
        var distance = chargeTime * (maxTime - chargeTime);
        if (distance > distanceRecord)
            beatableTimesCount++;
    }
    beatableRecords.Add(beatableTimesCount);
}

Console.WriteLine("Part two: " + beatableRecords.Aggregate((long)1, (acc, num) => acc*num));

