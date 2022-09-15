using FibSubseqModel;
using Xunit;
using Xunit.Abstractions;

namespace FibSubseqGeneratorTests
{
    public class FiboonacciUtilTests
    {
        [Fact]
        public void FibbonacciTets_ReturnsSequence()
        {
            var fibbonachi = new long[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89,
                144,
                233,
                377,
                610,
                987 };


            var result = new long[] {0};

            for (int i = 0; i < fibbonachi.Length - 1; i++)
            {
                result = FibbonacciCountUtil.FibSub(result);
            }
            
            Assert.Equal(fibbonachi, result);
        }
    }
}
