
using Shared.SharedAuth.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;


namespace AuthService.Applications.Services
{
    public interface IJwtManagerService
    {
        IUTokens GenerateTokens(SignInUser user);
        bool IsTokenValid(string token);
    }
}
