using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    internal sealed class DbCollectionDefinitionProcessor<TDocument>
        where TDocument : IMongoAggregate<TDocument>
    {
        private readonly IMongoCollection<TDocument> _collection;

        public DbCollectionDefinitionProcessor(IMongoCollection<TDocument> collection)
        {
            _collection = collection;
        }

        public async Task InitializeIndices(
            IEnumerable<IndexDefinition<TDocument>> indexDefinitions,
            CancellationToken cancellationToken = default)
        {
            var indexManager = _collection.Indexes;
            
            foreach (var indexDefinition in indexDefinitions)
            {
                try
                {
                    await indexManager.CreateOneAsync(indexDefinition.Model, cancellationToken: cancellationToken);
                }
                catch (MongoCommandException exception)
                {
                    if (!await TryHandleIndexCommandExceptionAsync(exception, indexDefinition, cancellationToken))
                    {
                        throw;
                    }
                }
            }
        }

        private Task<bool> TryHandleIndexCommandExceptionAsync(
            MongoCommandException exception,
            IndexDefinition<TDocument> indexDefinition,
            CancellationToken cancellationToken)
        {
            var command = exception.Command;
            var code = exception.Code;

            return code switch
            {
                MongoCommandErrorCodes.IndexOptionsConflict => TryHandleIndexOptionsConflictAsync(command, indexDefinition, cancellationToken),
                _ => Task.FromResult(false)
            };
        }

        private async Task<bool> TryHandleIndexOptionsConflictAsync(
            BsonDocument command,
            IndexDefinition<TDocument> indexDefinition,
            CancellationToken cancellationToken)
        {
            var indexManager = _collection.Indexes;

            if (indexDefinition.IndexOptionsConflictResolution == IndexOptionsConflictResolution.Drop)
            {
                var indexName = (string) command["createIndexes"];
                await indexManager.DropOneAsync(indexName, cancellationToken);

                // Retry
                await indexManager.CreateOneAsync(indexDefinition.Model, cancellationToken: cancellationToken);

                return true;
            }

            return false;
        }
    }
}