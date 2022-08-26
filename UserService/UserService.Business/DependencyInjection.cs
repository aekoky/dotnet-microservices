using Microsoft.Extensions.DependencyInjection;
using UserService.Business.Services;

namespace UserService.Business
{
    public static class DependencyInjection
    {
        public static void AddUserServiceBusiness(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAccountService, AccountService>();
            serviceCollection.AddTransient<IUserService, Services.UserService>();
        }
    }
}
