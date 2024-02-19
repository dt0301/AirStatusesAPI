using AirStatusesData.Services.Dto;

namespace AirStatusesApp.App.Helpers
{
    /// <summary>
    /// Интерфейс IUserProps предоставляет методы для работы с свойствами пользователя.
    /// </summary>
    public interface IUserProps
    {
        /// <summary>
        /// Получает свойства пользователя.
        /// </summary>
        /// <returns>Возвращает объект UserDto, содержащий свойства пользователя.</returns>
        UserDto GetUserProps();

        /// <summary>
        /// Асинхронно получает свойства пользователя.
        /// </summary>
        /// <returns>Возвращает задачу, результатом которой является объект UserDto, содержащий свойства пользователя.</returns>
        Task<UserDto> GetUserPropsAsync();

        /// <summary>
        /// Проверяет, является ли пользователь писателем.
        /// </summary>
        /// <param name="userDto">Объект UserDto, содержащий свойства пользователя.</param>
        /// <returns>Возвращает true, если пользователь является писателем, иначе false.</returns>
        bool IsWriter(UserDto userDto);

        /// <summary>
        /// Проверяет, является ли пользователь администратором.
        /// </summary>
        /// <param name="userDto">Объект UserDto, содержащий свойства пользователя.</param>
        /// <returns>Возвращает true, если пользователь является администратором, иначе false.</returns>
        bool IsAdmin(UserDto userDto);
    }
}
