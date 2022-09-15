namespace FibSubseqModel.Dtos
{
    public class FibbonacciSubsequenceDto
    {
        public long[] Sequence { get; }

        public string FromResource { get; }

        public string ExceptionMessage { get; }

        public FibbonacciSubsequenceDto(long[] sequence, string fromRersource, string exceptionMessage = "")
        {
            Sequence = sequence;
            FromResource = fromRersource;
            ExceptionMessage = exceptionMessage;
        }
    }
}
