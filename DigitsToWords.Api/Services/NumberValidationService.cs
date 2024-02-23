namespace DigitsToWords.Api.Services
{
    public interface INumberValidationService
    {
        bool IsValidNumber(string number, out string formattedNumber);
    }
    public class NumberValidationService : INumberValidationService
    {
        public bool IsValidNumber(string number, out string formattedNumber)
        {
            // Remove commas for validation
            formattedNumber = number.Replace(",", "");

            if (string.IsNullOrWhiteSpace(formattedNumber))
            {
                return false;
            }

            int decimalPointCount = formattedNumber.Count(c => c == '.');
            if (decimalPointCount > 1)
            {
                return false;
            }

            if (formattedNumber.Length == 1 && !char.IsDigit(formattedNumber[0]))
            {
                return false;
            }

            // Check if all characters are digits or a single decimal point
            return formattedNumber.All(num => char.IsDigit(num) || num == '.');

        }
    }
}
