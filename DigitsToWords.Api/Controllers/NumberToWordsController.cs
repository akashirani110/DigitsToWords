using DigitsToWords.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace DigitsToWords.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NumberToWordsController : ControllerBase
    {
        private readonly INumberValidationService _numberValidationService;
        private readonly INumberConversionService _numberConversionService;

        public NumberToWordsController(INumberValidationService numberValidationService, INumberConversionService numberConversionService)
        {
            _numberValidationService = numberValidationService;
            _numberConversionService = numberConversionService;
        }

        [HttpGet]
        [Route("convert")]
        public IActionResult ConvertToWords([FromQuery]string number)
        {
            try
            {
                // Validate the input number string
                if (!_numberValidationService.IsValidNumber(number, out string formattedNumber))
                {
                    return BadRequest(new { error = "Please enter a valid positive number containing only digits" });
                }

                // Convert the number to words
                string words = _numberConversionService.ConvertNumberToWords(formattedNumber);
                return Ok(words);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occured while converting the number to words: {ex.Message}");
            }
        }
    }
}
