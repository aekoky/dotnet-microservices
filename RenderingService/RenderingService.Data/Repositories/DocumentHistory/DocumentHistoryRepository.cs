using Formuler.Core.Repository;
using MongoDbGenericRepository;
using RenderingService.Data.Models;

namespace RenderingService.Data.Repositories
{
    public class DocumentHistoryRepository : MongoRepository<DocumentHistoryEntity>, IDocumentHistoryRepository
    {
        public DocumentHistoryRepository(IMongoDbContext context) : base(context)
        {
        }
    }
}
