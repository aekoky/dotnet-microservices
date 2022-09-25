using DinkToPdf;
using DinkToPdf.Contracts;
using Formuler.Core.ApiFacade.FileService;
using Formuler.Core.MessageBroker;
using Formuler.Core.Repository;
using Formuler.Shared.ApiFacade;
using Formuler.Shared.ApiFacade.ApiSettings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RenderingService.Data;
using RenderingService.Worker.Consumers;

namespace RenderingService.Worker
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            CurrentEnvironment = webHostEnvironment;
        }

        public IConfiguration Configuration { get; }
        private IHostEnvironment CurrentEnvironment { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<FileServiceFacadeSettings>(Configuration.GetSection(nameof(FileServiceFacadeSettings)));
            services.Configure<MongoDbSettings>(Configuration.GetSection(nameof(MongoDbSettings)));

            services.AddSingleton(typeof(IConverter), new SynchronizedConverter(new PdfTools()));

            services.AddRabbitMQ(Configuration, typeof(RenderingCommandConsumer), typeof(TemplateCreatedEventConsumer), typeof(TemplateDeletedEventConsumer));

            services.AddHostedService<Worker>();
            services.AddRestClient(Configuration, CurrentEnvironment.IsDevelopment());
            services.AddRenderingServiceeData();
            services.AddSingleton<IFileServiceApiFacade, FileServiceApiFacade>();
        }
    }
}
