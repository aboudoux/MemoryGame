﻿namespace MemoryEngine;

public record Player(int Value)
{
    public static Player Player1 => new(1);
    public static Player Player2 => new(2);
}