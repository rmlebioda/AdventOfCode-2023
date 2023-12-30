using Day10;

var filePath = "example.txt";
// var filePath = "source.txt";
var fileContent = File.ReadAllText(filePath);

var map = new Map(fileContent);

try
{
    Console.WriteLine($"First part: {map.GetFurthestPointFromStart()}");
}
catch (Exception e)
{
    Console.WriteLine(e);
    throw;
}