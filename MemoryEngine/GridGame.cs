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

    public void NextPlayer()
    {
        CurrentPlayer = (Player)(((int)CurrentPlayer + 1) % 3);
        CardsRevealedByCurrentPlayer = 0;
    }

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
                card.State = Card.CardState.Visible;
                CardsRevealedByCurrentPlayer++;
            }
        }
    }
}