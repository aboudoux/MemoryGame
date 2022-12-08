using FluentAssertions;
using MemoryEngine;

namespace Memory.Tests;

public class GridGamePlayTests
{
    [Fact(DisplayName = "Le joueur 1 ne trouve pas de paire")]
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

    [Fact(DisplayName = "Le joueur 1 trouve une paire")]
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
}