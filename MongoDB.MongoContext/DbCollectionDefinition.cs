using System;
using System.Collections.Generic;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    public sealed class DbCollectionDefinition<TDocument>
        where TDocument : IMongoAggregate<TDocument>
    {
        private readonly List<IndexDefinition<TDocument>> _indexDefinitions = new();

        private readonly MongoContext _context;

        public DbCollectionDefinition(
            MongoContext context,
            string name,
            PrimaryKeyFilterSelector<TDocument> primaryKeyFilterSelector)
        {
            Name = name;
            PrimaryKeyFilterSelector = primaryKeyFilterSelector;
            _context = context;
        }

        public string Name { get; }
        public PrimaryKeyFilterSelector<TDocument> PrimaryKeyFilterSelector { get; }

        public IReadOnlyList<IndexDefinition<TDocument>> IndexDefinitions => _indexDefinitions;

        public DbCollectionDefinition<TDocument> HasIndex(
            Func<IndexKeysDefinitionBuilder<TDocument>, IndexKeysDefinition<TDocument>> indexKeysSelector,
            Action<CreateIndexOptionsBuilder<TDocument>, CreateIndexDefinitionOptions<TDocument>>? configure = null)
        {
            var indexKeys = indexKeysSelector(Builders<TDocument>.IndexKeys);

            var mongoCollection = _context.Database.GetCollection<TDocument>(Name);
            
            var options = new CreateIndexDefinitionOptions<TDocument>();
            var optionsBuilder = new CreateIndexOptionsBuilder<TDocument>(mongoCollection);
            configure?.Invoke(optionsBuilder, options);

            var indexDefinition = new IndexDefinition<TDocument>(indexKeys, options);
            _indexDefinitions.Add(indexDefinition);

            return this;
        }

        public IDbCollection<TDocument> ToDbCollection()
        {
            return _context.GetCollection(this);
        }
    }
}