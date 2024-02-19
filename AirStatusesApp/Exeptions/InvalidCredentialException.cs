namespace StatusAirControl.Exeptions
{
    /// <summary>
    /// Класс InvalidCredentialException, наследуемый от базового класса BaseException.
    /// Используется для обработки исключений, связанных с недействительными учетными данными.
    /// </summary>
    public class InvalidCredentialException : BaseException
    {
        /// <summary>
        /// Конструктор класса InvalidCredentialException.
        /// </summary>
        /// <param name="ex">Исключение, которое может быть передано для получения дополнительной информации об ошибке. По умолчанию равно null.</param>
        public InvalidCredentialException(Exception ex = null) :
            /// Вызывает конструктор базового класса BaseException с сообщением об ошибке.
            base("invalid-credential", $"You provided a invalid credential: {ex?.Message}")
        {
        }
    }

}
