using System;

namespace number_to_devnagari_currency_text
{
    public class Converter
    {
        public static string NumberToCurrencyText(decimal number, MidpointRounding midpointRounding)
        {
            number = decimal.Round(number, 2, midpointRounding);

            string[] arrNumber = number.ToString().Split('.');

            long wholePart = long.Parse(arrNumber[0]);


            string[] ones = new string[] { "zero", "One", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Eleven", "Twelve", "Thirteen", "Fourteen", "Fifteen", "Sixteen", "Seventeen", "Eighteen", "Nineteen" };

            string[] powers = new string[] { "Thousand ", "Lakhs ", "Crores ", "Arab" };
            string[] tens = new string[] { "Twenty", "Thirty", "Forty", "Fifty", "Sixty", "Seventy", "Eighty", "Ninety" };

            string wordNumber = "";

            while (wholePart > 19)
            {
                char[] wholeNumberIntoCharacterArray = wholePart.ToString().ToCharArray();

                if (wholeNumberIntoCharacterArray.Length < 3)
                {
                    wordNumber += $"{tens[(wholePart / 10) - 2] }";
                    wholePart = wholePart % 10;
                }

                else if (wholeNumberIntoCharacterArray.Length < 4)
                {
                    int hundredNumber = Convert.ToInt32(Math.Floor(Convert.ToDecimal(wholePart / 100)));
                    wordNumber += $"{ones[hundredNumber]} hundred ";
                    wholePart = Convert.ToInt32(wholePart % 100);
                }
                else
                {
                    //get index of power
                    int powerIndex = 0;
                    int lengthOfNumber = wholeNumberIntoCharacterArray.Length;
                    bool isLengthOfNumberEven = lengthOfNumber % 2 == 0;

                    string divisibleNumberInStringFormat = "1";

                    int numberOfZerosInDivisibleNumber = 0;

                    if (isLengthOfNumberEven)
                    {
                        powerIndex = (lengthOfNumber - 4) / 2;
                        numberOfZerosInDivisibleNumber = lengthOfNumber - 1;
                    }
                    else
                    {
                        numberOfZerosInDivisibleNumber = lengthOfNumber - 2;
                        powerIndex = ((lengthOfNumber - 1) - 4) / 2;
                    }
                    for (int i = 0; i < numberOfZerosInDivisibleNumber; i++)
                    {
                        divisibleNumberInStringFormat += "0";
                    }

                    int divisibleNumber = Convert.ToInt32(divisibleNumberInStringFormat);

                    int tensNumber = Convert.ToInt32(Math.Floor(Convert.ToDecimal(wholePart / divisibleNumber)));

                    if (tensNumber > 19)
                    {
                        wordNumber += $" { tens[(tensNumber / 10) - 2] } ";
                        tensNumber %= 10;
                    }
                    wordNumber += $" {ones[tensNumber]} {powers[powerIndex]} ";

                    wholePart = Convert.ToInt32(wholePart % divisibleNumber);
                }
            }

            if (wholePart > 0)
            {
                wordNumber += $" {ones[wholePart]}";
            }
            wordNumber += " Rupees";

            wordNumber = appendPaisaIfPaisaPresent(arrNumber, ones, tens, wordNumber);

            return wordNumber;
        }

        private static string appendPaisaIfPaisaPresent(string[] arrNumber, string[] ones, string[] tens, string wordNumber)
        {
            bool isNumberDecimal = arrNumber.Length == 2;
            if (isNumberDecimal)
            {
                int paisa = Convert.ToInt32(arrNumber[1]);
                if (paisa > 0)
                {
                    paisa = paisa > 9 ? paisa : paisa * 10;
                    wordNumber += " and";
                    int indexOfPaisa = paisa > 9 ? paisa : (paisa * 10);
                    if (paisa > 19)
                    {
                        wordNumber += $" { tens[(paisa / 10) - 2] } ";
                        paisa %= 10;
                    }
                    if (paisa > 0)
                    {
                        wordNumber += $" {ones[paisa]}";
                    }
                    wordNumber += $" paisa";
                }
            }

            return wordNumber;
        }
    }
}
