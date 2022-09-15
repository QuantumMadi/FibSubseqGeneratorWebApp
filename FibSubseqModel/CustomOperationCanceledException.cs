namespace FibSubseqModel
{
    public class CustomOperationCanceledException : Exception
    {
        public long[] PreparedResult { get; set; }

        public CustomOperationCanceledException(string? message, long[] preparedResult) : base(message)
        {
            PreparedResult = preparedResult;
        }
    }
}
