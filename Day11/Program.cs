using Day11;

// var filePath = "example.txt";
// var filePath = "example2.txt";
var filePath = "source.txt";
var fileContent = File.ReadAllText(filePath);

var space = new Space(fileContent);
Console.WriteLine($"First part: {space.GetSumOfShortestPathsBetweenGalaxies()}");

// this throws OurOfMemoryException
// var newSpace = new Space(fileContent, (int)1e6);

var spaceV2 = new SpaceSimplified(fileContent, 1);
Console.WriteLine($"First part v2: {spaceV2.GetSumOfShortestPathsBetweenGalaxies()}");
// the REASON for subtracting one on next executing line is this part of text:
// "make each empty row or column one million times larger.
// That is, each empty row should be replaced with 1000000 empty rows,
// and each empty column should be replaced with 1000000 empty columns."
// because my argument expansionSize INCREASES number of rows instead of REPLACING,
// it has to be subtracted by one from original value
var newSpace = new SpaceSimplified(fileContent, (int)1e6 - 1);
Console.WriteLine($"Second part: {newSpace.GetSumOfShortestPathsBetweenGalaxies()}");
