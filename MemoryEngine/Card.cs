namespace MemoryEngine;

public class Card
{
    public CardId CardId { get; }
    public CardState State { get; }

    public Card(int id, int iid) {
        CardId = new CardId(id);
        ImageId= new ImageId(iid);
    }

    public ImageId ImageId { get; }
}