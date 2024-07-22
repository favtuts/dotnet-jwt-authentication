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
        Task<ResponseModel<TokenModel>> RefreshTokenAsync(TokenModel request);
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


        public async Task<ResponseModel<TokenModel>> RefreshTokenAsync(TokenModel request)
        {
            ResponseModel<TokenModel> response = new ResponseModel<TokenModel>();
            try
            {
                var authResponse = await GetRefreshTokenAsync(request.Token, request.RefreshToken);
                if (!authResponse.Success)
                {
                    response.IsSuccess = false;
                    response.Message = string.Join(",", authResponse.Errors);
                    return response;
                }
                TokenModel refreshTokenModel = new TokenModel();
                refreshTokenModel.Token = authResponse.Token;
                refreshTokenModel.RefreshToken = authResponse.RefreshToken;
                response.Data = refreshTokenModel;
                return response;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.Message = "Something went wrong!";
                return response;
            }
        }

        private async Task<AuthenticationResult> GetRefreshTokenAsync(string token, string refreshToken)
        {
            var validatedToken = GetPrincipalFromToken(token);

            if (validatedToken == null)
            {
                return new AuthenticationResult { Errors = new[] { "Invalid Token" } };
            }

            var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            if (expiryDateTimeUtc > DateTime.UtcNow)
            {
                return new AuthenticationResult { Errors = new[] { "This token hasn't expired yet" } };
            }

            var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = _context.RefreshTokens.FirstOrDefault(x => x.Token == refreshToken);

            if (storedRefreshToken == null)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh token does not exist" } };
            }

            if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh token has expired" } };
            }

            if (storedRefreshToken.Used.HasValue && storedRefreshToken.Used == true)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh token has been used" } };
            }

            if (storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult { Errors = new[] { "This refresh token does not match this JWT" } };
            }

            storedRefreshToken.Used = true;
            _context.RefreshTokens.Update(storedRefreshToken);
            await _context.SaveChangesAsync();
            string strUserId = validatedToken.Claims.Single(x => x.Type == "UserId").Value;
            long userId = 0;
            long.TryParse(strUserId, out userId);
            var user = _context.UsersMasters.FirstOrDefault(c => c.UserId == userId);
            if (user == null)
            {
                return new AuthenticationResult { Errors = new[] { "User Not Found" } };
            }

            return await AuthenticateAsync(user);
        }

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                var tokenValidationParameters = _tokenValidationParameters.Clone();
                tokenValidationParameters.ValidateLifetime = false;
                var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                   jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                       StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
