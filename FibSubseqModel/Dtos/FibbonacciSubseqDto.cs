namespace FibSubseqModel.Dtos
{
    public record class FibbonacciSubseqRequest
    {
        public int FirstIndex { get; init; }

        public int LastIndex { get; init; }

        public bool IsForCaching { get; init; }

        public int MillisecondsTimeout { get; init; }

        public int MaxMemoryKilobytes { get; init; }
    }
}
