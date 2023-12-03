namespace Day1;

public class CalibrationDocumentParser
{
    public string Content { get; set; }

    private Dictionary<string, int> SpelledValidDigits = new Dictionary<string, int>()
    {
        { "one", 1 }, { "two", 2 }, { "three", 3 }, { "four", 4 }, { "five", 5 },
        { "six", 6 }, { "seven", 7 }, { "eight", 8 }, { "nine", 9 }
    };

    public CalibrationDocumentParser(string path)
    {
        Content = File.ReadAllText(path);
    }
    
    public static string Reverse( string s )
    {
        char[] charArray = s.ToCharArray();
        Array.Reverse(charArray);
        return new string(charArray);
    }

    public long GetSum(bool useSpelledValidDigits = false)
    {
        long sum = 0;

        foreach (var line in Content.Split(new string[] { "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries))
        {
            var firstValidDigitIndex = line.TakeWhile(character => !char.IsDigit(character)).Count();
            string firstValue = line[firstValidDigitIndex].ToString();
            if (useSpelledValidDigits)
            {
                var tmp = GetIndexOfSpelledValidDigits(line, true);
                if (tmp.HasValue && tmp.Value.Item1 < firstValidDigitIndex)
                    firstValue = tmp.Value.Item2.ToString();
            }

            var reversedLine = Reverse(line);
            var lastValidDigitIndex = reversedLine.TakeWhile(character => !char.IsDigit(character)).Count();
            string lastValue = reversedLine[lastValidDigitIndex].ToString();
            if (useSpelledValidDigits)
            {
                var tmp = GetIndexOfSpelledValidDigits(line, false);
                if (tmp.HasValue && (line.Length - tmp.Value.Item3) < lastValidDigitIndex)
                    lastValue = tmp.Value.Item2.ToString();
            }

            sum += long.Parse(firstValue + lastValue);
        }
        
        return sum;
    }

    private (int startIndex, int value, int lastIndex)? GetIndexOfSpelledValidDigits(string line, bool first)
    {
        (int, int, int)? bestIndex = null;
        foreach (var keyPair in SpelledValidDigits)
        {
            int index;
            
            if (first)
                index = line.IndexOf(keyPair.Key, StringComparison.InvariantCulture);
            else
                index = line.LastIndexOf(keyPair.Key, StringComparison.InvariantCulture);

            if (index != -1)
            {
                if (!bestIndex.HasValue)
                {
                    bestIndex = (index, keyPair.Value, index + keyPair.Key.Length - 1);
                }
                else
                {
                    if ((first && index < bestIndex.Value.Item1) || (!first && index > bestIndex.Value.Item1))
                    {
                        bestIndex = (index, keyPair.Value, index + keyPair.Key.Length - 1);
                    }
                }
            }
        }

        return bestIndex;
    }
}