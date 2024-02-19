using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using AirStatusesDomain;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace AirStatusesInfrastructure.Security
{
    /// <summary>
    /// Класс для генерации JWT (JSON Web Token).
    /// </summary>
    public class JwtGenerator : IJwtGenerator
    {
        private readonly SymmetricSecurityKey _key;
        private readonly IConfiguration _configuration;

        /// <summary>
        /// Конструктор класса JwtGenerator.
        /// </summary>
        /// <param name="configuration">Конфигурация приложения.</param>
		public JwtGenerator(IConfiguration configuration)
        {
            // Создание ключа безопасности из конфигурации
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSecret"]));
            _configuration = configuration;
        }

        /// <summary>
        /// Генерирует JWT-токен для пользователя.
        /// </summary>
        /// <param name="user">Пользователь, для которого генерируется токен.</param>
        /// <returns>Возвращает DTO токена, если токен действителен. В противном случае возвращает null.</returns>
        public TokenDto? GenerateJwtToken(User user)
        {
            // Создание списка утверждений для пользователя
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.Code),
                // Добавьте другие claims здесь
            };

            // Создание учетных данных для подписи
            var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);

            // Создание дескриптора токена
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // Создание обработчика токена
            var tokenHandler = new JwtSecurityTokenHandler();

            // Создание токена
            var token = tokenHandler.CreateToken(tokenDescriptor);

            // Создание DTO токена
            var tokenDto = new TokenDto
            {
                Token = tokenHandler.WriteToken(token)
            };

            // Валидация токена
            var isValidToken = ValidateToken(tokenDto.Token);
            if (isValidToken)
            {
                return tokenDto;
            }
            return null;
        }


        /// <summary>
        /// Проверяет действительность JWT-токена.
        /// </summary>
        /// <param name="token">Токен для проверки.</param>
        /// <returns>Возвращает true, если токен действителен. В противном случае возвращает false.</returns>
        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtSecret")));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false
                }, out SecurityToken validatedToken);
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}