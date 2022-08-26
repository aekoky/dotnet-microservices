using Microsoft.Extensions.DependencyInjection;
using MongoDbGenericRepository;
using UserService.Data.Repositories;

namespace UserService.Data
{
    public static class DependencyInjection
    {
        public static void AddUserServiceData(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IMongoDbContext, UserServiceDbContext>();
            serviceCollection.AddTransient<IUserRepository, UserRepository>();
            serviceCollection.AddTransient<IAccountRepository, AccountRepository>();
        }
    }
}
