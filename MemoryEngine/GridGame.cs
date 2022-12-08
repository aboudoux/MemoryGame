using MemoryEngine.Exceptions;

namespace MemoryEngine;

public class GridGame
{
    public GridGame(params Player[] players)
    {
        if (players.Length < 2)
            throw new NotEnoughPlayerException();

        if (players.Distinct().Count() != players.Length)
            throw new SamePlayerException();

        List<Card> cards = new List<Card>();
        foreach (var player in players)
        {
            var shuffledNumbers = ShuffledImageId();
            cards.Add(new Card(CardId.GetId(), shuffledNumbers[0]));
            cards.Add(new Card(CardId.GetId(), shuffledNumbers[1]));
            cards.Add(new Card(CardId.GetId(), shuffledNumbers[2]));
            cards.Add(new Card(CardId.GetId(), shuffledNumbers[3]));
            cards.Add(new Card(CardId.GetId(), shuffledNumbers[4]));
            cards.Add(new Card(CardId.GetId(), shuffledNumbers[5]));
            cards.Add(new Card(CardId.GetId(), shuffledNumbers[6]));
            cards.Add(new Card(CardId.GetId(), shuffledNumbers[7]));
        }
        Cards = cards.ToArray();
    }
    public Card[] Cards { get; }

    public Player CurrentPlayer { get; set; }

    public int[] ShuffledImageId()
    {
        var numbers = Enumerable.Range(1, 8);

        Random random = new Random();

        var shuffledNumbers = numbers.OrderBy(x => random.Next()).ToArray();

        return shuffledNumbers;
    }

    public void Start()
    {
        //throw new NotImplementedException();
        CurrentPlayer = Player.Player1;
    }

    private int CardsRevealedByCurrentPlayer = 0;

    public void Show(int cardId)
    {
        var card = Cards.SingleOrDefault(c => c.CardId == cardId);
        if (card == null)
        {
            return;
        }
        else
        {
            if (CurrentPlayer != Player.None && CardsRevealedByCurrentPlayer < 2)
            {
                card.State = CardState.Visible;
                CardsRevealedByCurrentPlayer++;
            }
        }
    }

    public CheckState Check()
    {
        var VisibleCards = Cards.Where(c => c.State == CardState.Visible);

        var c1 = VisibleCards.First();
        var c2 = VisibleCards.Last();

        if (c1.ImageId == c2.ImageId)
        {
            c1.State = CardState.Removed;
            c2.State = CardState.Removed;
            return CheckState.PairFound;
        }
        else
        {
            c1.State = CardState.Hidden;
            c2.State = CardState.Hidden;
            CurrentPlayer = Player.Player2;
            return CheckState.PairNotFound;
        }
    }
}