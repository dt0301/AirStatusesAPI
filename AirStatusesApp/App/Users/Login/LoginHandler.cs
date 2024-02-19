using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AirStatusesApp.App.Dto;
using AirStatusesApp.Exeptions;
using AirStatusesData;
using AirStatusesData.Services;
using AirStatusesDomain;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using StatusAirControl.Exeptions;

namespace AirStatusesApp.App.Users.Login
{
    public class LoginHandler : IRequestHandler<CredentialDto, TokenDto>
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IUserDataService _userContext;
        public LoginHandler(/*IJwtGenerator jwtGenerator,*/ IConfiguration configuration, AppDbContext context, IUserDataService userContext)
        {
            //_jwtGenerator = jwtGenerator;
            _configuration = configuration;
            _context = context;
            _userContext = userContext;
        }

        async Task<TokenDto> IRequestHandler<CredentialDto, TokenDto>.Handle(CredentialDto request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userContext.GetUserByNamePassword(request.UserName, request.Password);
                return GenerateJwtToken(user);
            }
            catch (Exception ex)
            {
                //_logger.LogError($"Login exception: {ex.Message}");
                throw new InvalidCredentialException(ex);
            }
        }

        public TokenDto? GenerateJwtToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.Role, user.Role.Code),
                // Добавьте другие claims здесь
            };
            //var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JwtSecret")));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetValue<string>("JwtSecret")));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            var tokenDto = new TokenDto
            {
                Token = tokenHandler.WriteToken(token)
            };

            var isValidToken = ValidateToken(tokenDto.Token);
            if (isValidToken)
            {
                return tokenDto;
            }
            return null;
        }

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

        /// <summary>
        /// GetJwtToken by user claims
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        /// <exception cref="EntityNotfoundException"></exception>
        private async Task<TokenDto> GetJwtToken(User user)
        {
            if (user is null) throw new EntityNotfoundException(new Exception("GetJwtToken Exception"), 0, "User");

            var currentUser = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == user.Id);
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetValue<string>("JwtSecret"));
            string role = JsonConvert.SerializeObject(currentUser?.Role.Code);

            //string apps = JsonConvert.SerializeObject(currentUser.PortalApps.Select(p => p.Id).ToArray());

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.UserName != null ? user.UserName: ""),
                    new Claim(ClaimTypes.Role, value: user.Role.Code),

                    //new Claim(ClaimTypes.Email, user.Email != null ? user.Email.ToString() : ""),
                    //new Claim(type: "Apps", value: apps)
                }),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);

            var token = new TokenDto
            {
                Token = tokenHandler.WriteToken(securityToken)
            };

            return token;
        }
    }
}
