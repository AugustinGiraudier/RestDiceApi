using System;
using System.Collections.Generic;
using ModelAppLib;
using Xunit;

namespace ModelAppLib_UnitTests
{
    public class UT_DiceType
    {

        [Fact]
        void CreateObjectNotNull()
        {
            DiceType dt = new(3, new Dice(new SecureRandomizer(), new DiceSideType(3, new DiceSide("img1"))));
            Assert.NotNull(dt);
        }

        [Fact]
        void GetNbDices()
        {
            DiceType dt = new(3, new Dice(new SecureRandomizer(), new DiceSideType(3, new DiceSide("img1"))));
            Assert.Equal(3, dt.NbDices);
        }

        [Fact]
        void GetPrototypeWorks()
        {
            Dice d = new(new SecureRandomizer(), new DiceSideType(3, new DiceSide("img1")));
            DiceType dt = new(3, d);
            Assert.NotNull(dt.Prototype);
            Assert.True(d == dt.Prototype);
        }

        [Fact]
        void TestAddDice()
        {
            Dice d = new(new SecureRandomizer(), new DiceSideType(3, new DiceSide("img1")));
            DiceType dt = new(3, d);
            dt.AddDices(3);
            Assert.Equal(6, dt.NbDices);
        }

        [Theory]
        [MemberData(nameof(GetDatasForEquality))]
        void CheckEqual(Object obj1, Object obj2, bool shouldBeEqual)
        {
            if (obj1 != null)
                Assert.Equal(shouldBeEqual, obj1.Equals(obj2));
            if (obj2 != null)
                Assert.Equal(shouldBeEqual, obj2.Equals(obj1));
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
                new DiceType(3, new Dice(new SecureRandomizer(), new DiceSideType(3, new DiceSide("img1")))),
                new DiceType(3, new Dice(new SecureRandomizer(), new DiceSideType(3, new DiceSide("img1")))),
                true
            };
            
            yield return new object[]
            {
                new DiceType(2, new Dice(new SecureRandomizer(), new DiceSideType(3, new DiceSide("img1")))),
                new DiceType(3, new Dice(new SecureRandomizer(), new DiceSideType(3, new DiceSide("img1")))),
                false
            };
            
            yield return new object[]
            {
                new DiceType(3, new Dice(new SecureRandomizer(), new DiceSideType(2, new DiceSide("img1")))),
                new DiceType(3, new Dice(new SecureRandomizer(), new DiceSideType(3, new DiceSide("img1")))),
                false
            };
            
            yield return new object[]
            {
                new DiceType(3, new Dice(new SecureRandomizer(), new DiceSideType(3, new DiceSide("img2")))),
                new DiceType(3, new Dice(new SecureRandomizer(), new DiceSideType(3, new DiceSide("img1")))),
                false
            };
            
            yield return new object[]
            {
                new DiceType(3, new Dice(new SecureRandomizer(), new DiceSideType(3, new DiceSide("img2")))),
                null,
                false
            };
        }


    }
}
