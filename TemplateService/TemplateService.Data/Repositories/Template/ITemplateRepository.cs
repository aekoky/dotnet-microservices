using Core.Repository;
using TemplateService.Data.Models;

namespace TemplateService.Data.Repositories
{
    public interface ITemplateRepository : IMongoRepository<TemplateEntity>
    {
    }
}
