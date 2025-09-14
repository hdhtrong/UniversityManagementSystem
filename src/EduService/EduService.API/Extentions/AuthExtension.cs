using EduService.API.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Shared.SharedKernel;
using System.Text;

namespace EduService.API.Extentions
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

                config.AddPolicy("OaaPolicy", policyBuilder =>
                {
                    policyBuilder.AuthenticationSchemes.Add(JwtBearerDefaults.AuthenticationScheme);
                    policyBuilder.RequireAuthenticatedUser();
                    policyBuilder.AddCredentialRequirement("C_Course", "R_Course", "U_Course", "D_Course");
                    policyBuilder.AddCredentialRequirement("C_Enrolment", "R_Enrolment", "U_Enrolment", "D_Enrolment");
                });

                config.AddPolicy("ManagersAndOaaPolicy", policyBuilder =>
                {
                    policyBuilder.RequireRole("Managers");
                    policyBuilder.RequireClaim(Constants.CREDENTIAL_CLAIM, "C_Course", "R_Course", "U_Course", "D_Course");
                    policyBuilder.RequireClaim(Constants.CREDENTIAL_CLAIM, "C_Enrolment", "R_Enrolment", "U_Enrolment", "D_Enrolment");
                });
            });

            return services;
        }
    }
}
