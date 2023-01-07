using System;
using System.Collections.Generic;
using ModelAppLib;
using Xunit;

namespace ModelAppLib_UnitTests
{
    public class UT_DiceSide
    {

        [Fact]
        void CreateObjectNotNull()
        {
            DiceSide ds = new("img");
            Assert.NotNull(ds);
        }

        [Fact]
        void CanGetImage()
        {
            DiceSide ds = new("imagePath");
            Assert.Equal("imagePath", ds.Image);
        }

        [Theory]
        [InlineData("img1","img2", false)]
        [InlineData("img1","img1", true)]
        [InlineData("test test","test test", true)]
        [InlineData("","", true)]
        [InlineData(""," ", false)]
        void EqualityComprarerWorks(String img1, String img2, bool shouldItBeEqual)
        {
            DiceSide ds = new(img1);
            DiceSide ds2 = new(img2);

            Assert.Equal(shouldItBeEqual, ds.Equals(ds2));
            Assert.Equal(shouldItBeEqual, ds2.Equals(ds));
        }

        [Theory]
        [InlineData("","",true)]
        [InlineData("img1","img1",true)]
        [InlineData("img","img2",false)]
        [InlineData(""," ",false)]
        void HashCodesWork(String img1, String img2, bool shouldHaveSameCode)
        {
            Assert.Equal(shouldHaveSameCode, new DiceSide(img1).GetHashCode() == new DiceSide(img2).GetHashCode());
        }

        [Fact]
        void EqualityComparationWithOtherClassIsFalse()
        {
            Assert.False(new DiceSide("img").Equals(new DiceSideType(2, new DiceSide("img"))));
        }

    }
}
