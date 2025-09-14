using HRMService.API.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared.SharedKernel;
using System.Text;

namespace HRMService.API.Extentions
{
    public static class AuthExtension
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(o =>
            {
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidIssuer = config["Jwt:Issuer"],
                    ValidAudience = config["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"])),
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

            return services;
        }

        public static IServiceCollection AddCustomAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(config =>
            {
                config.AddPolicy("ViewUsersPolicy", policyBuilder =>
                {
                    policyBuilder.RequireClaim(Constants.CREDENTIAL_CLAIM, "R_User");
                });

                config.AddPolicy("HrPolicy", policyBuilder =>
                {
                    policyBuilder.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.AddCredentialRequirement("C_Department", "R_Department", "U_Department", "D_Department");
                    policyBuilder.AddCredentialRequirement("C_Employee", "R_Employee", "U_Employee", "D_Employee");
                });

                config.AddPolicy("ManagersAndHrPolicy", policyBuilder =>
                {
                    policyBuilder.RequireRole("Managers");
                    policyBuilder.RequireClaim(Constants.CREDENTIAL_CLAIM, "C_Department", "R_Department", "U_Department", "D_Department");
                    policyBuilder.RequireClaim(Constants.CREDENTIAL_CLAIM, "C_Employee", "R_Employee", "U_Employee", "D_Employee");
                });
            });

            return services;
        }
    }
}
