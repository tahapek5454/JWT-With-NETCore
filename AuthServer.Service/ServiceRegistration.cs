using AuthServer.Core.Services;
using AuthServer.Core.UnitOfWork;
using AuthServer.Data.UnitOfWork;
using AuthServer.Service.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Service
{
    public static class ServiceRegistration
    {
        public static void AddServicesService(this IServiceCollection serviceCollection)
        {
            //DI Register
            serviceCollection.AddScoped<IAuthenticationService, AuthenticationService>();
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<ITokenService, TokenService>();
            serviceCollection.AddScoped(typeof(IGenericService<>), typeof(GenericService<,>));
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
        }
    }
}
