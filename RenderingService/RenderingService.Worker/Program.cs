using Microsoft.Extensions.Hosting;

namespace RenderingService.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((context, services) => new Startup(context.Configuration, context.HostingEnvironment).ConfigureServices(services));
    }
}
