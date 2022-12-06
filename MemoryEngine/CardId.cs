namespace MemoryEngine;

public record CardId(int Value)
{
    private static int CurrentId = 0;

    public static int GetId()
    {
        return CurrentId++;
    }
}