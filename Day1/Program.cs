using Day1;

var filePath = "source.txt";

var parser = new CalibrationDocumentParser(filePath);

Console.WriteLine("Without spelled letters: " + parser.GetSum(useSpelledValidDigits: false));

Console.WriteLine("With spelled letters: " + parser.GetSum(useSpelledValidDigits: true));

