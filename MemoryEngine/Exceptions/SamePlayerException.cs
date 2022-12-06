namespace MemoryEngine.Exceptions;

public class SamePlayerException : Exception
{
    public SamePlayerException() : base("Je jeux possède 2 fois le même joueur")
    {
    }
}