using MemoryEngine.Exceptions;

namespace MemoryEngine;

public class GridGame
{
    

    public GridGame(params Player[] players)
    {
        if (players.Length < 2)
            throw new Exception();

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

    public Player Turn { get; }

    public void Start()
    {
        throw new NotImplementedException();
    }

    public void Show(int cardId)
    {

    }
}