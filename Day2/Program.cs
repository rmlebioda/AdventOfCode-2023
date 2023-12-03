var file = "source.txt";

var fileContent = File.ReadAllText(file);

(int red, int green, int blue) maxSubset = (12, 13, 14);

long goodGamesSum = 0;
long powerOfPossibleSetsOfCubes = 0;
foreach (var line in fileContent.Split(new[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries))
{
    var id = line.Split(":")[0];
    var idGame = int.Parse(id.Split(" ")[1]);
    var sets = line.Split(":")[1];
    bool wasPossible = true;
    (int red, int green, int blue) lowestPossibleSubset = (0, 0, 0);
    foreach (var set in sets.Split(";"))
    {
        var cubes = set.Split(",");
        foreach (var cube in cubes)
        {
            var cubeId = cube.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (cubeId[1] == "red")
            {
                var redNumber = int.Parse(cubeId[0]);
                if (redNumber > maxSubset.red)
                    wasPossible = false;
                lowestPossibleSubset.red = Math.Max(lowestPossibleSubset.red, redNumber);
            }
            if (cubeId[1] == "blue")
            {
                var blueNumber = int.Parse(cubeId[0]);
                if (blueNumber > maxSubset.blue)
                    wasPossible = false;
                lowestPossibleSubset.blue = Math.Max(lowestPossibleSubset.blue, blueNumber);
            }
            if (cubeId[1] == "green")
            {
                var greenNumber = int.Parse(cubeId[0]);
                if (greenNumber > maxSubset.green)
                    wasPossible = false;
                lowestPossibleSubset.green = Math.Max(lowestPossibleSubset.green, greenNumber);
            }
        }
    }

    if (wasPossible)
        goodGamesSum += idGame;
    powerOfPossibleSetsOfCubes += (lowestPossibleSubset.red * lowestPossibleSubset.blue * lowestPossibleSubset.green);
}

Console.WriteLine("Max sum: " + goodGamesSum);
Console.WriteLine("Power of lowest set: " + powerOfPossibleSetsOfCubes);
