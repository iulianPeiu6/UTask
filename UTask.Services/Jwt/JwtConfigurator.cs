using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using UTask.DataAccess;
using UTask.Models;
using UTask.Services.Cryptography;

namespace UTask.Services.Jwt
{
    public class JwtConfigurator
    {
        public static IServiceCollection Configure(IServiceCollection services, string tokenKey)
        {
            var key = Encoding.ASCII.GetBytes(tokenKey);
            services.AddAuthentication(authenticationOptions =>
            {
                authenticationOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                authenticationOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(jwtBearerOptions =>
            {
                jwtBearerOptions.RequireHttpsMetadata = false;
                jwtBearerOptions.SaveToken = true;
                jwtBearerOptions.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });

            services
                .AddSingleton<IJwtAuthenticator>(new JwtAuthenticator(
                    tokenKey,
                    services.BuildServiceProvider().GetRequiredService<IRepository<User>>(),
                    services.BuildServiceProvider().GetRequiredService<ILogger<JwtAuthenticator>>(),
                    services.BuildServiceProvider().GetRequiredService<ICryptographyService>()));

            return services;
        }
    }
}
