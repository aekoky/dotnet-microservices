using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Models;
using System;
using System.Runtime.Serialization;

namespace Formuler.Core.Repository
{
    [DataContract]
    [Serializable]
    [BsonIgnoreExtraElements(Inherited = true)]
    public abstract class MongoEntity : Document
    {
        public DateTime UpdatedDate { get; set; }
    }
}
