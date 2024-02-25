using System.Collections.Generic;

namespace DigitsToWords.Api.Services
{
    public interface INumberConversionService
    {
        string ConvertNumberToWords(string number);
    }
    public class NumberConversionService : INumberConversionService
    {
        // Static arrarys to hold words for numbers
        static readonly string[] ones = { "ONE", "TWO", "THREE", "FOUR", "FIVE", "SIX", "SEVEN", "EIGHT", "NINE" };
        static readonly string[] teens = { "ELEVEN", "TWELVE", "THIRTEEN", "FOURTEEN", "FIFTEEN", "SIXTEEN", "SEVENTEEN", "EIGHTEEN", "NINETEEN" };
        static readonly string[] tens = { "TEN", "TWENTY", "THIRTY", "FORTY", "FIFTY", "SIXTY", "SEVENTY", "EIGHTY", "NINETY" };
        static readonly string hundred = "HUNDRED";
        static readonly string thousand = "THOUSAND";
        static readonly string million = "MILLION";
        static readonly string billion = "BILLION";
        static readonly string trillion = "TRILLION";
       
        public string ConvertNumberToWords(string number)
        {
            // Splits the input string into dollars and cents parts.
            string[] dollarsAndCents = number.Split("."); 
            string dollars = dollarsAndCents[0];
            string? cents = dollarsAndCents.Length > 1 ? dollarsAndCents[1] : null;
            
            // Formatted cents with two-digits
            string formattedCents = FormatCentsToDisplay(cents);
            // Final result dollars in words
            List<string> wordsList = new List<string>();

            // Check for zero dollars, to directly return "ZERO"
            if (long.Parse(dollars) == 0)
            {
                wordsList.Add("ZERO");
            }
            else
            {
                List<string> chunks = DivideNumberIntoChunks(dollars);                
                for (int i = 0; i < chunks.Count; i++)
                {
                    int chunkIndex = i;
                    string chunk = chunks[i];
                    List<string> numericWords = chunk[0] == '0' ? ConvertDigitToWord(chunk.Substring(1)) : ConvertDigitToWord(chunk);

                    // Adding appropriate scale words (e.g., "THOUSAND", "MILLION" ) based on chunk position
                    if (numericWords.Count > 0)
                    {
                        var wordsWithScaleSuffix = AddScaleSuffix(chunkIndex, numericWords);
                        wordsList.Add(String.Join(" ", wordsWithScaleSuffix));
                    }
                }
            }

            // Reverse the result to match the original order
            wordsList.Reverse();
            if (wordsList.Count != 0)
            {
                wordsList.Add("DOLLARS");
            }
            
            // Handlling cents, adding "AND" and "CENTS" as necessary
            if (!string.IsNullOrEmpty(formattedCents))
            {
                if (int.Parse(formattedCents) > 0)
                {
                    wordsList.Add("AND");
                    wordsList.Add(AddCentsSuffix(ConvertDigitToWord(formattedCents)));
                }
                
            }

            // Adding space between words and adjusting spaces around hyphens
            string formattedNumberInWords = String.Join(" ", wordsList).Replace(" - ", "-");

            return formattedNumberInWords;
        }

        /*
         * Breaks a numerical string into three digit chunks or processing.
         * @param numericalString The numerical string to break into chunks
         * @return A list of three-digit (or fewer) chunks, in reverse order for processing
         */
        private List<string> DivideNumberIntoChunks(string numericalString)
        {
            List<string> chunks = new List<string>();
            // Loop to create chunks of up to three digits, starting from the end of the string
            for (int i = numericalString.Length; i > 0; i -= 3)
            {
                // Handling chunk with less than three digits
                if (i - 3 < 0)
                {
                    chunks.Add(numericalString.Substring(0, i));
                }
                else
                {
                    chunks.Add(numericalString.Substring(i - 3, 3));
                }
            }

            return chunks;
        }

        /*
         * Converts a numeric string chunk to its word representation
         * @param chunk A string representing a numeric chunk of up to three digits
         * @return A list of words representing the chunk
         */
        private List<string> ConvertDigitToWord(string chunk)
        {
            List<string> result = new List<string>();
            int lenChunk = chunk.Length;
            // Casting the string chunk into a long variable
            long chunkDigits = long.Parse(chunk);

            // Handling different chunk sizes (1-3 digits) separately
            if (lenChunk == 3 && chunkDigits > 0)
            {
                // Processing hundreds palce and the remainder
                long firstDigit = chunkDigits / 100;
                long remainder = chunkDigits % 100;

                if (firstDigit == 0)
                {
                    result.Add("AND");
                    result.AddRange(ConvertDigitToWord((remainder).ToString()));
                }
                else
                {   
                    // Converting the hundreds place and handling the remainder
                    string firstWord = ones[firstDigit - 1];
                    result.Add(firstWord);
                    result.Add(hundred);

                    if (remainder > 0)
                    {
                        result.Add("AND");
                        result.AddRange(ConvertDigitToWord((remainder).ToString()));
                    }
                    else
                    {
                        return result;
                    }
                }
            }
            else if (lenChunk == 2 & chunkDigits > 0)
            {
                // Handling tens and teens
                long secondDigit = chunkDigits / 10;
                long thirdDigit = chunkDigits % 10;
                string secondWord = "";
                if (secondDigit == 1)
                {
                    secondWord = thirdDigit == 0 ? tens[0] : teens[thirdDigit - 1];
                    result.Add(secondWord);
                    return result;
                }
                else if (secondDigit == 0)
                {
                    // Directly processing the unit's place if the ten's place is '0'
                    result.AddRange(ConvertDigitToWord((chunkDigits % 10).ToString()));
                }
                else
                {
                    // Check tens and map tens if the last digit is '0'
                    if (thirdDigit == 0)
                    {
                        secondWord = tens[secondDigit - 1];
                        result.Add(secondWord);
                        return result;
                    }
                    else
                    {
                        secondWord = tens[secondDigit - 1];
                        result.Add(secondWord);
                        result.Add("-");
                        result.AddRange(ConvertDigitToWord((chunkDigits % 10).ToString()));
                    }
                }
            }
            else
            {   
                if (chunkDigits > 0)
                {
                    // Converting the single digit numbers
                    long thirdDigit = chunkDigits;
                    string thirdWord = ones[thirdDigit - 1];
                    result.Add(thirdWord);
                }

                return result;
            }
            
            return result;
        }

        /*
         * Adds a scale word (e.g., "THOUSAND", "MILLION") to teh word lsit based on the chunk's position
         * @param chunksIndex The index of the chunk in the list of chunks
         * @param words The list of words representing the chunk, to which the suffix/scale word will be added
         * @return The updated list of words with the suffix/scale word added
         */
        private List<string> AddScaleSuffix(int chunkIndex, List<string> words)
        {
            switch (chunkIndex)
            {
                case 1: 
                    words.Add(thousand);
                    break;
                case 2: 
                    words.Add(million);
                    break;
                case 3: 
                    words.Add(billion);
                    break;
                case 4:
                    words.Add(trillion);
                    break;
            }

            return words;
        }

        /*
         * Formats the cents part of the number to ensure a two-digit representation
         * @param cents The cents part of the number as a string
         * @return A two-digit string representation of cents, properly formatted
         */
        private string FormatCentsToDisplay(string? cents)
        {
            if (!string.IsNullOrEmpty(cents))
            {
                cents = cents.Length > 2 ? cents.Substring(0, 2) : cents;

                if (cents.Length == 1)
                {
                    cents += "0";
                    return cents;
                }
            }

            return cents;
        }

        /*
         * Adds the word "CENTS" to the end of the cents part of the conversion
         * @param words The list of words representing the numerical value of cents
         * @return A string representing the cents followed by "CENTS"
         */
        private string AddCentsSuffix(List<string> words)
        {
            words.Add("CENTS");
            string centsInWords = String.Join(" ", words);
            return centsInWords;
        }
    }
}
