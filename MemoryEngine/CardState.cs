namespace MemoryEngine;

public record CardState(int Value)
{
    public static CardState Hidden => new(0);
    public static CardState Visible => new(1);
}