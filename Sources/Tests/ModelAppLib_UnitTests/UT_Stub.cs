using System;
using System.Collections.Generic;
using System.Linq;
using ModelAppLib;
using StubLib;
using Xunit;

namespace ModelAppLib_UnitTests
{
    public class UT_Stub
    {
        [Fact]
        void CreateObjectNotNull()
        {
            var stub = new Stub();
            Assert.NotNull(stub);
        }

        [Fact]
        void DiceCollectionNotEmpty()
        {
            var stub = new Stub();
            Assert.NotEmpty(stub.GetAllDices().Result);
        }

        [Fact]
        void SideCollectionNotEmpty()
        {
            var stub = new Stub();
            Assert.NotEmpty(stub.GetAllSides().Result);
        }


        [Fact]
        void GameCollectionNotEmpty()
        {
            var stub = new Stub();
            Assert.NotEmpty(stub.GetAllGames().Result);
        }

        [Theory]
        [InlineData(3, 5)]
        [InlineData(1, 0)]
        [InlineData(9, 3)]
        void CheckGettingSomeSides(int nbSides, int pageNum)
        {
            var stub = new Stub();
            var result = stub.GetSomeSides(nbSides, pageNum).Result.ToList();

            Assert.Equal(nbSides, result.Count);
            Assert.Equal("img" + (int)(nbSides * pageNum), result[0].Image);
            Assert.Equal("img" + (int)((nbSides * pageNum) + nbSides - 1), result[result.Count - 1].Image);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(20)]
        [InlineData(100)]
        void CheckGettingSomeGames(int nbGames)
        {
            var stub = new Stub();
            var result = stub.GetSomeGames(nbGames, 1).Result;

            Assert.Equal(nbGames, result.Count());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        [InlineData(4)]
        [InlineData(20)]
        [InlineData(100)]
        void CheckGettingSomeDices(int nbDices)
        {
            var stub = new Stub();
            var result = stub.GetSomeDices(nbDices, 1).Result;

            Assert.Equal(nbDices, result.Count());
        }

        [Fact]
        void CheckAddingDice()
        {
            var stub = new Stub();
            Assert.True(stub.AddDice(new Dice(new SecureRandomizer(), new DiceSideType(1, new DiceSide("img1")))).Result);
        }

        [Fact]
        void CheckAddingSide()
        {
            var stub = new Stub();
            Assert.True(stub.AddSide(new DiceSide("img1")).Result);
        }

        [Fact]
        void CheckAddingGame()
        {
            var stub = new Stub();
            Assert.True(stub.AddGame(new Game(new List<DiceType>())).Result);
        }

        [Fact]
        void CheckGettingNbDices()
        {
            var stub = new Stub();
            Assert.Equal(stub.GetAllDices().Result.Count(), stub.GetNbDice().Result);
        }
        
        [Fact]
        void CheckGettingNbSides()
        {
            var stub = new Stub();
            Assert.Equal(stub.GetAllSides().Result.Count(), stub.GetNbSide().Result);
        }
        
        [Fact]
        void CheckGettingNbGames()
        {
            var stub = new Stub();
            Assert.Equal(stub.GetAllGames().Result.Count(), stub.GetNbGame().Result);
        }
    }
}
