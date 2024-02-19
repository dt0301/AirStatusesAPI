using AirStatusesData.Services.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System.IdentityModel.Tokens.Jwt;

namespace AirStatusesApp.App.Helpers
{
    public static class UserPropsExtensions
    {
        public static void AddUserProps(this IServiceCollection services)
        {
            services.AddScoped<IUserProps, UserProps>();
        }
    }

    public class UserProps : IUserProps
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserProps(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<UserDto> GetUserPropsAsync()
        {
            return await Task.Run(() => GetUserProps());
        }

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

        public bool IsWriter(UserDto userDto)
        {
            return userDto != null && userDto.RoleCode == "Writer";
        }

        public bool IsAdmin(UserDto userDto)
        {
            return userDto != null && (userDto.RoleCode.Contains("Admin") || userDto.RoleCode.Contains("Administrator"));
        }
    }
}
