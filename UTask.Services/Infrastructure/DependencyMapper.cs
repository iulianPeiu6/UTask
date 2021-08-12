using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UTask.DataAccess;
using UTask.DataAccess.Context;
using UTask.Services.Cryptography;
using UTask.Services.Users;

namespace UTask.Services.Infrastructure
{
    public class DependencyMapper
    {
        public static IServiceCollection AddDbContext(IServiceCollection services, string connectionString)
        {
            services.AddDbContext<UTaskContext>(options => options.UseSqlServer(connectionString));

            return services;
        }

        public static IServiceCollection MapDependencies(IServiceCollection services)
        {
            services
                .AddScoped(typeof(IRepository<>), typeof(Repository<>))
                .AddScoped<IUnitOfWork, UnitOfWork>()
                .AddScoped<IUserService, UserService>()
                .AddScoped<ICryptographyService, CryptographyService>();

            return services;
        }
    }
}
