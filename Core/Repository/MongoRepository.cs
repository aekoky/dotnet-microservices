using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MongoDbGenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Core.Repository
{
    public class MongoRepository<T> : BaseMongoRepository<Guid>, IMongoRepository<T> where T : MongoEntity
    {
        private readonly IMongoDbContext _context;
        private IMongoCollection<T> _document;

        public MongoRepository(IMongoDbContext context):base(context)
        {
            _context = context;
        }

        #region Properties
        protected IMongoCollection<T> Document
        {
            get
            {
                if (_document == null)
                    _document = _context.GetCollection<T>();

                return _document;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public IMongoDbContext Context
        {
            get
            {
                return _context;
            }
        }

        /// <summary>
        /// Get type of IMongoQueryable document
        /// If you want an IEnumerable list make 'Table.ToList()'
        /// </summary>
        public IMongoQueryable<T> Table
        {
            get
            {
                return Document.AsQueryable();
            }
        }

        /// <summary>
        /// Get all document count
        /// </summary>
        public int Count
        {
            get
            {
                return Table.Count();
            }
        }
        #endregion

        #region Query
        /// <summary>
        /// Get single document by expression
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public T Find(FilterDefinition<T> filterDefinition, FindOptions options = null)
        {
            return Document.Find(filterDefinition, options).FirstOrDefault();
        }

        /// <summary>
        /// Get single document by expression
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public T Find(Expression<Func<T, bool>> filter, FindOptions options = null)
        {
            return Document.Find(filter, options).FirstOrDefault();
        }

        /// <summary>
        /// Get single document by object id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public T Find(Guid Id, FindOptions options = null)
        {
            return Document.Find(x => x.Id == Id, options).FirstOrDefault();
        }

        /// <summary>
        /// Get document list by expression
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<T> Get(Expression<Func<T, bool>> filter, FindOptions options = null)
        {
            return Document.Find(filter, options).ToList();
        }

        /// <summary>
        /// Get document list by expression
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public IEnumerable<T> Get(FilterDefinition<T> filterDefinition, FindOptions options = null)
        {
            return Document.Find(filterDefinition, options).ToList();
        }
        #endregion
    }
}
