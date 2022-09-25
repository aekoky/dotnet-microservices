using Formuler.Core.Repository;
using MongoDbGenericRepository;
using RenderingService.Data.Models;

namespace RenderingService.Data.Repositories
{
    public class DocumentRepository : MongoRepository<DocumentEntity>, IDocumentRepository
    {
        public DocumentRepository(IMongoDbContext context) : base(context)
        {
        }
    }
}
