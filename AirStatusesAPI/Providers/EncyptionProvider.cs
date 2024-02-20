using System.Security.Cryptography;
using System.Text;

namespace AirStatusesAPI.Providers
{
    /// <summary>
    /// Класс EncryptionProvider предоставляет методы для шифрования и дешифрования текста с использованием симметричного ключа.
    /// </summary>
    public class EncryptionProvider
    {
        // Эта константа используется для определения размера ключа алгоритма шифрования в битах.
        // Мы делим это на 8 в коде ниже, чтобы получить эквивалентное количество байтов.
        private const int Keysize = 128;

        // Эта константа определяет количество итераций для функции генерации байтов пароля.
        private const int DerivationIterations = 1000;

        /// <summary>
        /// Шифрует исходный текст с использованием указанной фразы-пароля.
        /// </summary>
        /// <param name="plainText">Исходный текст для шифрования.</param>
        /// <param name="passPhrase">Фраза-пароль, используемая для генерации ключа шифрования.</param>
        /// <returns>Возвращает зашифрованный текст в виде строки Base64.</returns>
        public string Encrypt(string plainText, string passPhrase)
        {
            // Генерируем случайные байты для соли и вектора инициализации
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            // Преобразуем исходный текст в байты
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            // Создаем ключ шифрования из фразы-пароля и соли
            using var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations);
            var keyBytes = password.GetBytes(Keysize / 8);
            // Создаем объект симметричного ключа
            using var symmetricKey = Aes.Create("AesManaged");

            symmetricKey.BlockSize = 128;
            symmetricKey.Mode = CipherMode.CBC;
            symmetricKey.Padding = PaddingMode.PKCS7;

            // Создаем объект для шифрования
            using var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes);
            using var memoryStream = new MemoryStream();
            using var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);

            // Записываем исходные байты в поток шифрования
            cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
            cryptoStream.FlushFinalBlock();

            // Конкатенируем байты соли, вектора инициализации и зашифрованные байты
            var cipherTextBytes = saltStringBytes;
            cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
            cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            // Возвращаем зашифрованные байты в виде строки Base64
            return Convert.ToBase64String(cipherTextBytes);
        }

        /// <summary>
        /// Дешифрует зашифрованный текст с использованием указанной фразы-пароля.
        /// </summary>
        /// <param name="cipherText">Зашифрованный текст для дешифрования.</param>
        /// <param name="passPhrase">Фраза-пароль, используемая для генерации ключа шифрования.</param>
        /// <returns>Возвращает дешифрованный текст.</returns>
        public string Decrypt(string cipherText, string passPhrase)
        {
            // Преобразуем зашифрованный текст из строки Base64 в байты
            var cipherTextBytesWithSaltAndIv = Convert.FromBase64String(cipherText);
            // Извлекаем байты соли и вектора инициализации
            var saltStringBytes = cipherTextBytesWithSaltAndIv.Take(Keysize / 8).ToArray();
            var ivStringBytes = cipherTextBytesWithSaltAndIv.Skip(Keysize / 8).Take(Keysize / 8).ToArray();
            // Извлекаем собственно зашифрованные байты
            var cipherTextBytes = cipherTextBytesWithSaltAndIv.Skip((Keysize / 8) * 2).Take(cipherTextBytesWithSaltAndIv.Length - ((Keysize / 8) * 2)).ToArray();

            // Создаем ключ шифрования из фразы-пароля и соли
            using var password = new Rfc2898DeriveBytes(passPhrase, saltStringBytes, DerivationIterations);
            var keyBytes = password.GetBytes(Keysize / 8);
            // Создаем объект симметричного ключа
            using var symmetricKey = Aes.Create("AesManaged");

            symmetricKey.BlockSize = 128;
            symmetricKey.Mode = CipherMode.CBC;
            symmetricKey.Padding = PaddingMode.PKCS7;

            // Создаем объект для дешифрования
            using var decryptor = symmetricKey.CreateDecryptor(keyBytes, ivStringBytes);
            using var memoryStream = new MemoryStream(cipherTextBytes);
            using var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);

            // Читаем зашифрованные байты из потока дешифрования
            var plainTextBytes = new byte[cipherTextBytes.Length];
            var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            // Возвращаем дешифрованные байты в виде строки
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
        }

        /// <summary>
        /// Генерирует 256 бит случайной энтропии.
        /// </summary>
        /// <returns>Возвращает массив байтов, содержащий случайные байты.</returns>
        private static byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[16]; // 32 байта дадут нам 256 бит.
            using var rngCsp = RandomNumberGenerator.Create();

            // Заполняем массив криптографически безопасными случайными байтами.
            rngCsp.GetBytes(randomBytes);

            return randomBytes;
        }
    }
}

