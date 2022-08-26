using Core.Repository;
using MongoDbGenericRepository;
using TemplateService.Data.Models;

namespace TemplateService.Data.Repositories
{
    public class TemplateRepository : MongoRepository<TemplateEntity>, ITemplateRepository
    {
        public TemplateRepository(IMongoDbContext context) : base(context)
        {
        }

    }
}
