using NUnit.Framework;
using DigitsToWords.Api.Services;

namespace DigitsToWords.Api.Tests
{
    [TestFixture]
    public class NumberConversionServiceTests
    {
        private INumberConversionService _numberConversionService;

        [SetUp]
        public void Setup()
        {
            _numberConversionService = new NumberConversionService();
        }

        [Test]
        public void ConvertNumberToWords_SingleDigit_ReturnsWords()
        {
            var result = _numberConversionService.ConvertNumberToWords("1");
            Assert.That(result, Is.EqualTo("ONE DOLLARS"));
        }

        [Test]
        public void ConvertNumberToWords_TeenNumber_ReturnsWord()
        {
            var result = _numberConversionService.ConvertNumberToWords("15");
            Assert.That(result, Is.EqualTo("FIFTEEN DOLLARS"));
        }

        [Test]
        public void ConvertNumberToWords_Tens_ReturnsWord()
        {
            var result = _numberConversionService.ConvertNumberToWords("50");
            Assert.That(result, Is.EqualTo("FIFTY DOLLARS"));
        }

        [Test]
        public void ConvertNumberToWords_Hundred_ReturnsWord()
        {
            var result = _numberConversionService.ConvertNumberToWords("100");
            Assert.That(result, Is.EqualTo("ONE HUNDRED DOLLARS"));
        }

        [Test]
        public void ConvertNumberToWords_Thousand_ReturnsWord()
        {
            var result = _numberConversionService.ConvertNumberToWords("1000");
            Assert.That(result, Is.EqualTo("ONE THOUSAND DOLLARS"));
        }

        [Test]
        public void ConvertNumberToWords_ComplexThousand_ReturnsWord()
        {
            var result = _numberConversionService.ConvertNumberToWords("78990");
            Assert.That(result, Is.EqualTo("SEVENTY-EIGHT THOUSAND NINE HUNDRED AND NINETY DOLLARS"));
        }

        [Test]
        public void ConvertNumberToWords_Million_ReturnsWord()
        {
            var result = _numberConversionService.ConvertNumberToWords("1000000");
            Assert.That(result, Is.EqualTo("ONE MILLION DOLLARS"));
        }

        [Test]
        public void ConvertNumberToWords_ComplexMillion_ReturnsWord()
        {
            var result = _numberConversionService.ConvertNumberToWords("1555000");
            Assert.That(result, Is.EqualTo("ONE MILLION FIVE HUNDRED AND FIFTY-FIVE THOUSAND DOLLARS"));
        }

        [Test]
        public void ConvertNumberToWords_Billion_ReturnsWord()
        {
            var result = _numberConversionService.ConvertNumberToWords("1000000000");
            Assert.That(result, Is.EqualTo("ONE BILLION DOLLARS"));
        }

        [Test]
        public void ConvertNumberToWords_ComplexBillion_ReturnsWord()
        {
            var result = _numberConversionService.ConvertNumberToWords("10000550000");
            Assert.That(result, Is.EqualTo("TEN BILLION FIVE HUNDRED AND FIFTY THOUSAND DOLLARS"));
        }

        [Test]
        public void ConvertNumberToWords_Trillion_ReturnsWord()
        {
            var result = _numberConversionService.ConvertNumberToWords("7000000000000");
            Assert.That(result, Is.EqualTo("SEVEN TRILLION DOLLARS"));
        }

        [Test]
        public void ConvertNumberToWords_WithCents_ReturnsWord()
        {
            var result = _numberConversionService.ConvertNumberToWords("134.87");
            Assert.That(result, Is.EqualTo("ONE HUNDRED AND THIRTY-FOUR DOLLARS AND EIGHTY-SEVEN CENTS"));
        }

        [Test]
        public void ConvertNumberToWords_WithOneCentDigit_ReturnsWord()
        {
            var result = _numberConversionService.ConvertNumberToWords("134.7");
            Assert.That(result, Is.EqualTo("ONE HUNDRED AND THIRTY-FOUR DOLLARS AND SEVENTY CENTS"));
        }

        [Test]
        public void ConvertNumberToWords_WithDecimalPointAtEndWithoutCents_ReturnsWord()
        {
            var result = _numberConversionService.ConvertNumberToWords("123.");
            Assert.That(result, Is.EqualTo("ONE HUNDRED AND TWENTY-THREE DOLLARS"));
        }

        [Test]
        public void ConvertNumberToWords_Zero_ReturnsWord()
        {
            var result = _numberConversionService.ConvertNumberToWords("0");
            Assert.That(result, Is.EqualTo("ZERO DOLLARS"));
        }
    }
}