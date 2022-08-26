using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDbGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Repository
{
    public interface IMongoRepository<T> : IBaseMongoRepository<Guid> where T : MongoEntity
    {
        IMongoDbContext Context { get; }
        IMongoQueryable<T> Table { get; }
        int Count { get; }
        T Find(Guid Id, FindOptions options = null);
        T Find(FilterDefinition<T> filterDefinition, FindOptions options = null);
        T Find(Expression<Func<T, bool>> filter, FindOptions options = null);
        IEnumerable<T> Get(Expression<Func<T, bool>> filter, FindOptions options = null);
        IEnumerable<T> Get(FilterDefinition<T> filterDefinition, FindOptions options = null);
    }

}
