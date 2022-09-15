namespace FibSubseqModel.Records
{
    public record FibbonacciCacheModel(int FirstIndex, int LastIndex, Task<long[]> Task);
}
