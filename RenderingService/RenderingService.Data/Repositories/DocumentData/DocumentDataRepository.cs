using Formuler.Core.Repository;
using MongoDbGenericRepository;
using RenderingService.Data.Models;

namespace RenderingService.Data.Repositories
{
    public class DocumentDataRepository : MongoRepository<DocumentDataEntity>, IDocumentDataRepository
    {
        public DocumentDataRepository(IMongoDbContext context) : base(context)
        {
        }
    }
}
