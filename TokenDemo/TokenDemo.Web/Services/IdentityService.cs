using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using TokenDemo.Web.DataContext;
using TokenDemo.Web.Models;

namespace TokenDemo.Web.Services
{
    public interface IIdentityService
    {
        Task<ResponseModel<TokenModel>> LoginAsync(LoginModel login);
    }
    public class IdentityService: IIdentityService
    {
        private readonly DemoTokenContext _context;
        private readonly ServiceConfiguration _appSettings;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public IdentityService(
            DemoTokenContext context,
            IOptions<ServiceConfiguration> settings,
            TokenValidationParameters tokenValidationParameters
            )
        {
            _context = context;
            _appSettings = settings.Value;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<ResponseModel<TokenModel>> LoginAsync(LoginModel login)
        {
            ResponseModel<TokenModel> response = new ResponseModel<TokenModel>();
            try
            {
                UsersMaster loginUser = _context.UsersMasters.FirstOrDefault(c => c.UserName == login.UserName && c.Password == login.Password);
                if (loginUser == null)
                {
                    response.IsSuccess = false;
                    response.Message = "Invalid Username and Password";
                    return response;
                }

                AuthenticationResult authenticationResult = await AuthenticateAsync(loginUser);
                if (authenticationResult != null && authenticationResult.Success)
                {
                    response.Data = new TokenModel()
                    {
                        Token = authenticationResult.Token,
                        RefreshToken = authenticationResult.RefreshToken
                    };
                }
                else
                {
                    response.Message = "Something went wrong!";
                    response.IsSuccess = false;
                }

                return response;
            }
            catch ( Exception ex )
            {
                throw ex;
            }
        }

        private List<RolesMaster> GetUserRoles(long userId)
        {
            try
            {
                List<RolesMaster> rolesMasters = (from UM in _context.UsersMasters
                                                 join UR in _context.UserRoles on UM.UserId equals UR.UserId
                                                 join RM in _context.RolesMasters on UR.RoleId equals RM.RoleId
                                                 where UM.UserId == userId
                                                 select RM).ToList();
                return rolesMasters;
            }
            catch (Exception ex)
            {
                return new List<RolesMaster>();
            }
        }


        public async Task<AuthenticationResult> AuthenticateAsync(UsersMaster user)
        {
            // authentication successful so generate jwt token
            AuthenticationResult authenticationResult = new AuthenticationResult();
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var key = Encoding.ASCII.GetBytes(_appSettings.JwtSettings.Secret);

                ClaimsIdentity subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim("FirstName", user.FirstName),
                    new Claim("LastName",user.LastName),
                    new Claim("EmailId",user.Email==null?"":user.Email),
                    new Claim("UserName",user.UserName==null?"":user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                });

                foreach (var item in GetUserRoles(user.UserId))
                {
                    subject.AddClaim(new Claim(ClaimTypes.Role, item.RoleName));
                }

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = subject,
                    Expires = DateTime.UtcNow.Add(_appSettings.JwtSettings.TokenLifetime),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);

                authenticationResult.Token = tokenHandler.WriteToken(token);

                var refreshToken = new RefreshToken
                {
                    Token = Guid.NewGuid().ToString(),
                    JwtId = token.Id,
                    UserId = user.UserId,
                    CreationDate = DateTime.UtcNow,
                    ExpiryDate = DateTime.UtcNow.AddMonths(6)
                };
                await _context.RefreshTokens.AddAsync(refreshToken);
                await _context.SaveChangesAsync();

                authenticationResult.RefreshToken = refreshToken.Token;
                authenticationResult.Success = true;
                return authenticationResult;
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
