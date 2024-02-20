using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AirStatusesAPI.Extensions
{
    public static class StringExtension
    {
        /// <summary>
        /// Преобразует входную строку в формат "snake_case".
        /// </summary>
        /// <param name="input">Входная строка для преобразования.</param>
        /// <returns>Строка в формате "snake_case".</returns>
        public static string ToSnakeCase(this string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return input;
            }

            var startUnderscores = Regex.Match(input, @"^_+");
            return startUnderscores + Regex.Replace(input, @"([a-z0-9])([A-Z])", "$1_$2").ToLower();
        }

        /// <summary>
        /// Преобразует входную строку в хеш SHA256.
        /// </summary>
        /// <param name="input">Входная строка для хеширования.</param>
        /// <returns>Хеш SHA256 входной строки.</returns>
        public static string ToSha256(this string input)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha256.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        /// <summary>
        /// Преобразует входную строку в хеш SHA512.
        /// </summary>
        /// <param name="input">Входная строка для хеширования.</param>
        /// <returns>Хеш SHA512 входной строки.</returns>
        public static string ToSha512(this string input)
        {
            using var sha512 = SHA512.Create();
            var bytes = Encoding.UTF8.GetBytes(input);
            var hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        /// <summary>
        /// Преобразует входную строку в хеш MD5.
        /// </summary>
        /// <param name="input">Входная строка для хеширования.</param>
        /// <returns>Хеш MD5 входной строки.</returns>
        public static string ToMd5(this string input)
        {
            using var md5 = MD5.Create();
            var bytes = Encoding.ASCII.GetBytes(input);
            var hash = md5.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }

        /// <summary>
        /// Преобразует байтовый массив хеша в строку.
        /// </summary>
        /// <param name="hash">Байтовый массив хеша.</param>
        /// <returns>Строковое представление хеша.</returns>
        private static string GetStringFromHash(byte[] hash)
        {
            var result = new StringBuilder();
            foreach (var t in hash)
            {
                result.Append(t.ToString("X2"));
            }
            return result.ToString();
        }

        /// <summary>
        /// Проверяет, является ли символ кириллицей.
        /// </summary>
        /// <param name="s">Символ для проверки.</param>
        /// <returns>Возвращает true, если символ является кириллицей, иначе false.</returns>
        private static readonly Regex regexCyrlics = new Regex(@"\p{IsCyrillic}");
        public static bool IsCyrylic(this char s)
        {
            return regexCyrlics.IsMatch(s.ToString());
        }

        /// <summary>
        /// Проверяет, является ли входная строка адресом электронной почты.
        /// </summary>
        /// <param name="input">Входная строка для проверки.</param>
        /// <returns>Возвращает true, если входная строка является адресом электронной почты, иначе false.</returns>
        private static readonly Regex regexMail = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
        public static bool IsMail(this string input)
        {
            return regexMail.IsMatch(input);
        }
    }
}

