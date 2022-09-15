using FibSubseqModel.Services;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace FibSubseqGeneratorTests
{
    public class FibbonacciServiceTests
    {
        [Fact]
        public async Task FibbonacciTets_ReturnsSequence()
        {
         var fibbonacci = new long[] { 0, 1, 1, 2, 3, 5, 8, 13, 21, 34, 55, 89,
                144,
                233,
                377,
                610,
                987,
                1597,
                2584,
                4181,
                6765,
                10946,
                17711,
                28657,
                46368,
                75025,
                 };

           // var fibbonachi = new long[] { 0, 1, 1, 2 };

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            var fibbonacciCacheService = new FibbonacciCacheService(memoryCache, new Mock<IConfiguration>().Object);

            var service = new FibbonacciService(fibbonacciCacheService, new Mock<IConfiguration>().Object);

            var firstTaskReq = new FibSubseqModel.Dtos.FibbonacciSubseqRequest
            {
                FirstIndex = 5,
                LastIndex = 10,
                MillisecondsTimeout = 1000000
            };

            var secondTaskReq = new FibSubseqModel.Dtos.FibbonacciSubseqRequest
            {
                FirstIndex = 5,
                LastIndex = 4,
                MillisecondsTimeout = 1000000
            };

            var task1 =  service.CalculateFibbonacciSubsequenceAsync(firstTaskReq);
            var task2 =  service.CalculateFibbonacciSubsequenceAsync(secondTaskReq);

            var result = await Task.WhenAll(new [] { task1, task2});

            // first task assertion
            Assert.Equal(fibbonacci.Skip(firstTaskReq.FirstIndex).Take(firstTaskReq.LastIndex - firstTaskReq.FirstIndex ).ToArray(), result[0].Sequence);
                
            // second task assertion
            Assert.Equal(fibbonacci.Skip(secondTaskReq.FirstIndex).Take(secondTaskReq.LastIndex - secondTaskReq.FirstIndex).ToArray(), result[1].Sequence);
        }
    }
}
