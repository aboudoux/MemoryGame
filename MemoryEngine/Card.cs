namespace MemoryEngine;

public class Card
{
    public enum CardState
    {
        Visible = 1,
        Hidden = 2,
    }

    public int CardId { get; } = 0;
    public CardState State { get; } = CardState.Hidden;

    public int ImageId { get; } = 0;

    public Card(int cardId, int imageId) {

    }
}