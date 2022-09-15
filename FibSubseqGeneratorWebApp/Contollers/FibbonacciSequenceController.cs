using Microsoft.AspNetCore.Mvc;
using FibSubseqModel.Dtos;
using FibSubseqModel.Services;

namespace FibSubseqGeneratorWebApp.Contollers
{
    public class FibbonacciSequenceController : ControllerBase
    {
        private readonly IFibbonacciService _fibbonacciService;
        
        public FibbonacciSequenceController(IFibbonacciService fibbonacciService)
        {
            _fibbonacciService = fibbonacciService;
        }

        [HttpGet("api/ping")]
        public string PingTest()
        {
            return "pong";
        }
        
        [HttpGet]
        [Route("api/fibbonacci")]
        public async Task<FibbonacciSubsequenceDto> GetSubsequenceAsync([FromQuery] FibbonacciSubseqRequest fibbonacciSubseqDto) {
            return await _fibbonacciService.CalculateFibbonacciSubsequenceAsync(fibbonacciSubseqDto);
        }
    }
}
