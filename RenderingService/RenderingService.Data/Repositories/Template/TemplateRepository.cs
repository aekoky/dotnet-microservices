using Formuler.Core.Repository;
using MongoDbGenericRepository;
using RenderingService.Data.Models;

namespace RenderingService.Data.Repositories
{
    public class TemplateRepository : MongoRepository<TemplateEntity>, ITemplateRepository
    {
        public TemplateRepository(IMongoDbContext context) : base(context)
        {
        }
    }
}
