using System;
using System.Collections.Generic;
using ModelAppLib;
using Xunit;

namespace ModelAppLib_UnitTests
{
    public class UT_Dice
    {
        [Fact]
        internal void CreateObjectNotNull()
        {
            var lst = new List<DiceSideType>
            {
                new DiceSideType(1, new DiceSide("img1"))
            };
            Dice d = new(new SecureRandomizer(), lst);
            Assert.NotNull(d);
        }

        [Fact]
        void CreateObjectWithNullCollection()
        {
            List<DiceSideType> ldt = null;
            Assert.Throws<ArgumentNullException>(() => new Dice(new SecureRandomizer(), ldt));

            List<DiceSideType> ldt2 = new List<DiceSideType>{ 
                new DiceSideType(2, new DiceSide("img")),
                null
            };
            Assert.Throws<ArgumentNullException>(() => new Dice(new SecureRandomizer(), ldt2));
            
            Assert.Throws<ArgumentNullException>(() => new Dice(new SecureRandomizer(), new DiceSideType(2, new DiceSide("img")),null));
        }

        [Fact]
        void GettingSidesNotNull()
        {
            var lst = new List<DiceSideType>()            {
                new DiceSideType(1, new DiceSide("img1"))
            };
            Dice d = new(new SecureRandomizer(), lst);
            Assert.NotNull(d.SideTypes);
        }

        [Fact]
        void AddingSidesWorking()
        {
            DiceSide ds = new("imgPath");
            DiceSideType dst = new DiceSideType(1, ds);
            var lst = new List<DiceSideType>();
            lst.Add(dst);
            Dice d = new(new SecureRandomizer(), lst);
            Assert.Single(d.SideTypes);
        }

        [Fact]
        void AddingSideThatAlreadyExistWorks()
        {
            DiceSideType dst = new DiceSideType(1, new DiceSide("imgPath"));
            var lst = new List<DiceSideType>();
            lst.Add(dst);
            Dice d = new(new SecureRandomizer(), lst);
            d.AddSide(dst);
            Assert.Single(d.SideTypes);
            Assert.Equal(2, d.SideTypes[0].NbSide);
        }

        [Fact]
        void AddingMultipleSidesWorks()
        {
            var lst = new List<DiceSideType>()            {
                new DiceSideType(1, new DiceSide("img1"))
            };
            Dice d = new(new SecureRandomizer(), lst);
            d.AddSide(new DiceSideType(2, new DiceSide("img")));
            d.AddSide(new DiceSideType(3, new DiceSide("img2")));
            Assert.Equal(3, d.SideTypes.Count);
            Assert.Equal(6, d.GetTotalSides());
        }

        [Theory]
        [MemberData(nameof(GetDatasForEquality))]
        void EqualityComparerWorks(Object d1, Object d2, bool shouldItBeEqual)
        {
            if(d1 != null)
                Assert.Equal(shouldItBeEqual, d1.Equals(d2));
            if(d2 != null)
                Assert.Equal(shouldItBeEqual, d2.Equals(d1));
        }

        [Theory]
        [MemberData(nameof(GetDatasForEquality))]
        void HashCodesWork(Object d1, Object d2, bool shouldItHaveSameHash)
        {
            if(d1 != null && d2 != null)
            {
                Assert.Equal(shouldItHaveSameHash, d1.GetHashCode() == d2.GetHashCode());
            }
        }

        [Theory]
        [MemberData(nameof(GetDatasForNumberOfSides))]
        void CheckTotalNumberOfSides(Dice d, int theoricalNumberOfSides)
        {
            Assert.Equal(theoricalNumberOfSides, d.GetTotalSides());
        }

        [Theory]
        [MemberData(nameof(GetDatasForIndexesOfSides))]
        void CheckIndexOfSide(Dice d, int index, DiceSide ds)
        {
            Assert.True(d[index] == ds);
        }

        public static IEnumerable<object[]> GetDatasForIndexesOfSides()
        {
            DiceSide ds = new DiceSide("img");
            yield return new object[]
            {
                new Dice(new SecureRandomizer(), 
                    new DiceSideType(3, ds),
                    new DiceSideType(3, new DiceSide("img"))
                ),
                0,
                ds
            };

            DiceSide ds2 = new DiceSide("img2");
            yield return new object[]
            {
                new Dice(new SecureRandomizer(), 
                    new DiceSideType(3, ds2),
                    new DiceSideType(3, new DiceSide("img"))
                ),
                2,
                ds2
            };

            DiceSide ds3 = new DiceSide("img1");
            yield return new object[]
            {
                new Dice(new SecureRandomizer(), 
                    new DiceSideType(3, new DiceSide("img2")),
                    new DiceSideType(3, ds3)
                ),
                3,
                ds3
            };

            yield return new object[]
            {
                new Dice(new SecureRandomizer(), 
                    new DiceSideType(3, new DiceSide("img2")),
                    new DiceSideType(3, new DiceSide("img2"))
                ),
                6,
                null
            };

            yield return new object[]
            {
                new Dice(new SecureRandomizer(), 
                    new DiceSideType(3, new DiceSide("img2")),
                    new DiceSideType(3, new DiceSide("img2"))
                ),
                7,
                null
            };
        }

        public static IEnumerable<object[]> GetDatasForNumberOfSides()
        {
            yield return new object[]
            {
                new Dice(new SecureRandomizer(), 
                    new DiceSideType(3, new DiceSide("img")),
                    new DiceSideType(3, new DiceSide("img2"))
                ),
                6
            };
            
            yield return new object[]
            {
                new Dice(new SecureRandomizer(), 
                    new DiceSideType(2, new DiceSide("img2")),
                    new DiceSideType(3, new DiceSide("img2"))
                ),
                5
            };

            yield return new object[]
            {
                new Dice(new SecureRandomizer(), 
                    new DiceSideType(2, new DiceSide("img2")),
                    new DiceSideType(2, new DiceSide("img")),
                    new DiceSideType(3, new DiceSide("img2"))
                ),
                7
            };
        }

        public static IEnumerable<object[]> GetDatasForEquality()
        {
            yield return new object[]
            {
                new Dice(new SecureRandomizer(), 
                    new List<DiceSideType>{
                        new DiceSideType(1,new DiceSide("img1")),
                        new DiceSideType(1,new DiceSide("img1"))
                    } 
                ),
                new Dice(new SecureRandomizer(), 
                    new List<DiceSideType>{
                        new DiceSideType(2,new DiceSide("img1"))
                    }
                ),
                true
            };

            yield return new object[]
            {
                new Dice(new SecureRandomizer(), 
                    new List<DiceSideType>{
                        new DiceSideType(1,new DiceSide("img1")),
                        new DiceSideType(1,new DiceSide("img2"))
                    }
                ),
                new Dice(new SecureRandomizer(), 
                    new List<DiceSideType>{
                        new DiceSideType(2,new DiceSide("img1"))
                    }
                ),
                false
            };
            
            yield return new object[]
            {
                new Dice(new SecureRandomizer(), 
                    new List<DiceSideType>{
                        new DiceSideType(1,new DiceSide("img1")),
                        new DiceSideType(1,new DiceSide("img2"))
                    }
                ),
                new DiceSide("img"),
                false
            };
            
            yield return new object[]
            {
                new Dice(new SecureRandomizer(), 
                    new List<DiceSideType>{
                        new DiceSideType(1,new DiceSide("img1")),
                        new DiceSideType(1,new DiceSide("img2"))
                    }
                ),
                null,
                false
            };
        }

    }
}
