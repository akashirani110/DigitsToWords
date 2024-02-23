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
            string limitedCents = CheckCents(cents);
            // Final result dollars in words
            List<string> result = new List<string>();

            // Check for zero dollars, to directly return "ZERO"
            if (long.Parse(dollars) == 0)
            {
                result.Add("ZERO");
            }
            else
            {
                List<string> chunks = DivideIntoChunks(dollars);
                List<string> words = new List<string>();
                for (int i = 0; i < chunks.Count; i++)
                {
                    int chunkIndex = i;
                    string chunk = chunks[i];
                    // If a chunk starts with '0', modify it by removing the '0'
                    if (chunk[0] == '0')
                    {
                        string modifiedChunk = chunk.Substring(1);
                        words = DigitToWordMapping(modifiedChunk);
                    }
                    else
                    {
                        words = DigitToWordMapping(chunk);
                    }

                    // Adding appropriate scale words (e.g., "THOUSAND", "MILLION" ) based on chunk position
                    if (words.Count > 0)
                    {
                        var suffixAdded = EvaluateSuffix(chunkIndex, words);
                        string resultWithSpace = String.Join(" ", suffixAdded);
                        result.Add(resultWithSpace);
                    }
                }
            }

            // Reverse the result to match the original order
            result.Reverse();
            if (result.Count != 0)
            {
                result.Add("DOLLARS");
            }
            
            // Handlling cents, adding "AND" and "CENTS" as necessary
            if (!string.IsNullOrEmpty(limitedCents))
            {
                if (int.Parse(limitedCents) > 0)
                {
                    result.Add("AND");
                    result.Add(AddCentsSuffix(DigitToWordMapping(limitedCents)));
                }
                
            }
            string finalResult = String.Join(" ", result);
            // Adjusting spaces around hyphens
            string adjustedResutl = finalResult.Replace(" - ", "-");

            return adjustedResutl;
        }

        /*
         * Breaks a numerical string into three digit chunks or processing.
         * @param numericalString The numerical string to break into chunks
         * @return A list of three-digit (or fewer) chunks, in reverse order for processing
         */
        private List<string> DivideIntoChunks(string numericalString)
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
         * Maps a numeric string chunk to its word representation
         * @param chunk A string representing a numeric chunk of up to three digits
         * @return A list of words representing the chunk
         */
        private List<string> DigitToWordMapping(string chunk)
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
                    result.AddRange(DigitToWordMapping((remainder).ToString()));
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
                        result.AddRange(DigitToWordMapping((remainder).ToString()));
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
                    if (thirdDigit == 0)
                    {
                        secondWord = tens[0].ToUpperInvariant();
                    }
                    else
                    {
                        secondWord = teens[thirdDigit - 1].ToUpperInvariant();
                    }
                    result.Add(secondWord);
                    return result;
                }
                else if (secondDigit == 0)
                {
                    // Directly processing the unit's place if the ten's place is '0'
                    result.AddRange(DigitToWordMapping((chunkDigits % 10).ToString()));
                }
                else
                {
                    // Check tens and map tens if the last digit is '0'
                    if (thirdDigit == 0)
                    {
                        secondWord = tens[secondDigit - 1].ToUpperInvariant();
                        result.Add(secondWord);
                        return result;
                    }
                    else
                    {
                        secondWord = tens[secondDigit - 1].ToUpperInvariant();
                        result.Add(secondWord);
                        result.Add("-");
                        result.AddRange(DigitToWordMapping((chunkDigits % 10).ToString()));
                    }
                }
            }
            else
            {   
                if (chunkDigits > 0)
                {
                    // Converting the single digit numbers
                    long thirdDigit = chunkDigits;
                    string thirdWord = ones[thirdDigit - 1].ToUpperInvariant();
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
        private List<string> EvaluateSuffix(int chunksIndex, List<string> words)
        {
            switch (chunksIndex)
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
        private string CheckCents(string? cents)
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
            string resultWithSpace = String.Join(" ", words);
            return resultWithSpace;
        }
    }
}
