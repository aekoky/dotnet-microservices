using Formuler.Core.Repository;
using RenderingService.Data.Models;

namespace RenderingService.Data.Repositories
{
    public interface IDocumentRepository : IMongoRepository<DocumentEntity>
    {
    }
}
