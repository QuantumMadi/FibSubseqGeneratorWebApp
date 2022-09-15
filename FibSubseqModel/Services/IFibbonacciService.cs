using FibSubseqModel.Dtos;

namespace FibSubseqModel.Services
{
    public interface IFibbonacciService
    {
        Task<FibbonacciSubsequenceDto> CalculateFibbonacciSubsequenceAsync(FibbonacciSubseqRequest fibbonacciSubseqDto);
    }
}
