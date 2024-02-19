using System.Security.Claims;
using AirStatusesData;
using AirStatusesDomain;

namespace AirStatusesAPI.Providers
{
    public class AuthenticatedUserProvider
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly AppDbContext context;
        public AuthenticatedUserProvider(IHttpContextAccessor httpContextAccessor, AppDbContext context)
        {
            this.httpContextAccessor = httpContextAccessor;
            this.context = context;
        }

        public User? GetAuthenticatedUser()
        {
            try
            {
                var claims = httpContextAccessor?.HttpContext?.User?.Claims;
                if (claims != null && claims.Any())
                {
                    var tokenId = claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    if (tokenId != null)
                    {
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

        public JwtUserDto GetAuthenticatedJwtUser()
        {
            try
            {
                var userClaimes = httpContextAccessor?.HttpContext?.User?.Claims;
                if (userClaimes != null && userClaimes.Any())
                {
                    var tokenId = httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                    var tokenLogin = httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                    var role = httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role)?.Value;

                    //var tokenEmail = httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
                    //var apps = JsonConvert.DeserializeObject<long[]>(httpContextAccessor?.HttpContext?.User?.Claims.FirstOrDefault(x => x.Type == "Apps")?.Value);

                    if (tokenId != null)
                    {
                        var id = Convert.ToInt32(tokenId);
                        return new JwtUserDto(id,  tokenLogin, role);
                    }
                } 
            }
            catch (Exception ex)
            {
                throw new Exception("GetAuthenticatedJwtUser:", ex);
            }

            return new JwtUserDto(0, null, null);
        }
    }
}
