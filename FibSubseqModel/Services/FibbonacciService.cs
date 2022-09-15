using FibSubseqModel.Dtos;
using FibSubseqModel.Records;
using Microsoft.Extensions.Configuration;

namespace FibSubseqModel.Services
{
    public class FibbonacciService : IFibbonacciService
    {
        private IFibbonacciCacheService _fibCacheService;
        private readonly int CalculationThreadCount = 2;

        public FibbonacciService(IFibbonacciCacheService fibCacheService, IConfiguration configuration)
        {
            _fibCacheService = fibCacheService ?? throw new ArgumentNullException(nameof(fibCacheService));

            if (int.TryParse(configuration["CalculationThreadCount"], out int threadCount))
            {
                CalculationThreadCount = threadCount;
            }
        }

        public async Task<FibbonacciSubsequenceDto> CalculateFibbonacciSubsequenceAsync(FibbonacciSubseqRequest fibbonacciSubseqDto)
        {
            var cachedSequence = _fibCacheService.TryGetCachedSequence(fibbonacciSubseqDto.FirstIndex, fibbonacciSubseqDto.LastIndex);

            if (cachedSequence.Any())
            {
                return new FibbonacciSubsequenceDto(cachedSequence, "From sequence cache");
            }

            try
            {
                var isCachedTask = _fibCacheService.TryGetCachedTask(out Task<long[]> task);
                var cancellationToken = GetTimeoutCancellationToken(fibbonacciSubseqDto.MillisecondsTimeout);
                
                if (!isCachedTask)
                {
                    task = GetFibbonacciSequenceAsync(fibbonacciSubseqDto.FirstIndex, fibbonacciSubseqDto.LastIndex, cancellationToken: cancellationToken.Token);
                    _fibCacheService.SetCachedTask(new FibbonacciCacheModel(fibbonacciSubseqDto.FirstIndex, fibbonacciSubseqDto.LastIndex, task));
                }

                var result = await task;

                if (fibbonacciSubseqDto.LastIndex > result.Length)
                {
                    result = await GetFibbonacciSequenceAsync(
                        startIndex: fibbonacciSubseqDto.FirstIndex,
                        lastIndex: fibbonacciSubseqDto.LastIndex,
                        array: result,
                        cancellationToken: cancellationToken.Token);
                }

                var resToReturn = result
                    .Skip(fibbonacciSubseqDto.FirstIndex)
                    .Take(fibbonacciSubseqDto.LastIndex - fibbonacciSubseqDto.FirstIndex)
                    .ToArray();

                if (fibbonacciSubseqDto.IsForCaching)
                {
                    _fibCacheService.SetCachedSequence(fibbonacciSubseqDto.FirstIndex, fibbonacciSubseqDto.LastIndex, resToReturn);
                };

                return new FibbonacciSubsequenceDto(resToReturn, $"Is from cached task: {isCachedTask}");
            }
            catch (CustomOperationCanceledException ex)
            {
                Console.WriteLine("Operations time is up");
                return new FibbonacciSubsequenceDto(ex.PreparedResult, "Not from cache", "Operations time is up, return calculated result");
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private CancellationTokenSource GetTimeoutCancellationToken(int millisecondsTimeout)
        {
            var cancelationSource = new CancellationTokenSource();

            int cancelletionTime = 100;
            if (millisecondsTimeout > cancelletionTime)
            {
                cancelletionTime = millisecondsTimeout;
            }

            cancelationSource.CancelAfter(cancelletionTime);

            return cancelationSource;
        }

        private async Task<long[]> GetFibbonacciSequenceAsync(int startIndex, int lastIndex, long[] array = null, CancellationToken cancellationToken = default)
        {
            int threadCount = CalculationThreadCount;

            List<Task> tasks = new();

            if (array == null)
            {
                array = new long[] { 0 };
            }

            for (int tCtr = 0; tCtr < threadCount; tCtr++)
            {
                var task = Task.Run(() =>
                {
                    for (int i = array.Length - 1; i < lastIndex; i++)
                    {
                        if (cancellationToken.IsCancellationRequested)
                        {
                            throw new CustomOperationCanceledException("Operation is canceled", array);
                        }

                        lock (array)
                        {
                            if (array.Length < lastIndex)
                            {
                                array = FibbonacciCountUtil.FibSub(array);
                            }
                        }
                    }
                });
                tasks.Add(task);
            }

            await Task.WhenAll(tasks);
            return array.ToArray();
        }
    }
}
