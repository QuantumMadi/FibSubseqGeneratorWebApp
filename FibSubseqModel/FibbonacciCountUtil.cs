namespace FibSubseqModel
{
    public static class FibbonacciCountUtil
    {  
        public static long[] FibSub(long[]array) // { 0, 1 } size 2
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);

            var arr = new long[array.Length + 1]; // { 0, 0, 0 } size = 3

            array.CopyTo(arr, 0); // {0, 1, 0}
            arr[array.Length] = Fib(array.Length); //   arr [1]  = 

            return arr;
        }

        public static long[] FibSubseq(int startIndex, int endIndex, CancellationToken cancellationToken = default)
        {
            if (startIndex < endIndex)
            {
                long[] fibbonacciSeq = new long[endIndex];

                for (int i = 0; i < endIndex; i++)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        break;
                    }

                    fibbonacciSeq[i] = Fib(i);
                }

                return fibbonacciSeq.Skip(startIndex).ToArray();
            }
            throw new InvalidOperationException($"Start index should be less then end index");
        }

        private static long Fib(long n)
        {
            if (n == 0)
            {
                return 0;
            }
            else if (n == 1)
            {
                return 1;
            }
            else
            {
                return Fib(n - 1) + Fib(n - 2);
            }
        }
    }
}