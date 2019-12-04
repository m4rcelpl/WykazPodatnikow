using System;
using System.Linq;

namespace WykazPodatnikow.SharedLib
{
    public static class Extension
    {
        public static bool IsValidNIP(this string input)
        {
            int[] weights = { 6, 5, 7, 2, 3, 4, 5, 6, 7 };
            bool result = false;
            if (input.Length != 10)
            {
                return result;
            }
            int controlSum = CalculateControlSum(input, weights);
            int controlNum = controlSum % 11;
            if (controlNum == 10)
            {
                return result;
            }
            int lastDigit = int.Parse(input[input.Length - 1].ToString());
            result = controlNum == lastDigit;
            return result;
        }

        public static bool IsValidREGON(this string input)
        {
            int controlSum;
            if (input.Length == 7 || input.Length == 9)
            {
                int[] weights = { 8, 9, 2, 3, 4, 5, 6, 7 };
                int offset = 9 - input.Length;
                controlSum = CalculateControlSum(input, weights, offset);
            }
            else if (input.Length == 14)
            {
                int[] weights = { 2, 4, 8, 5, 0, 9, 7, 3, 6, 1, 2, 4, 8 };
                controlSum = CalculateControlSum(input, weights);
            }
            else
            {
                return false;
            }

            int controlNum = controlSum % 11;
            if (controlNum == 10)
            {
                controlNum = 0;
            }
            int lastDigit = int.Parse(input[input.Length - 1].ToString());

            return controlNum == lastDigit;
        }

        private static int CalculateControlSum(string input, int[] weights, int offset = 0)
        {
            int controlSum = 0;
            for (int i = 0; i < input.Length - 1; i++)
            {
                controlSum += weights[i + offset] * int.Parse(input[i].ToString());
            }
            return controlSum;
        }

        public static bool IsValidBankAccountNumber(string input)
        {
            input = input.Replace(" ", String.Empty);
            if (input.Length != 26 && input.Length != 32)
            {
                return false;
            }

            const int countryCode = 2521;
            string checkSum = input.Substring(0, 2);
            string accountNumber = input.Substring(2);
            string reversedDigits = accountNumber + countryCode + checkSum;
            return ModString(reversedDigits, 97) == 1;
        }

        private static int ModString(string x, int y)
        {
            if (string.IsNullOrEmpty(x))
            {
                return 0;
            }
            string firstDigit = x.Substring(0, x.Length - 1); // first digits
            int lastDigit = int.Parse(x.Substring(x.Length - 1)); // last digit
            return (ModString(firstDigit, y) * 10 + lastDigit) % y;
        }

        public static string SHA512(this string input)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(input);
            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(bytes);

                // Convert to text
                // StringBuilder Capacity is 128, because 512 bits / 8 bits in byte * 2 symbols for byte 
                var hashedInputStringBuilder = new System.Text.StringBuilder(128);
                foreach (var b in hashedInputBytes)
                    hashedInputStringBuilder.Append(b.ToString("X2"));
                return hashedInputStringBuilder.ToString().ToLower();
            }
        }

        public static string SHA512(this string input, int HowMenyReHash)
        {
            var hashedInputStringBuilder = new System.Text.StringBuilder(128);

            using (var hash = System.Security.Cryptography.SHA512.Create())
            {
                var hashedInputBytes = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(input));

                for (int i = 0; i < HowMenyReHash; i++)
                {
                    hashedInputStringBuilder.Clear();

                    foreach (var b in hashedInputBytes)
                        hashedInputStringBuilder.Append(b.ToString("X2"));

                    hashedInputBytes = hash.ComputeHash(System.Text.Encoding.UTF8.GetBytes(hashedInputStringBuilder.ToString().ToLower()));
                }

                return hashedInputStringBuilder.ToString().ToLower();
            }
        }
    }
}
