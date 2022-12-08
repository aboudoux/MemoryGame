using FluentAssertions;
using MemoryEngine;

namespace Memory.Tests;

public class GridGamePlayTests
{
    [Fact(DisplayName = "Le joueur ne trouve pas de paire")]
    public void Test138() 
    {
        GridGame grid = new GridGame(Player.Player1, Player.Player2);
        grid.Start();

        var c1 = grid.Cards.First(a => a.ImageId == 1);
        var c2 = grid.Cards.First(a => a.ImageId == 2);

        grid.Show(c1.CardId);
        grid.Show(c2.CardId);

        grid.Check().Should().Be(CheckState.PairNotFound);
        grid.CurrentPlayer.Should().Be(Player.Player2);
        grid.Cards.All(a=>a.State == CardState.Hidden).Should().BeTrue();
    }

    [Fact(DisplayName = "chaque joueur qui perd passe son tour au joueur suivant")]
    public void Test26()
    {
        GridGame grid = new GridGame(Player.Player1, Player.Player2, Player.Player3);
        grid.Start();
        var c1 = grid.Cards.First(a => a.ImageId == 1);
        var c2 = grid.Cards.First(a => a.ImageId == 2);

        PlayAndEnsureNextPlayerIs(Player.Player2);
        PlayAndEnsureNextPlayerIs(Player.Player3);
        PlayAndEnsureNextPlayerIs(Player.Player1);
        PlayAndEnsureNextPlayerIs(Player.Player2);
        PlayAndEnsureNextPlayerIs(Player.Player3);
        PlayAndEnsureNextPlayerIs(Player.Player1);

        void PlayAndEnsureNextPlayerIs(Player player)
        {
            grid.Show(c1.CardId);
            grid.Show(c2.CardId);

            grid.Check().Should().Be(CheckState.PairNotFound);
            grid.CurrentPlayer.Should().Be(player);
            grid.Cards.All(a => a.State == CardState.Hidden).Should().BeTrue();
        }
    }

    [Fact(DisplayName = "Le joueur trouve une paire")]
    public void Test139() 
    {
        GridGame grid = new GridGame(Player.Player1, Player.Player2);
        grid.Start();

        var c1 = grid.Cards.First(a => a.ImageId == 1);
        var c2 = grid.Cards.First(a => a.ImageId == 1 && a.CardId != c1.CardId);

        grid.Show(c1.CardId);
        grid.Show(c2.CardId);

        grid.Check().Should().Be(CheckState.PairFound);
        grid.CurrentPlayer.Should().Be(Player.Player1);

        grid.Cards.Any(a => a.State == CardState.Visible).Should().BeFalse();
        var removedCards = grid.Cards.Where(a => a.State == CardState.Removed).ToArray();
        removedCards.Should().HaveCount(2);
        removedCards.Select(a => a.CardId).Should().BeEquivalentTo(new[]{c1.CardId, c2.CardId});
    }

    [Fact(DisplayName = "Si le joueur trouve une pair, il gagne 1 point")]
    public void Test47() 
    {
        GridGame grid = new GridGame(Player.Player1, Player.Player2);
        grid.Start();

        var c1 = grid.Cards.First(a => a.ImageId == 1);
        var c2 = grid.Cards.First(a => a.ImageId == 1 && a.CardId != c1.CardId);

        grid.Show(c1.CardId);
        grid.Show(c2.CardId);

        grid.Check().Should().Be(CheckState.PairFound);
        grid.CurrentPlayer.Should().Be(Player.Player1);

        grid.Score[Player.Player1].Should().Be(1);
        grid.Score[Player.Player2].Should().Be(0);
    }

    [Fact(DisplayName = "Lorsqu'il n'y a plus de cartes visible, le jeu s'arrete.")]
    public void Test92()
    {
        GridGame grid = new GridGame(Player.Player1, Player.Player2);
        grid.Start();
        for (int i = 1; i <= 8; i++)
        {
            var c1 = grid.Cards.First(a => a.ImageId == i);
            var c2 = grid.Cards.First(a => a.ImageId == i && a.CardId != c1.CardId);
            grid.Show(c1.CardId);
            grid.Show(c2.CardId);
            grid.Check().Should().Be(CheckState.PairFound);

            c1.State.Should().Be(CardState.Removed);
            c2.State.Should().Be(CardState.Removed);
        }

        grid.Score[Player.Player1].Should().Be(8);
        grid.Cards.All(a=>a.State == CardState.Removed).Should().BeTrue();
    }
}