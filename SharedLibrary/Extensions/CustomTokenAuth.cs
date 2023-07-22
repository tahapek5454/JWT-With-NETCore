using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using SharedLibrary.Configurations;
using SharedLibrary.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibrary.Extensions
{
    public static class CustomTokenAuth
    {
        public static void AddCustomTokenAuth(this IServiceCollection serviceCollection, CustomTokenOptions tokenOptions)
        {
            //Authentication operations
            serviceCollection.AddAuthentication(options =>
            {
                //set Schema : "Bearer"
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                // merge two schema each other (Authentication schema and jwt schema)
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                // validation parameters choose (valid payload)
                // for using token options parameters
                options.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = tokenOptions.Issuer,
                    ValidAudience = tokenOptions.Audiences[0], // enough 0 index for this api
                    IssuerSigningKey = SignService.GetSymmetricSecurityKey(tokenOptions.SecurityKey),

                    ValidateIssuerSigningKey = true,
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero // servers times diff 0

                };

            });
        }
    }
}
