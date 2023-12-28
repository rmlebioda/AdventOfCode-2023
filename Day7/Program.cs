using Day7;

// var filePath = "example.txt";
var filePath = "source.txt";
var fileContent = File.ReadAllText(filePath);

var lines = fileContent.Split(new string[] {"\r", "\n"}, StringSplitOptions.RemoveEmptyEntries);

var hands = lines.Select(line => new Hand
    {Bid = long.Parse(line.Split(' ')[1]), Cards = line.Split(' ')[0].ToCharArray().ToList()}).ToList();

var handsByStrength = hands.Order(new HandsComparer());

long result = 0;
foreach (var handWithIndex in handsByStrength.Select((x, i) => new {Hand=x, Index=i}))
{
    result += handWithIndex.Hand.Bid * (handWithIndex.Index + 1);
}

Console.WriteLine("First part: " + result);

var handsByStrengthWithJoker = hands.Order(new HandsComparerPartTwo());

result = 0;
var tmpHandsByStrengthWithJoker = handsByStrengthWithJoker.Select((x, i) => new { Hand = x, Index = i }).ToList();
foreach (var handWithIndex in tmpHandsByStrengthWithJoker)
{
    result += handWithIndex.Hand.Bid * (handWithIndex.Index + 1);
}

Console.WriteLine("Second part: " + result);
