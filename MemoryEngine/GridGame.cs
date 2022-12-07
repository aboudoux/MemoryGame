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
            cards.Add(new Card(CardId.GetId(), ImageId.GetImageId()));
            cards.Add(new Card(CardId.GetId(), ImageId.GetImageId()));
            cards.Add(new Card(CardId.GetId(), ImageId.GetImageId()));
            cards.Add(new Card(CardId.GetId(), ImageId.GetImageId()));
            cards.Add(new Card(CardId.GetId(), ImageId.GetImageId()));
            cards.Add(new Card(CardId.GetId(), ImageId.GetImageId()));
            cards.Add(new Card(CardId.GetId(), ImageId.GetImageId()));
            cards.Add(new Card(CardId.GetId(), ImageId.GetImageId()));
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