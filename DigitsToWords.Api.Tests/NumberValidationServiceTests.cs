using NUnit.Framework;
using DigitsToWords.Api.Services;

namespace DigitsToWords.Api.Tests
{
    [TestFixture]
    public class NumberValidationServiceTests
    {
        private INumberValidationService _numberValidationService;

        [SetUp]
        public void SetUp()
        {
            _numberValidationService = new NumberValidationService();
        }

        [Test]
        public void IsValidNumber_WithValidWholeNumber_ReturnsTrue()
        {
            // Arrange
            string formattedNumber;

            // Act
            bool result = _numberValidationService.IsValidNumber("12345", out formattedNumber);

            // Assert
            Assert.That(result);
            Assert.That(formattedNumber, Is.EqualTo("12345"));
        }

        [Test]
        public void IsValidNumber_WithValidDecimalNumber_ReturnsTrue()
        {
            string formattedNumber;
            bool result = _numberValidationService.IsValidNumber("123.45", out formattedNumber);
            Assert.That(result);
            Assert.That(formattedNumber, Is.EqualTo("123.45"));
        }

        [Test]
        public void IsValidNumber_WithNumberContainingCommas_ReturnsTrue()
        {
            string formattedNUmber;
            bool result = _numberValidationService.IsValidNumber("1,234,567", out formattedNUmber);
            Assert.That(result);
            Assert.That(formattedNUmber, Is.EqualTo("1234567"));
        }

        [Test]
        public void IsValidNumber_WithEmptyString_ReturnsFalse()
        {
            string formattedNumber;
            bool result = _numberValidationService.IsValidNumber("", out formattedNumber);
            Assert.That(!result);
            Assert.That(formattedNumber, Is.EqualTo(""));
        }

        [Test]
        public void IsValidNumber_WithWhitespaceString_ReturnsFalse()
        {
            string formattedNumber;
            bool result = _numberValidationService.IsValidNumber("  ", out formattedNumber);
            Assert.That(!result);
            Assert.That(formattedNumber, Is.EqualTo("  "));
        }

        [Test]
        public void IsValidNumber_WithMultipleDecimalPoints_ReturnsFalse()
        {
            string formattedNumber;
            bool result = _numberValidationService.IsValidNumber("123.45.67", out formattedNumber);
            Assert.That(!result);
            Assert.That(formattedNumber, Is.EqualTo("123.45.67"));
        }

        [Test]
        public void IsValidNumber_WithInvalidCharacters_ReturnsFalse()
        {
            string formattedNumber;
            bool result = _numberValidationService.IsValidNumber("123a456", out formattedNumber);
            Assert.That(!result);
            Assert.That(formattedNumber, Is.EqualTo("123a456"));
        }

        [Test]
        public void IsValidNumber_WithJustDecimalPoint_ReturnsFalse()
        {
            string formattedNumber;
            bool result = _numberValidationService.IsValidNumber(".", out formattedNumber);
            Assert.That(!result);
            Assert.That(formattedNumber, Is.EqualTo("."));
        }

        [Test]
        public void IsValidNumber_WithNegativeNumber_ReturnsFalse()
        {
            string formattedNumber;
            bool result = _numberValidationService.IsValidNumber("-123", out formattedNumber);
            Assert.That(!result);
            Assert.That(formattedNumber, Is.EqualTo("-123"));
        }

        [Test]
        public void IsValidNumber_WithDecimalNumberEndingWithDecimalPoint_ReturnsTrue()
        {
            string formattedNumber;
            bool result = _numberValidationService.IsValidNumber("123.", out formattedNumber);
            Assert.That(result);
            Assert.That(formattedNumber, Is.EqualTo("123."));
        }
    }
}
