using EntitiesLib;
using ModelAppLib;
using StubEntitiesLib;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Entities_UnitTests
{
    public class UT_DataBaseLinker
    {

        // =============================================== //
        //      TESTS
        // =============================================== //

        // ---------- ADD ---------- //

        [Fact]
        async void TestAddingSides()
        {
            var linker = GetLinkerInMemory();
            var NbSides = linker.GetNbSide().Result;

            DiceSide ds1 = new("TEST.png");
            DiceSide ds2 = new("TEST2.png");

            await linker.AddSide(ds1);
            await linker.AddSide(ds2);

            Assert.Equal(NbSides + 2, linker.GetNbSide().Result);

            var sides = linker.GetAllSides().Result;
            Assert.Equal("TEST2.png", sides.Last().Image);
        }

        [Fact]
        async void TestAddingDice()
        {
            var linker = GetLinkerInMemory();
            var sides = linker.GetAllSides().Result;

            await linker.AddDice(new Dice(new SecureRandomizer(), new DiceSideType(2, sides.First())));

            var dices = linker.GetSomeDices(1,0).Result;

            Assert.Single(dices);
            Assert.Equal(1, linker.GetNbDice().Result);
            Assert.Equal(sides.First(), dices.First().SideTypes[0].Prototype);

        }

        [Fact]
        async void TestAddingGame()
        {
            var linker = GetLinkerInMemory();
            var sides = linker.GetAllSides().Result;

            var dice = new Dice(new SecureRandomizer(), new DiceSideType(2, sides.First()));

            await linker.AddDice(dice);

            await linker.AddGame(new Game(new DiceType(2, dice)));

            Assert.Single(linker.GetAllDices().Result);
            Assert.Equal(1, linker.GetNbDice().Result);

            var games = linker.GetSomeGames(1,0).Result;

            Assert.Single(games);
            Assert.Equal(1, linker.GetNbGame().Result);

            Assert.Equal(dice, games.First().Dices[0].Prototype);
        }

        [Fact]
        async void TestAddingGameWithUnknownDice()
        {
            var linker = GetLinkerInMemory();
            var sides = linker.GetSomeSides(1,0).Result;

            var dice = new Dice(new SecureRandomizer(), new DiceSideType(2, sides.First()));

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await linker.AddGame(new Game(new DiceType(2, dice)));
            });
        }

        [Fact]
        async void TestAddingDiceWithUnknownSide()
        {
            var linker = GetLinkerInMemory();

            var side = new DiceSide("TEST.png");
            var dice = new Dice(new SecureRandomizer(), new DiceSideType(2, side));

            await Assert.ThrowsAsync<InvalidOperationException>(async () =>
            {
                await linker.AddDice(dice);
            });
        }

        // ---------- REMOVE ---------- //

        [Fact]
        async void TestRemovingSide()
        {
            var linker = GetLinkerInMemory();
            var sides = linker.GetAllSides().Result.ToList();

            await linker.DeleteSide(sides[0]);
            var newSides = linker.GetAllSides().Result;

            Assert.Equal(sides.Count - 1, newSides.Count());
            Assert.Equal(sides.Count - 1, linker.GetNbSide().Result);
            Assert.Equal(sides[1], newSides.First());
        }

        [Fact]
        async void TestRemovingDice()
        {
            var linker = GetLinkerInMemory();
            var sides = linker.GetAllSides().Result;

            var dice = new Dice(new SecureRandomizer(), new DiceSideType(1, sides.First()));
            await linker.AddDice(dice);
            await linker.DeleteDice(dice);

            // Dice should disepear
            Assert.Empty(linker.GetAllDices().Result);
            Assert.Equal(0, linker.GetNbDice().Result);
            // Side should still exist
            Assert.Equal(sides, linker.GetAllSides().Result);
            Assert.Equal(sides.Count(), linker.GetNbSide().Result);
        }

        [Fact]
        async void TestRemovingGame()
        {
            var linker = GetLinkerInMemory();
            var sides = linker.GetAllSides().Result;

            var dice = new Dice(new SecureRandomizer(), new DiceSideType(1, sides.First()));
            await linker.AddDice(dice);

            var game = new Game(new DiceType(1, dice));
            await linker.AddGame(game);

            await linker.DeleteGame(game);

            // Game should disapear
            Assert.Empty(linker.GetAllGames().Result);
            Assert.Equal(0, linker.GetNbGame().Result);

            // Dice should still exist
            Assert.Single(linker.GetAllDices().Result);
            Assert.Equal(1, linker.GetNbDice().Result);
        }

        [Fact]
        async void TestRemovingUnknownSide()
        {
            var linker = GetLinkerInMemory();
            var side = new DiceSide("TEST.png");
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await linker.DeleteSide(side);
            });
        }

        [Fact]
        async void TestRemovingUnknownDice()
        {
            var linker = GetLinkerInMemory();
            var sides = linker.GetAllSides().Result;
            var dice = new Dice(new SecureRandomizer(), new DiceSideType(1, sides.First()));
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await linker.DeleteDice(dice);
            });
        }

        [Fact]
        async void TestRemovingUnknownGame()
        {
            var linker = GetLinkerInMemory();
            var sides = linker.GetAllSides().Result;
            var dice = new Dice(new SecureRandomizer(), new DiceSideType(1, sides.First()));
            var game = new Game(new DiceType(1, dice));
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await linker.DeleteGame(game);
            });
        }

        // ---------- ADD TO ---------- //

        [Theory]
        [MemberData(nameof(DataAddingSidesToDice))]
        async void TestAddingSidesToDice(Dice d, IEnumerable<DiceSide> sides, DiceSide ToAdd, int nbToAdd)
        {
            var linker = GetLinkerInMemory();

            foreach(var side in sides)
                await linker.AddSide(side);
            if(!sides.Contains(ToAdd))
                await linker.AddSide(ToAdd);

            await linker.AddDice(d);
            var oldCount = d.SideTypes.Count;
            var oldTotalSides = d.GetTotalSides();
            await linker.AddSideToDice(d, ToAdd, nbToAdd);

            var dice = linker.GetAllDices().Result.First();

            if (sides.Contains(ToAdd))
                Assert.Equal(oldCount, dice.SideTypes.Count);
            else
                Assert.Equal(oldCount + 1, dice.SideTypes.Count);

            Assert.Equal(oldTotalSides+nbToAdd, dice.GetTotalSides());
        }

        [Theory]
        [MemberData(nameof(DataAddingDicesToGame))]
        async void TestAddingDicesToGamee(Game g,IEnumerable<DiceSide> sides, IEnumerable<Dice> dices, Dice ToAdd, int nbToAdd)
        {
            var linker = GetLinkerInMemory();
            foreach (var side in sides)
                await linker.AddSide(side);
            foreach (var dice in dices)
                await linker.AddDice(dice);
            if (!dices.Contains(ToAdd))
                await linker.AddDice(ToAdd);

            await linker.AddGame(g);
            var oldCount = g.Dices.Count;
            await linker.AddDiceToGame(g, ToAdd, nbToAdd);

            var game = linker.GetAllGames().Result.First();

            if (dices.Contains(ToAdd))
                Assert.Equal(oldCount, game.Dices.Count);
            else
                Assert.Equal(oldCount + 1, game.Dices.Count);
        }

        [Fact]
        async void TestAddingNullAndNegativeNbOfSidesToDice()
        {
            var linker = GetLinkerInMemory();
            var sides = linker.GetAllSides().Result;

            Dice d = new Dice(new SecureRandomizer(), new DiceSideType(2, sides.First()));

            await linker.AddDice(d);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                await linker.AddSideToDice(d, sides.Last(), 0);

            });
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                await linker.AddSideToDice(d, sides.Last(), -1);

            });
        }
        
        [Fact]
        async void TestAddingNullAndNegativeNbOfDicesToGame()
        {
            var linker = GetLinkerInMemory();
            var sides = linker.GetAllSides().Result;

            Dice d = new Dice(new SecureRandomizer(), new DiceSideType(2, sides.First()));
            Game g = new Game(new DiceType(2, d));

            await linker.AddDice(d);
            await linker.AddGame(g);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                await linker.AddDiceToGame(g, d, 0);

            });
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                await linker.AddDiceToGame(g, d, -1);

            });
        }

        // ---------- REMOVE FROM ---------- //

        [Theory]
        [MemberData(nameof(DataRemovingSidesFromDice))]
        async void TestRemovingSidesFromDice(Dice d, IEnumerable<DiceSide> sides, DiceSide ToRemove, int nbOfRemoved, int nbToRemove)
        {
            var linker = GetLinkerInMemory();
            foreach (var side in sides)
                await linker.AddSide(side);
            await linker.AddDice(d);
            var oldCount = d.SideTypes.Count;
            var oldNum = d.SideTypes.First(st => st.Prototype.Equals(ToRemove)).NbSide;
            await linker.RemoveSideFromDice(d, ToRemove, nbToRemove);
            var dice = linker.GetAllDices().Result.First();
            if(nbOfRemoved > nbToRemove)
            { // il reste des faces de ce type
                Assert.Equal(oldCount, dice.SideTypes.Count);
                var newNum = dice.SideTypes.First(st => st.Prototype.Equals(ToRemove)).NbSide;
                Assert.Equal(oldNum - nbToRemove, newNum);
            }
            else
            { // il ne reste plus de face de ce type
                Assert.Equal(oldCount - 1, dice.SideTypes.Count);
                Assert.Throws<InvalidOperationException>(() =>
                {
                    dice.SideTypes.First(st => st.Prototype.Equals(ToRemove));
                });
            }

        }

        [Theory]
        [MemberData(nameof(DataRemovingDicesFromGame))]
        async void TestRemovingDicesFromGame(Game g, IEnumerable<DiceSide> sides, IEnumerable<Dice> dices, Dice ToRemove, int nbOfRemoved, int nbToRemove)
        {
            var linker = GetLinkerInMemory();
            foreach (var side in sides)
                await linker.AddSide(side);
            foreach (var dice in dices)
                await linker.AddDice(dice);
            await linker.AddGame(g);
            var oldCount = g.Dices.Count;
            var oldNum = g.Dices.First(st => st.Prototype.Equals(ToRemove)).NbDices;
            await linker.RemoveDiceFromGame(g, ToRemove, nbToRemove);
            var game = linker.GetAllGames().Result.First();
            if (nbOfRemoved > nbToRemove)
            { // il reste des dés de ce type
                Assert.Equal(oldCount, game.Dices.Count);
                var newNum = game.Dices.First(st => st.Prototype.Equals(ToRemove)).NbDices;
                Assert.Equal(oldNum - nbToRemove, newNum);
            }
            else
            { // il ne reste plus de dé de ce type
                Assert.Equal(oldCount - 1, game.Dices.Count);
                Assert.Throws<InvalidOperationException>(() =>
                {
                    game.Dices.First(st => st.Prototype.Equals(ToRemove));
                });
            }

        }

        [Fact]
        async void TestRemovingNullAndNegativeNbOfSidesFromDice()
        {
            var linker = GetLinkerInMemory();
            var sides = linker.GetAllSides().Result;

            Dice d = new Dice(new SecureRandomizer(), new DiceSideType(2, sides.First()));

            await linker.AddDice(d);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                await linker.RemoveSideFromDice(d, sides.Last(), 0);

            });
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                await linker.RemoveSideFromDice(d, sides.Last(),-1);

            });
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await linker.RemoveSideFromDice(null, sides.Last());

            });
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await linker.RemoveSideFromDice(d, null);

            });
        }

        [Fact]
        async void TestRemovingNullAndNegativeNbOfDicesFromGame()
        {
            var linker = GetLinkerInMemory();
            var sides = linker.GetAllSides().Result;

            Dice d = new Dice(new SecureRandomizer(), new DiceSideType(2, sides.First()));
            Game g = new Game(new DiceType(2, d));

            await linker.AddDice(d);
            await linker.AddGame(g);

            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                await linker.RemoveDiceFromGame(g, d, 0);

            });
            await Assert.ThrowsAsync<ArgumentOutOfRangeException>(async () =>
            {
                await linker.RemoveDiceFromGame(g, d, -1);

            });            
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await linker.RemoveDiceFromGame(null, d);

            });            
            await Assert.ThrowsAsync<ArgumentException>(async () =>
            {
                await linker.RemoveDiceFromGame(g, null);

            });
        }


        // =============================================== //
        //      MEMBER DATAS
        // =============================================== //

        public static IEnumerable<object[]> DataAddingSidesToDice()
        {
            {
                List<DiceSide> sides = new List<DiceSide>
                {
                    new DiceSide("TEST1"),
                    new DiceSide("TEST2")
                };
                Dice d = new Dice(new SecureRandomizer(), new DiceSideType(1, sides[0]), new DiceSideType(2, sides[1]));

                yield return new object[]
                {
                    d,
                    sides,
                    sides[1],
                    5
                };
            }
            {
                List<DiceSide> sides = new List<DiceSide>
                {
                    new DiceSide("TEST1"),
                    new DiceSide("TEST2")
                };
                Dice d = new Dice(new SecureRandomizer(), new DiceSideType(1, sides[0]), new DiceSideType(2, sides[1]));
                yield return new object[]
                {
                    d,
                    sides,
                    sides[1],
                    1
                };
            }
            {
                List<DiceSide> sides = new List<DiceSide>
                {
                    new DiceSide("TEST1"),
                    new DiceSide("TEST2")
                };
                Dice d = new Dice(new SecureRandomizer(), new DiceSideType(1, sides[0]), new DiceSideType(2, sides[1]));
                yield return new object[]
                {
                    d,
                    sides,
                    new DiceSide("TEST3"),
                    5
                };
            }
            {
                List<DiceSide> sides = new List<DiceSide>
                {
                    new DiceSide("TEST1")
                };
                Dice d = new Dice(new SecureRandomizer(), new DiceSideType(1, sides[0]));
                yield return new object[]
                {
                    d,
                    sides,
                    new DiceSide("TEST3"),
                    5
                };
            }
        }

        public static IEnumerable<object[]> DataRemovingSidesFromDice()
        {
            {
                List<DiceSide> sides = new List<DiceSide>
                {
                    new DiceSide("TEST1"),
                    new DiceSide("TEST2")
                };
                Dice d = new Dice(new SecureRandomizer(), new DiceSideType(1, sides[0]), new DiceSideType(6, sides[1]));

                yield return new object[]
                {
                    d,
                    sides,
                    sides[1],
                    6,
                    5
                };
            }
            {
                List<DiceSide> sides = new List<DiceSide>
                {
                    new DiceSide("TEST1"),
                    new DiceSide("TEST2")
                };
                Dice d = new Dice(new SecureRandomizer(), new DiceSideType(1, sides[0]), new DiceSideType(2, sides[1]));
                yield return new object[]
                {
                    d,
                    sides,
                    sides[1],
                    2,
                    2
                };
            }
            {
                List<DiceSide> sides = new List<DiceSide>
                {
                    new DiceSide("TEST1"),
                    new DiceSide("TEST2")
                };
                Dice d = new Dice(new SecureRandomizer(),new DiceSideType(1, sides[0]), new DiceSideType(2, sides[1]));
                yield return new object[]
                {
                    d,
                    sides,
                    sides[0],
                    1,
                    5
                };
            }
        }

        public static IEnumerable<object[]> DataAddingDicesToGame()
        {
            {
                List<DiceSide> sides = new List<DiceSide>
                {
                    new DiceSide("TEST1"),
                    new DiceSide("TEST2")
                };
                List<Dice> dices = new List<Dice>
                {
                    new Dice(new SecureRandomizer(),new DiceSideType(1, sides[0]), new DiceSideType(2, sides[1]))
                };
                Game g = new Game(new DiceType(1, dices[0]));

                yield return new object[]
                {
                    g,
                    sides,
                    dices,
                    dices[0],
                    5
                };
            }
            {
                List<DiceSide> sides = new List<DiceSide>
                {
                    new DiceSide("TEST1"),
                    new DiceSide("TEST2")
                };
                List<Dice> dices = new List<Dice>
                {
                    new Dice(new SecureRandomizer(),new DiceSideType(1, sides[0]), new DiceSideType(2, sides[1])),
                    new Dice(new SecureRandomizer(),new DiceSideType(2, sides[0]), new DiceSideType(5, sides[1]))
                };
                Game g = new Game(new DiceType(1, dices[0]), new DiceType(2, dices[1]));

                yield return new object[]
                {
                    g,
                    sides,
                    dices,
                    dices[1],
                    5
                };
            }
            {
                List<DiceSide> sides = new List<DiceSide>
                {
                    new DiceSide("TEST1"),
                    new DiceSide("TEST2")
                };
                List<Dice> dices = new List<Dice>
                {
                    new Dice(new SecureRandomizer(),new DiceSideType(1, sides[0]), new DiceSideType(2, sides[1])),
                    new Dice(new SecureRandomizer(),new DiceSideType(2, sides[0]), new DiceSideType(5, sides[1]))
                };
                Game g = new Game(new DiceType(1, dices[0]));

                yield return new object[]
                {
                    g,
                    sides,
                    dices,
                    new Dice(new SecureRandomizer(),new DiceSideType(1, sides[0])),
                    5
                };
            }

        }

        public static IEnumerable<object[]> DataRemovingDicesFromGame()
        {
            {
                List<DiceSide> sides = new List<DiceSide>
                {
                    new DiceSide("TEST1"),
                    new DiceSide("TEST2")
                };
                List<Dice> dices = new List<Dice>
                {
                    new Dice(new SecureRandomizer(),new DiceSideType(1, sides[0]), new DiceSideType(2, sides[1]))
                };
                Game g = new Game(new DiceType(5, dices[0]));

                yield return new object[]
                {
                    g,
                    sides,
                    dices,
                    dices[0],
                    5,
                    3
                };
            }
            {
                List<DiceSide> sides = new List<DiceSide>
                {
                    new DiceSide("TEST1"),
                    new DiceSide("TEST2")
                };
                List<Dice> dices = new List<Dice>
                {
                    new Dice(new SecureRandomizer(),new DiceSideType(1, sides[0]), new DiceSideType(2, sides[1])),
                    new Dice(new SecureRandomizer(),new DiceSideType(2, sides[0]), new DiceSideType(5, sides[1]))
                };
                Game g = new Game(new DiceType(1, dices[0]), new DiceType(2, dices[1]));

                yield return new object[]
                {
                    g,
                    sides,
                    dices,
                    dices[1],
                    2,
                    5
                };
            }
            {
                List<DiceSide> sides = new List<DiceSide>
                {
                    new DiceSide("TEST1"),
                    new DiceSide("TEST2")
                };
                List<Dice> dices = new List<Dice>
                {
                    new Dice(new SecureRandomizer(),new DiceSideType(1, sides[0]), new DiceSideType(2, sides[1])),
                    new Dice(new SecureRandomizer(),new DiceSideType(2, sides[0]), new DiceSideType(5, sides[1]))
                };
                Game g = new Game(new DiceType(1, dices[0]), new DiceType(2, dices[1]));

                yield return new object[]
                {
                    g,
                    sides,
                    dices,
                    dices[1],
                    2,
                    2
                };
            }

        }


        // =============================================== //
        //      PRIVATE
        // =============================================== //

        private static DataBaseLinker GetLinkerInMemory()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            var options = new DbContextOptionsBuilder<DiceLauncherDbContext>()
                .UseSqlite(connection)
                .Options;

            var context = new DiceLauncherDbContext(options);
            context.Database.EnsureCreated();
            StubedDatabaseLinker linker = new StubedDatabaseLinker(context);
            return linker;
        }

    }
}
