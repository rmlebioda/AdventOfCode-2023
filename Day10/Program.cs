using Day10;

// var filePath = "example.txt";
// var filePath = "example2.txt";
var filePath = "source.txt";
var fileContent = File.ReadAllText(filePath);

var map = new Map(fileContent);

Console.WriteLine($"First part: {map.GetFurthestPointFromStart()}");

Console.WriteLine($"Second part: {map.GetTilesEnclosedByLoop()}");
