using ModelAppLib;
using Xunit;

namespace ModelAppLib_UnitTests
{
    public class UT_SecureRandomize
    {
        
        [Fact]
        void CreateObjectNotNull()
        {
            var rd = new SecureRandomizer();
            Assert.NotNull(rd);
        }

        [Theory]
        [InlineData(0,10,50)]
        [InlineData(0,100,50)]
        [InlineData(0,2,100)]
        void TestRandom(int min, int max, int nbTest)
        {
            var rd = new SecureRandomizer();
            for(int iTest=0; iTest<nbTest; iTest++)
            {
                int val = rd.GetRandomInt(min, max);
                Assert.True(min <= val && val < max);
            }
        }

    }
}
