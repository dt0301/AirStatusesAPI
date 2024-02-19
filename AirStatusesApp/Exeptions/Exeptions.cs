using System.Net;

namespace StatusAirControl.Exeptions
{
    /// <summary>
    /// Класс исключения RestException, наследуемый от базового класса Exception.
    /// Используется для обработки исключений, связанных с HTTP-статусом.
    /// </summary>
    public class RestException : Exception
    {
        /// <summary>
        /// Конструктор класса RestException.
        /// </summary>
        /// <param name="code">HTTP-статус, который вызвал исключение.</param>
        /// <param name="errors">Объект, содержащий дополнительные данные об ошибке. По умолчанию равен null.</param>
        public RestException(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }

        /// <summary>
        /// Свойство Code. Получает HTTP-статус, который вызвал исключение.
        /// </summary>
        public HttpStatusCode Code { get; }

        /// <summary>
        /// Свойство Errors. Получает или устанавливает объект, содержащий дополнительные данные об ошибке.
        /// </summary>
        public object Errors { get; set; }
    }
}
