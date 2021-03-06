using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    internal interface IMongoSet
    {
        Task InitializeAsync(CancellationToken cancellationToken = default);
        
        Task<AsyncDelegate> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public interface IMongoSet<TDocument> : IFindFluentSource<TDocument>
    {
        string Name { get; }

        IMongoCollection<TDocument> Collection { get; }

        MongoContext Context { get; }

        IFindFluent<TDocument> Find(FilterDefinition<TDocument> filter);

        void Add(TDocument document);

        void Remove(TDocument document);
    }
}