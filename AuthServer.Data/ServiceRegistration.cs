using AuthServer.Core.Models;
using AuthServer.Core.Repositories;
using AuthServer.Data.Context;
using AuthServer.Data.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuthServer.Data
{
    public static class ServiceRegistration
    {
        public static void AddDataService(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration["ConnectionStrings:PostgreSQL"]));

            serviceCollection.AddIdentity<UserApp, IdentityRole<int>>(options =>
            {
                options.User.RequireUniqueEmail= true;
                options.Password.RequiredLength = 3;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
            }).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();
            //TokenProvider for reset password operations
            serviceCollection.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

        }
    }
}
