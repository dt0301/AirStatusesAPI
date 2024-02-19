using AirStatusesData.Services.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace AirStatusesApp.App.Helpers
{
    /// <summary>
    /// Статический класс расширений для IServiceCollection.
    /// </summary>
    public static class UserPropsExtensions
    {
        /// <summary>
        /// Добавляет IUserProps в коллекцию сервисов.
        /// </summary>
        /// <param name="services">Коллекция сервисов.</param>
        public static void AddUserProps(this IServiceCollection services)
        {
            services.AddScoped<IUserProps, UserProps>();
        }
    }

    /// <summary>
    /// Класс UserProps, реализующий интерфейс IUserProps.
    /// </summary>
    public class UserProps : IUserProps
    {
        // Доступ к контексту HTTP.
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Конструктор класса UserProps.
        /// </summary>
        /// <param name="httpContextAccessor">Доступ к контексту HTTP.</param>
        public UserProps(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Асинхронно получает свойства пользователя.
        /// </summary>
        /// <returns>Возвращает задачу, результатом которой является объект UserDto, содержащий свойства пользователя.</returns>
        public async Task<UserDto> GetUserPropsAsync()
        {
            return await Task.Run(() => GetUserProps());
        }

        /// <summary>
        /// Получает свойства пользователя.
        /// </summary>
        /// <returns>Возвращает объект UserDto, содержащий свойства пользователя, или null, если токен авторизации отсутствует или недействителен.</returns>
        public UserDto? GetUserProps()
        {
            // Получаем токен из заголовка "Authorization"
            string authToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authToken) || !authToken.StartsWith("Bearer "))
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(authToken.Replace("Bearer ", string.Empty));

            UserDto userDto = new();

            // Если утверждение не найдено, генерируем исключение
            userDto.Id = int.Parse(jwtToken.Claims.FirstOrDefault(claim => claim.Type == "nameid")?
                .Value ?? throw new Exception("Token does not contain expected claims"));

            userDto.UserName = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "unique_name")?
                .Value ?? throw new Exception("Token does not contain expected claims");

            userDto.RoleCode = jwtToken.Claims.FirstOrDefault(claim => claim.Type == "role")?
                .Value ?? throw new Exception("Token does not contain expected claims");

            return userDto;
        }

        /// <summary>
        /// Проверяет, является ли пользователь писателем.
        /// </summary>
        /// <param name="userDto">Объект UserDto, содержащий свойства пользователя.</param>
        /// <returns>Возвращает true, если пользователь является писателем, иначе false.</returns>
        public bool IsWriter(UserDto userDto)
        {
            return userDto != null && userDto.RoleCode == "Writer";
        }

        /// <summary>
        /// Проверяет, является ли пользователь администратором.
        /// </summary>
        /// <param name="userDto">Объект UserDto, содержащий свойства пользователя.</param>
        /// <returns>Возвращает true, если пользователь является администратором, иначе false.</returns>
        public bool IsAdmin(UserDto userDto)
        {
            return userDto != null && (userDto.RoleCode.Contains("Admin") || userDto.RoleCode.Contains("Administrator"));
        }
    }
}

