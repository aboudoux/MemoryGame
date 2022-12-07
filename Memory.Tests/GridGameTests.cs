using FluentAssertions;
using MemoryEngine;
using MemoryEngine.Exceptions;

namespace Memory.Tests {
    public class GridGameTests 
    {
        [Fact(DisplayName = "Une grille ne peux pas avoir 2 fois le m�me joueur")]
        public void Test16() 
        {
            FluentActions.Invoking(() => new GridGame(Player.Player1, Player.Player1)).Should().Throw<SamePlayerException>();
        }

        [Fact(DisplayName = "Une grille doit au moins avoir 2 joueurs minimum")]
        public void Test14()
        {
            FluentActions.Invoking(() => new GridGame(Player.Player1)).Should().Throw<NotEnoughPlayerException>();
        }

        private class Test1Class : TheoryData<(Player[] players, int expectedCards)>
        {
            public Test1Class()
            {
                Add((new []{ Player.Player1, Player.Player2}, 16) );
                Add((new []{ Player.Player1, Player.Player2, Player.Player3 }, 24) );
            }
        }

        [Theory(DisplayName = "La grille me retourne un nombre pair de cartes en fonction du nombre de joueur")]
        [ClassData(typeof(Test1Class))]
        public void Test1((Player[] players, int expectedCards) arg)
        {
            GridGame grid = new GridGame(arg.players);
            grid.Cards.Length.Should().Be(arg.expectedCards);
        }

        [Fact(DisplayName = "Tous les id de cartes doivents �tre unique")]
        public void Test28()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Cards.Select(a => a.CardId).Distinct().Should().HaveCount(16);
        }

        [Fact(DisplayName = "Chaque carte doit avoir une image id associ�e � une autre carte")]
        public void Test35()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Cards.GroupBy(a => a.ImageId).Should().HaveCount(8);
        }


        [Fact(DisplayName = "Toutes les cartes sont cach� au d�marrage du jeu")]
        public void Test52()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Cards.All(a => a.State.Equals(Card.CardState.Hidden)).Should().BeTrue();
        }


        [Fact(DisplayName = "Tant que le jeu n'a pas commenc�, c'est le tour de personne")]
        public void Test60()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.CurrentPlayer.Should().Be(Player.None);
        }

        [Fact(DisplayName = "Lorsque je d�marre le jeu, c'est au tour du joueur 1")]
        public void Test67()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Start();
            grid.CurrentPlayer.Should().Be(Player.Player1);
        }

        [Fact(DisplayName = "Le jeu ne revele aucune carte si j'essaye de r�v�ler une carte alors que c'est le tour de personne")]
        public void Test75()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.CurrentPlayer = Player.None;
            grid.Show(grid.GetCardId(0));
            grid.Cards.All(a => a.State == Card.CardState.Hidden).Should().BeTrue();
        }

        [Fact(DisplayName = "Un joueur peu demander � reveler une carte quand c'est sont tour")]
        public void Test51()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Start();
            grid.Show(grid.GetCardId(0));
            grid.GetCard(grid.GetCardId(0)).State.Should().Be(Card.CardState.Visible);
        }

        [Fact(DisplayName = "Un joueur ne peux pas r�veler + de 2 cartes")]
        public void Test91()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Start();
            grid.Show(grid.GetCardId(0));
            grid.Show(grid.GetCardId(1));
            grid.Show(grid.GetCardId(2));

            grid.GetCard(grid.GetCardId(0)).State.Should().Be(Card.CardState.Visible);
            grid.GetCard(grid.GetCardId(1)).State.Should().Be(Card.CardState.Visible);
            grid.GetCard(grid.GetCardId(2)).State.Should().Be(Card.CardState.Hidden);
        }

        [Fact(DisplayName = "Une fois qu'un joueur a r�v�l� une carte, il ne peut pas la cacher")]
        public void Test105()
        {
            GridGame grid = new GridGame(Player.Player1, Player.Player2);
            grid.Start();
            grid.Show(grid.GetCardId(0));
            grid.Show(grid.GetCardId(0));
            grid.GetCard(grid.GetCardId(0)).State.Should().Be(Card.CardState.Visible);
        }

        /// <summary>
        /// Les ImagesId doivent �tre distribu�s de mani�re al�atoires dans les cartes � chaque partie.
        /// Attention, chaque Id doit etre compris entre 1 et n * 8 (n �tant le nombre de joueur)
        /// </summary>
        [Fact(DisplayName = "Distribution al�atoire des images")]
        public void Test118()
        {
            var imagesIds = Array.Empty<int>();

            for (int i = 0; i < 10; i++)
            {
                GridGame grid = new GridGame(Player.Player1, Player.Player2);
                var allImages = grid.Cards.Select(a => a.ImageId).ToArray();
                allImages.Should().NotBeEquivalentTo(imagesIds, a=>a.WithStrictOrdering());
                imagesIds = allImages;

                allImages.All(a => a is >= 1 and <= (2 * 8)).Should().BeTrue();
            }
        }

    }

    public static class GridGameExtensions
    {
        public static Card GetCard(this GridGame game, int cardId) => game.Cards.First(a => a.CardId == cardId);

        public static int GetCardId(this GridGame game, int cardPosition) => game.Cards[cardPosition].CardId;
    }
}