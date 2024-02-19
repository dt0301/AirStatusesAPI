using AirStatusesData.Services.Dto;
using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace AirStatusesApp.App.Users
{
    public static class UserProps
    {
        public static UserDto GetUserProps(IHttpContextAccessor httpContextAccessor)
        {
            // Получаем токен из заголовка "Authorization"
            string authToken = httpContextAccessor.HttpContext.Request.Headers["Authorization"];

            if (string.IsNullOrEmpty(authToken) || !authToken.StartsWith("Bearer "))
            {
                throw new Exception("Invalid authorization token");
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = tokenHandler.ReadJwtToken(authToken.Substring("Bearer ".Length));

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
    }
}
