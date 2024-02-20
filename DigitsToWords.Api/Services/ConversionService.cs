using System.Collections.Generic;

namespace DigitsToWords.Api.Services
{
    public interface IConversionService
    {
        string ConvertNumberToWords(string number);
    }
    public class ConversionService : IConversionService
    {
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

            string[] dollarsAndCents = number.Split(".");
            string dollars = dollarsAndCents[0];
            string? cents = dollarsAndCents.Length > 1 ? dollarsAndCents[1] : null;

            string limitedCents = CheckCents(cents);
            
            List<string> result = new List<string>();

            if (int.Parse(dollars) == 0)
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
                    if (chunk[0] == '0')
                    {
                        string modifiedChunk = chunk.Substring(1);
                        words = DigitToWordMapping(modifiedChunk);
                    }
                    else
                    {
                        words = DigitToWordMapping(chunk);
                    }
                    //foreach (string word in words) 
                    //{ 
                    //    result.Add(word);
                    //}
                    if (words.Count > 0)
                    {
                        var suffixAdded = EvaluateSuffix(chunkIndex, words);
                        string resultWithSpace = String.Join(" ", suffixAdded);
                        result.Add(resultWithSpace);
                    }


                }
            }

            
            //result = EvaluateSuffix(chunks, words);

            result.Reverse();
            if (result.Count != 0)
            {
                result.Add("DOLLARS");
            }
            
            if (!string.IsNullOrEmpty(limitedCents))
            {
                if (int.Parse(limitedCents) > 0)
                {
                    result.Add("AND");
                    result.Add(AddCentsSuffix(DigitToWordMapping(limitedCents)));
                }
                
            }
            string finalResult = String.Join(" ", result);

            string adjustedResutl = finalResult.Replace(" - ", "-");

            return adjustedResutl;
        }

        private List<string> DivideIntoChunks(string numericalString)
        {
            List<string> chunks = new List<string>();

            for (int i = numericalString.Length; i > 0; i -= 3)
            {
                if (i - 3 < 0)
                {
                    chunks.Add(numericalString.Substring(0, i));
                }
                else
                {
                    chunks.Add(numericalString.Substring(i - 3, 3));
                }
            }

            //chunks.Reverse();

            return chunks;
        }

        private List<string> DigitToWordMapping(string chunk)
        {
            List<string> result = new List<string>();
            int lenChunk = chunk.Length;
            int chunkDigits = int.Parse(chunk);

            if (lenChunk == 3 && chunkDigits > 0)
            {
                int firstDigit = chunkDigits / 100;
                int remainder = chunkDigits % 100;

                if (firstDigit == 0)
                {
                    result.Add("AND");
                    result.AddRange(DigitToWordMapping((remainder).ToString()));
                }
                else
                {   
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
                
                int secondDigit = chunkDigits / 10;
                int thirdDigit = chunkDigits % 10;
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
                    result.AddRange(DigitToWordMapping((chunkDigits % 10).ToString()));
                }
                else
                {
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
                    int thirdDigit = chunkDigits;
                    string thirdWord = ones[thirdDigit - 1].ToUpperInvariant();
                    result.Add(thirdWord);
                }

             return result;
            }
            
            return result;
        }

        private List<string> EvaluateSuffix(int chunksIndex, List<string> words)
        {
                if (chunksIndex == 1)
                {
                    words.Add(thousand);
                    return words;
                } 
                else if (chunksIndex == 2)
                {
                    words.Add(million);
                    return words;
                }
                else if (chunksIndex == 3)
                {
                    words.Add(billion);
                    return words;
                }

            return words;
        }

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

        private string AddCentsSuffix(List<string> words)
        {
            words.Add("CENTS");
            string resultWithSpace = String.Join(" ", words);
            return resultWithSpace;
        }
    }
}
