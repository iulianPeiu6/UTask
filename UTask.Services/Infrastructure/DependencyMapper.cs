using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using UTask.DataAccess.Context;
using UTask.Services.Users;

namespace UTask.Services.Infrastructure
{
    public class DependencyMapper
    {
        public static ServiceCollection AddDbContext(ServiceCollection services, string connectionString)
        {
            services.AddDbContext<UTaskContext>(options => options.UseSqlServer(connectionString));

            return services;
        }

        public static ServiceCollection MapDependencies(ServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();

            return services;
        }
    }
}
