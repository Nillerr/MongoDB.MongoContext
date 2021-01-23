using System;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    /// <summary>
    /// Like DbContext from Entity Framework, but with nowhere near the same features, and it's for MongoDB. Provides a
    /// context for performing operations on a MongoDB database, enabling transactions without manually passing
    /// <see cref="IClientSessionHandle"/> objects around, and provides access to every collection all in one place.
    /// </summary>
    /// <remarks>
    /// This class is designed to be used as a Scoped service.
    /// </remarks>
    /// <example>
    /// <code>
    /// public class AppContext : MongoContext
    /// {
    ///     public AppContext(IMongoDatabase database) : base(database) { }
    ///     
    ///     public IMongoCollection&lt;Product&gt; Products => Collection&lt;Product&gt;("products");
    ///     public IMongoCollection&lt;Order&gt; Orders => Collection&lt;Order&gt;("orders");
    /// }
    /// </code>
    /// </example>
    public abstract class MongoContext :
        IDisposable
#if NETSTANDARD2_1
        , IAsyncDisposable
#endif
    {
        private readonly IClientSessionHandle _session;
        
#if NETSTANDARD2_1
        private MongoTransaction? _currentTransaction;
#else
        private MongoTransaction _currentTransaction;
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoContext"/> class. Override this constructor in a derived
        /// type to pass the <see cref="MongoContextOptions"/> to the context.
        /// </summary>
        /// <param name="options">The context options.</param>
        protected MongoContext(MongoContextOptions options)
        {
            Database = options.Database;
            
            // `StartSession` will block while determining whether the MongoDB server supports sessions, but that query
            // will only be performed once, being saved in memory for the next invocation. Therefore it is both simpler
            // and alright to just use the blocking version here.
            _session = options.Database.Client.StartSession(options.SessionOptions);
        }

        /// <summary>
        /// <para>
        /// Gets the current <see cref="IMongoTransaction"/> being used by the context, or <see langword="null"/> if no
        /// transaction is in use.
        /// </para>
        /// <para>
        /// This property will be <see langword="null"/> unless <see cref="StartTransaction"/> has been called. No
        /// attempt is made to obtain a transaction from the current connection or similar.
        /// </para>
        /// </summary>
#if NETSTANDARD2_1
        public IMongoTransaction? CurrentTransaction => _currentTransaction;
#else
        public IMongoTransaction CurrentTransaction => _currentTransaction;
#endif

        /// <summary>
        /// Starts a new transaction.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="cancellationToken"></param>
        /// <returns>An object for interacting with the started transaction.</returns>
        /// <exception cref="InvalidOperationException">A transaction is already in progress in this context.</exception>
        public IMongoTransaction StartTransaction(
#if NETSTANDARD2_1
            TransactionOptions? options = null,
#else
            TransactionOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            _session.StartTransaction(options);
            
            var mongoTransaction = new MongoTransaction(_session);
            _currentTransaction = mongoTransaction;

            return mongoTransaction;
        }

        /// <summary>
        /// Gets the underlying <see cref="IMongoClient"/>.
        /// </summary>
        public IMongoClient Client => Database.Client;
        
        /// <summary>
        /// Gets the underlying <see cref="IMongoDatabase"/>.
        /// </summary>
        public IMongoDatabase Database { get; }
        
        /// <summary>
        /// Gets the collection with the specified <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the collection.</param>
        /// <typeparam name="TDocument">The type of document.</typeparam>
        /// <returns>
        /// An implementation of a collection. The implementation will be
        /// <see cref="SessionWrappedMongoCollection{TDocument}"/> while the context is in a transaction; otherwise the
        /// return value of <see cref="IMongoDatabase.GetCollection{TDocument}"/>.
        /// </returns>
        protected virtual IMongoCollection<TDocument> Collection<TDocument>(string name)
        {
            var collection = Database.GetCollection<TDocument>(name);
            return new SessionWrappedMongoCollection<TDocument>(collection, _session);
        }

#if NETSTANDARD2_1
        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            var transaction = _currentTransaction;
            if (transaction != null)
            {
                await transaction.DisposeAsync().ConfigureAwait(false);
            }
            
            _session.Dispose();
        }
#endif

        /// <inheritdoc />
        public void Dispose()
        {
            _currentTransaction?.Dispose();
            _session.Dispose();
        }
    }
}