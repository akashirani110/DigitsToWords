using DigitsToWords.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitsToWords.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NumberToWordsController : ControllerBase
    {
        private readonly IConversionService _conversionService;

        public NumberToWordsController(IConversionService conversionService)
        {
            _conversionService = conversionService;
        }

        [HttpGet]
        [Route("convert")]
        public IActionResult ConvertToWords([FromQuery]string number)
        {
            string words = _conversionService.ConvertNumberToWords(number);
            return Ok(words);
        }
    }
}
