using Formuler.Core.Repository;
using MongoDB.Driver;
using MongoDbGenericRepository;
using System.Collections.Generic;
using System.Threading.Tasks;
using TemplateService.Data.Models;

namespace TemplateService.Data.Repositories
{
    public class TemplateRepository : MongoRepository<TemplateEntity>, ITemplateRepository
    {
        public TemplateRepository(IMongoDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<TemplateEntity>> FilterTemplates(string keyword)
        {
            var filter = Builders<TemplateEntity>.Filter.Empty;
            if (!string.IsNullOrEmpty(keyword))
                filter = Builders<TemplateEntity>.Filter.StringIn(template => template.Label, keyword) |
                   Builders<TemplateEntity>.Filter.StringIn(template => template.Description, keyword) |
                   Builders<TemplateEntity>.Filter.StringIn(template => template.Details, keyword);

            return await GetAsync(filter).ConfigureAwait(false);
        }
    }
}
