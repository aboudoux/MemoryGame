using FluentAssertions;
using MemoryEngine;

namespace Memory.Tests {
    public class GridGameTests 
    {
        [Fact(DisplayName = "Une grille ne peux pas avoir 2 fois le même joueur")]
        public void Test16() 
        {
            FluentActions.Invoking(() => new GridGame(Player.Player1, Player.Player1)).Should().Throw<Exception>();
        }

        [Fact(DisplayName = "Une grille doit au moint avoir 2 joueurs minimums")]
        public void Test14()
        {
            FluentActions.Invoking(() => new GridGame(Player.Player1)).Should().Throw<Exception>();
        }

        [Fact(DisplayName = "La grille me retourne un nombre pair de cartes en fonction du nombre de joueur")]
        public void Test1()
        {
            // on va créer 8 cartes par joueur
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Cards.Length.Should().Be(16);
        }

        [Fact(DisplayName = "Tous les id de cartes doivents être unique")]
        public void Test28()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Cards.Select(a => a.CardId).Distinct().Should().HaveCount(16);
        }

        [Fact(DisplayName = "chaque carte doit avoir une image id associée à une autre carte")]
        public void Test35()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Cards.GroupBy(a => a.ImageId).Should().HaveCount(8);
        }
    }
}