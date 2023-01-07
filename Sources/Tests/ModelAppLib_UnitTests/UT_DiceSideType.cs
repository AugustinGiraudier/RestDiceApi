using System;
using System.Collections.Generic;
using ModelAppLib;
using Xunit;


namespace ModelAppLib_UnitTests
{
    public class UT_DiceSideType
    {

        [Fact]
        void CreateObjectNotNull()
        {
            DiceSideType dst = new(3, new DiceSide("img1.png"));
            Assert.NotNull(dst);
        }

        [Fact]
        void GetNbSide()
        {
            DiceSideType dst = new(3, new DiceSide("img1.png"));
            Assert.Equal(3, dst.NbSide);
        }

        [Fact]
        void GetPrototypeWorks()
        {
            DiceSide ds = new DiceSide("img1.png");
            DiceSideType dst = new(3, ds);
            Assert.NotNull(dst.Prototype);
            Assert.True(ds == dst.Prototype);
        }

        [Fact]
        void AddingSideWorks()
        {
            DiceSideType dst = new(3, new DiceSide("img1.png"));
            dst.AddSides(2);
            Assert.Equal(5, dst.NbSide);
        }

        [Theory]
        [MemberData(nameof(GetDatasForEquality))]
        void CheckEqual(DiceSideType obj1, DiceSideType obj2, bool shouldBeEqual)
        {
            if (obj1 != null)
                Assert.Equal(shouldBeEqual, obj1.Equals(obj2 as Object));
            if (obj2 != null)
                Assert.Equal(shouldBeEqual, obj2.Equals(obj1 as Object));
        }

        [Theory]
        [MemberData(nameof(GetDatasForEquality))]
        void HashCodesWork(Object d1, Object d2, bool shouldItHaveSameHash)
        {
            if (d1 != null && d2 != null)
            {
                Assert.Equal(shouldItHaveSameHash, d1.GetHashCode() == d2.GetHashCode());
            }
        }

        public static IEnumerable<object[]> GetDatasForEquality()
        {
            yield return new object[]
            {
                new DiceSideType(3 , new DiceSide("img1")),
                new DiceSideType(3 , new DiceSide("img1")),
                true
            };
            
            yield return new object[]
            {
                new DiceSideType(4 , new DiceSide("img1")),
                new DiceSideType(3 , new DiceSide("img1")),
                false
            };
            
            yield return new object[]
            {
                new DiceSideType(3 , new DiceSide("img2")),
                new DiceSideType(3 , new DiceSide("img1")),
                false
            };

            yield return new object[]
            {
                new DiceSideType(3 , new DiceSide("img2")),
                null,
                false
            };
        }

    }
}
