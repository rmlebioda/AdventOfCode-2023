using System.Diagnostics;

namespace Day7;

[DebuggerDisplay("{DisplayValue}")]
public class Hand
{
    public string DisplayValue => string.Join("", Cards) + " | " + Bid + " | " + HandType + " | " + HandTypePartTwo;
    public List<char> Cards {get; set; }
    public long Bid { get; set; }

    public HandType HandType
    {
        get
        {
            if (Cards.Count != 5)
                throw new Exception("Invalid cards count: " + Cards.Count);

            var groupedByCards = Cards.GroupBy(card => card)
                .OrderByDescending(group => group.Count()).ToList();

            return groupedByCards.Count switch
            {
                1 when groupedByCards[0].Count() == 5 => HandType.FiveOfAKind,
                2 when groupedByCards[0].Count() == 4 => HandType.FourOfAKind,
                2 when groupedByCards[0].Count() == 3 => HandType.FullHouse,
                3 when groupedByCards[0].Count() == 3 => HandType.ThreeOfAKind,
                3 when groupedByCards[0].Count() == 2 => HandType.TwoPair,
                4 => HandType.OnePair,
                _ => HandType.HighCard
            };
        }
    }
    
    public HandType HandTypePartTwo
    {
        get
        {
            if (Cards.Count != 5)
                throw new Exception("Invalid cards count: " + Cards.Count);

            var groupedByCards = Cards.GroupBy(card => card)
                .OrderByDescending(group => group.Count()).ToList();

            return groupedByCards.Count switch
            {
                1 when groupedByCards[0].Count() == 5 => HandType.FiveOfAKind,
                2 when groupedByCards[0].Key == 'J' || groupedByCards[1].Key == 'J' => HandType.FiveOfAKind,
                2 when groupedByCards[0].Count() == 4 => HandType.FourOfAKind,
                2 when groupedByCards[0].Count() == 3 => HandType.FullHouse,
                3 when groupedByCards[0].Count() == 3 && (groupedByCards[0].Key == 'J' || groupedByCards[1].Key == 'J' || groupedByCards[2].Key == 'J') => HandType.FourOfAKind,
                3 when groupedByCards[0].Count() == 3 => HandType.ThreeOfAKind,
                3 when groupedByCards[0].Count() == 2 && (groupedByCards[0].Key == 'J' || groupedByCards[1].Key == 'J') => HandType.FourOfAKind,
                3 when groupedByCards[0].Count() == 2 && groupedByCards[2].Key == 'J' => HandType.FullHouse,
                3 when groupedByCards[0].Count() == 2 => HandType.TwoPair,
                4 when groupedByCards[0].Key == 'J' => HandType.ThreeOfAKind,
                4 when groupedByCards[1].Key == 'J' || groupedByCards[2].Key == 'J' || groupedByCards[3].Key == 'J' => HandType.ThreeOfAKind,
                4 => HandType.OnePair,
                _ when Cards.Contains('J') => HandType.OnePair,
                _ => HandType.HighCard
            };
        }
    }
}