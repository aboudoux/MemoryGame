﻿using MemoryEngine.Exceptions;

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

            Score[player] = 0;
        }
        Cards = cards.ToArray();
    }
    public Card[] Cards { get; }

    public Player CurrentPlayer { get; set; } = Player.None;

    public bool IsGameStarted { get; private set; } = false;

    public void Start()
    {
        CurrentPlayer = Player.Player1;
        IsGameStarted = true;
        Status.State = GameState.Playing;
    }

    private List<Card> CurrentPlayerCards = new List<Card>();
    private int CardsRevealedByCurrentPlayer = 0;

    public int Show(int cardId)
    {
        var card = Cards.SingleOrDefault(c => c.CardId == cardId);
        if (card == null)
        {
            return CurrentPlayerCards.Count;
        }

        if (card.State == CardState.Visible)
        {
            return CurrentPlayerCards.Count;
        }

        else
        {
            if (CurrentPlayer != Player.None && CardsRevealedByCurrentPlayer < 2)
            {
                card.State = CardState.Visible;
                CurrentPlayerCards.Add(card);
                CardsRevealedByCurrentPlayer++;
            }
        }

        return CurrentPlayerCards.Count;
    }

    public GameStatus Status = new();


    public CheckState Check()
    {
        if (!IsGameStarted)
        {
            return CheckState.NotStarted;
        }

        if (Cards.All(c => c.State == CardState.Removed) && Status.State != GameState.Playing)
        {
            return CheckState.GameOver;
        }
            
        if (CurrentPlayerCards.Count() != 2)
        {
            if (Cards.All(c => c.State == CardState.Removed))
            {
                CurrentPlayer = Player.None;
                if (Score[Player.Player1] == Score[Player.Player2])
                {
                    Status.State = GameState.Execo;
                    Status.Winner = Player.None;
                }
                else if (Score[Player.Player1] > Score[Player.Player2])
                {
                    Status.State = GameState.PlayerWin;
                    Status.Winner = Player.Player1;
                }
                else
                {
                    Status.State = GameState.PlayerWin;
                    Status.Winner = Player.Player2;
                }

                return CheckState.GameOver;
            }

            return CheckState.CantCheck;
        }

        var c1 = CurrentPlayerCards[0];
        var c2 = CurrentPlayerCards[1];

        if (c1.ImageId == c2.ImageId)
        {
            c1.State = CardState.Removed;
            c2.State = CardState.Removed;
            Score[CurrentPlayer]++;
            ClearRevelatedCards();

            return CheckState.PairFound;
        }
        else
        {
            if (CurrentPlayer == Player.Player1)
            {
                c1.State = CardState.Hidden;
                c2.State = CardState.Hidden;
                CurrentPlayer = Player.Player2;
                ClearRevelatedCards();
                return CheckState.PairNotFound;
            }
            else if (CurrentPlayer == Player.Player2)
            {
                c1.State = CardState.Hidden;
                c2.State = CardState.Hidden;
                CurrentPlayer = Player.Player1;
                ClearRevelatedCards();
                return CheckState.PairNotFound;
            }
            else
            {
                c1.State = CardState.Hidden;
                c2.State = CardState.Hidden;
                CurrentPlayer = Player.Player1;
                ClearRevelatedCards();
                return CheckState.PairNotFound;
            }
        }
    }

    private void ClearRevelatedCards()
    {
        CurrentPlayerCards.Clear();
        CardsRevealedByCurrentPlayer = 0;
    }

    public Dictionary<Player, int> Score = new();

    private int[] ShuffledImageId() {
        var numbers = Enumerable.Range(1, 8);

        Random random = new Random();

        var shuffledNumbers = numbers.OrderBy(x => random.Next()).ToArray();

        return shuffledNumbers;
    }
}

public class GameStatus
{
    public Player Winner { get; set; }
    public GameState State { get; set; } = GameState.NotStarted;
}

public enum GameState
{
    NotStarted,
    Playing,
    PlayerWin,
    Execo,
}