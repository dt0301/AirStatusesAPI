namespace StatusAirControl.Exeptions
{
    /// <summary>
    /// Базовый класс исключения, который содержит дополнительное свойство "Token".
    /// </summary>
    public class BaseException : Exception
    {
        /// <summary>
        /// Токен, связанный с исключением.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Конструктор исключения с сообщением об ошибке.
        /// </summary>
        /// <param name="token">Токен, связанный с исключением.</param>
        /// <param name="message">Сообщение об ошибке.</param>
        public BaseException(string token, string message) : base(message)
        {
            this.Token = token;
        }

        /// <summary>
        /// Конструктор исключения с сообщением об ошибке и внутренним исключением.
        /// </summary>
        /// <param name="token">Токен, связанный с исключением.</param>
        /// <param name="message">Сообщение об ошибке.</param>
        /// <param name="innerException">Внутреннее исключение.</param>
        public BaseException(string token, string message, Exception innerException)
            : base(message, innerException)
        {
            this.Token = token;
        }
    }
}
