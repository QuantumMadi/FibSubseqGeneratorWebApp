using FibSubseqModel.Records;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace FibSubseqModel.Services
{
    public class FibbonacciCacheService : IFibbonacciCacheService
    {
        private readonly IMemoryCache _memoryCache;
        private const string TaskCacheKey = "FibbonacciTaskCacheKey";
        private int CacheTtl = 1000;
        public FibbonacciCacheService(IMemoryCache memoryCache, IConfiguration configuration)
        {
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(memoryCache));
            if (int.TryParse(configuration["CacheTtlPeriod"], out int cacheTtl))
            {
                CacheTtl = cacheTtl;
            }
        }
        public void SetCachedTask(FibbonacciCacheModel fibbonacciCacheModel) =>
            _memoryCache.Set(TaskCacheKey, fibbonacciCacheModel, new MemoryCacheEntryOptions().SetSize(10));
        

        public bool TryGetCachedTask(out Task<long[]> task)
        {
            if (_memoryCache.TryGetValue(TaskCacheKey, out FibbonacciCacheModel cache))
            {
                if (cache.Task.IsCompleted)
                {
                    _memoryCache.Remove(TaskCacheKey);
                }
                task = cache.Task;
                return true;
            }
            task = default;

            return false;
        }

        public void SetCachedSequence(int startIndex, int lastIndex, long[] sequence)
        {
            var cacheEntryOption = new MemoryCacheEntryOptions()
                .SetSize(10)
                .SetAbsoluteExpiration(TimeSpan.FromMilliseconds(CacheTtl));
            
            _memoryCache.Set($"{startIndex}-{lastIndex}", sequence, cacheEntryOption);
        }

        public long[] TryGetCachedSequence(int startIndex, int lastIndex)
        {
            if (_memoryCache.TryGetValue($"{startIndex}-{lastIndex}", out long[] sequence))
            {
                return sequence;
            }
            return Array.Empty<long>();
        }
    }
}
