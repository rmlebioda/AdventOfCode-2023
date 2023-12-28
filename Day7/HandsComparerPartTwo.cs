namespace Day7;

public class HandsComparerPartTwo : IComparer<Hand>
{
    public static readonly List<char> cards = new List<char> { 'A', 'K', 'Q', 'T', '9', '8', '7', '6', '5', '4', '3', '2', 'J' };
                                  
    public int Compare(Hand? x, Hand? y)
    {
        if (ReferenceEquals(x, y))
            return 0;
        if (ReferenceEquals(x, null))
            return -1;
        if (ReferenceEquals(y, null))
            return 1;
        if (x.GetType() != y.GetType())
            return 0;
        
        if (x.Cards.Count != 5 || y.Cards.Count != x.Cards.Count)
            throw new ArgumentException("Invalid hands");
        
        if (x.HandTypePartTwo > y.HandTypePartTwo)
            return 1;
        if (y.HandTypePartTwo > x.HandTypePartTwo)
            return -1;

        for (int i = 0; i < x.Cards.Count; i++)
        {
            var xIndex = cards.IndexOf(x.Cards[i]);
            var yIndex = cards.IndexOf(y.Cards[i]);
            if (xIndex < yIndex)
                return 1;
            if (yIndex < xIndex)
                return -1;
        }

        return 0;
    }
}