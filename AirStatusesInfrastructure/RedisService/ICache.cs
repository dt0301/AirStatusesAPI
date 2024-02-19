using AirStatusesDomain;

namespace AirStatusesInfrastructure.RedisService
{
    /// <summary>
    /// Интерфейс ICache определяет контракт для кэширования данных.
    /// </summary>
    public interface ICache
    {
        /// <summary>
        /// Устанавливает значение в кэш по ключу.
        /// </summary>
        /// <param name="key">Ключ для установки значения.</param>
        /// <param name="value">Значение для установки.</param>
        void Set(string key, string value);

        /// <summary>
        /// Обновляет значение в кэше по ключу.
        /// </summary>
        /// <param name="key">Ключ для обновления значения.</param>
        /// <param name="newValue">Новое значение для установки.</param>
        void Update(string key, string newValue);

        /// <summary>
        /// Получает значение из кэша по ключу.
        /// </summary>
        /// <param name="key">Ключ для получения значения.</param>
        /// <returns>Значение из кэша.</returns>
        string Get(string key);

        /// <summary>
        /// Удаляет значение из кэша по ключу.
        /// </summary>
        /// <param name="key">Ключ для удаления значения.</param>
        void Remove(string key);

        /// <summary>
        /// Асинхронно получает список рейсов из кэша по шаблону ключа.
        /// </summary>
        /// <param name="keyPattern">Шаблон ключа для получения рейсов.</param>
        /// <returns>Список рейсов, соответствующих шаблону ключа.</returns>
        Task<List<Flight>> GetFlightsByPatternAsync(string keyPattern);
    }
}

