using Formuler.Core.Repository;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateService.Data.Models;

namespace TemplateService.Data.Repositories
{
    public interface ITemplateRepository : IMongoRepository<TemplateEntity>
    {
        Task<IEnumerable<TemplateEntity>> FilterTemplates(string keyword);
    }
}
