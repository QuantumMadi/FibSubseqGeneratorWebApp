using FibSubseqModel.Records;

namespace FibSubseqModel.Services
{
    public interface IFibbonacciCacheService
    {
        bool TryGetCachedTask(out Task<long[]> task);
        
        void SetCachedTask(FibbonacciCacheModel fibbonacciCacheModel);

        void SetCachedSequence(int startIndex, int lastIndex, long[]sequence);

        long[] TryGetCachedSequence(int startIndex, int lastIndex);
    }
}
