namespace MemoryEngine.Exceptions;

public class NotEnoughPlayerException : Exception
{
    public NotEnoughPlayerException() : base("Le jeu doit au moins avoir 2 joueurs")
    {
    }
}