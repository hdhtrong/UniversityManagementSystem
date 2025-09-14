using AuthService.Applications.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Shared.SharedAuth.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Services.Implementations
{
    public class JwtManagerService : IJwtManagerService
    {
        private readonly IConfiguration _configuration;

        public JwtManagerService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IUTokens GenerateTokens(SignInUser user)
        {
            // create claims
            List<Claim> claims =
            [
                new Claim(ClaimTypes.Name, user.Fullname),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim("code", user.Code),
            ];
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            foreach (var credential in user.Credentials)
            {
                claims.Add(new Claim(Shared.SharedKernel.Constants.CREDENTIAL_CLAIM, credential));
            }
            // generate token based on user data from the database
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
            int tokenLifetime = int.TryParse(_configuration["JWT:Lifetime"], out tokenLifetime) ? tokenLifetime : Shared.SharedKernel.Constants.DEFAULT_TOKEN_LIFETIME;
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = _configuration["JWT:Issuer"],
                Audience = _configuration["JWT:Audience"],
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(tokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var strToken = tokenHandler.WriteToken(token);
            return new IUTokens { Token = strToken };
        }

        public bool IsTokenValid(string token)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var validationParameters = GetValidationParameters();
                SecurityToken validatedToken;
                IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters, out validatedToken);

                if (validatedToken.ValidFrom.Ticks <= DateTime.Now.Ticks && DateTime.Now.Ticks <= validatedToken.ValidFrom.Ticks && principal.Identity.IsAuthenticated)
                    return true;
            }
            catch (Exception)
            {
                return false;
            }
            return false;
        }

        private TokenValidationParameters GetValidationParameters()
        {
            return new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["JWT:Issuer"],
                ValidAudience = _configuration["JWT:Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Key"]))
            };
        }
    }
}
