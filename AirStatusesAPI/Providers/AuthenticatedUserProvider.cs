using System.Security.Claims;
using AirStatusesData;
using AirStatusesDomain;

namespace AirStatusesAPI.Providers
{
    /// <summary>
    /// Класс AuthenticatedUserProvider предназначен для получения информации об аутентифицированном пользователе.
    /// </summary>
    public class AuthenticatedUserProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AppDbContext context;

        /// <summary>
        /// Конструктор класса AuthenticatedUserProvider.
        /// </summary>
        /// <param name="httpContextAccessor">Предоставляет доступ к HttpContext.</param>
        /// <param name="context">Контекст базы данных.</param>
        public AuthenticatedUserProvider(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
        }

        /// <summary>
        /// Получает информацию об аутентифицированном пользователе.
        /// </summary>
        /// <returns>Возвращает объект User, если пользователь аутентифицирован, иначе null.</returns>
        public User? GetAuthenticatedUser()
        {
            try
            {
                // Получаем claims из контекста текущего запроса
                var claims = httpContextAccessor?.HttpContext?.User?.Claims;
                if (claims != null && claims.Any())
                {
                    // Ищем claim с типом NameIdentifier, который обычно содержит идентификатор пользователя
                    var tokenId = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    if (tokenId != null)
                    {
                        // Преобразуем идентификатор пользователя в int и ищем пользователя в базе данных
                        var id = Convert.ToInt32(tokenId);
                        return context.Set<User>().FirstOrDefault(x => x.Id == id);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetAuthenticatedUser:", ex);
            }

            return null;
        }

        /// <summary>
        /// Получает информацию об аутентифицированном пользователе в формате JwtUserDto.
        /// </summary>
        /// <returns>Возвращает объект JwtUserDto, содержащий информацию об аутентифицированном пользователе.</returns>
        public JwtUserDto GetAuthenticatedJwtUser()
        {
            try
            {
                // Получаем claims из контекста текущего запроса
                var userClaimes = httpContextAccessor?.HttpContext?.User?.Claims;
                if (userClaimes != null && userClaimes.Any())
                {
                    // Ищем различные claims, которые могут содержать информацию о пользователе
                    var tokenId = httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    var tokenLogin = httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                    var role = httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

                    if (tokenId != null)
                    {
                        // Преобразуем идентификатор пользователя в int и создаем новый объект JwtUserDto
                        var id = Convert.ToInt32(tokenId);
                        return new JwtUserDto(id, tokenLogin, role);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("GetAuthenticatedJwtUser:", ex);
            }

            // Если пользователь не аутентифицирован, возвращаем объект JwtUserDto с пустыми значениями
            return new JwtUserDto(0, null, null);
        }
    }
}

