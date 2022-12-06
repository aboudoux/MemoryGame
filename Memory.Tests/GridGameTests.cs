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

        [Fact(DisplayName = "Une grille doit au moins avoir 2 joueurs minimum")]
        public void Test14()
        {
            FluentActions.Invoking(() => new GridGame(Player.Player1)).Should().Throw<Exception>();
        }

        private class Test1Class : TheoryData<(Player[] players, int expectedCards)>
        {
            public Test1Class()
            {
                Add((new []{ Player.Player1, Player.Player2}, 16) );
                Add((new []{ Player.Player1, Player.Player2, new Player(3) }, 24) );
            }
        }

        [Theory(DisplayName = "La grille me retourne un nombre pair de cartes en fonction du nombre de joueur")]
        [ClassData(typeof(Test1Class))]
        public void Test1((Player[] players, int expectedCards) arg)
        {
            GridGame grid = new GridGame(arg.players);
            grid.Cards.Length.Should().Be(arg.expectedCards);
        }

        [Fact(DisplayName = "Tous les id de cartes doivents être unique")]
        public void Test28()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Cards.Select(a => a.CardId).Distinct().Should().HaveCount(16);
        }

        [Fact(DisplayName = "Chaque carte doit avoir une image id associée à une autre carte")]
        public void Test35()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Cards.GroupBy(a => a.ImageId).Should().HaveCount(8);
        }


        [Fact(DisplayName = "Toutes les cartes sont caché au démarrage du jeu")]
        public void Test52()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Cards.All(a => a.State.Equals(CardState.Hidden)).Should().BeTrue();
        }


        [Fact(DisplayName = "Tant que le jeu n'a pas commencé, c'est le tour de personne")]
        public void Test60()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Turn.Should().Be(Player.None);
        }

        [Fact(DisplayName = "Lorsque je démarre le jeu, c'est au tour du joueur 1")]
        public void Test67()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Start();
            grid.Turn.Should().Be(Player.Player1);
        }

        [Fact(DisplayName = "Le jeu retourne une erreur si j'essaye de révéler une carte alors que c'est le tour de personne")]
        public void Test75()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            FluentActions.Invoking(() => grid.Cards[0].Show()).Should().Throw<Exception>();
        }

        [Fact(DisplayName = "Un joueur peu demander à reveler une carte quand c'est sont tour")]
        public void Test51()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Start();
            grid.Cards[0].Show();
            grid.Cards[0].State.Should().Be(CardState.Visible);
        }

        [Fact(DisplayName = "Un joueur ne peux pas réveler + de 2 cartes")]
        public void Test91()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Start();
            grid.Cards[0].Show();
            grid.Cards[1].Show();
            grid.Cards[2].Show();

            grid.Cards[0].State.Should().Be(CardState.Visible);
            grid.Cards[1].State.Should().Be(CardState.Visible);
            grid.Cards[2].State.Should().Be(CardState.Hidden);
        }

        [Fact(DisplayName = "Une fois qu'un joueur a révélé une carte, il ne peut pas la cacher")]
        public void Test105()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Start();
            grid.Cards[0].Show();
            grid.Cards[0].Show();
            grid.Cards[0].State.Should().Be(CardState.Visible);
        }
    }
}