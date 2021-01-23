using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    /// <summary>
    /// Wraps calls to an underlying <see cref="IMongoCollection{TDocument}"/> with calls to the overloads accepting
    /// <see cref="IClientSessionHandle"/>.
    /// </summary>
    /// <typeparam name="TDocument">The type of document.</typeparam>
    internal sealed class SessionWrappedMongoCollection<TDocument> : IMongoCollection<TDocument>
    {
        private readonly IMongoCollection<TDocument> _collection;
        private readonly IClientSessionHandle _session;

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionWrappedMongoCollection{TDocument}"/> class.
        /// </summary>
        /// <param name="collection">The underlying collection.</param>
        /// <param name="session">The session.</param>
        public SessionWrappedMongoCollection(IMongoCollection<TDocument> collection, IClientSessionHandle session)
        {
            _collection = collection;
            _session = session;
        }

#pragma warning disable 618
        /// <inheritdoc />
        public IAsyncCursor<TResult> Aggregate<TResult>(
            PipelineDefinition<TDocument, TResult> pipeline,
#if NETSTANDARD2_1
            AggregateOptions? options = null,
#else
            AggregateOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.Aggregate(_session, pipeline, options, cancellationToken);
        }

        /// <inheritdoc />
        public IAsyncCursor<TResult> Aggregate<TResult>(
            IClientSessionHandle session,
            PipelineDefinition<TDocument, TResult> pipeline,
#if NETSTANDARD2_1
            AggregateOptions? options = null,
#else
            AggregateOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.Aggregate(session, pipeline, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(
            PipelineDefinition<TDocument, TResult> pipeline,
#if NETSTANDARD2_1
            AggregateOptions? options = null,
#else
            AggregateOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.AggregateAsync(_session, pipeline, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<IAsyncCursor<TResult>> AggregateAsync<TResult>(
            IClientSessionHandle session,
            PipelineDefinition<TDocument, TResult> pipeline,
#if NETSTANDARD2_1
            AggregateOptions? options = null,
#else
            AggregateOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.AggregateAsync(session, pipeline, options, cancellationToken);
        }

        /// <inheritdoc />
        public void AggregateToCollection<TResult>(
            PipelineDefinition<TDocument, TResult> pipeline,
#if NETSTANDARD2_1
            AggregateOptions? options = null,
#else
            AggregateOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            _collection.AggregateToCollection(_session, pipeline, options, cancellationToken);
        }

        /// <inheritdoc />
        public void AggregateToCollection<TResult>(
            IClientSessionHandle session,
            PipelineDefinition<TDocument, TResult> pipeline,
#if NETSTANDARD2_1
            AggregateOptions? options = null,
#else
            AggregateOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            _collection.AggregateToCollection(session, pipeline, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task AggregateToCollectionAsync<TResult>(
            PipelineDefinition<TDocument, TResult> pipeline,
#if NETSTANDARD2_1
            AggregateOptions? options = null,
#else
            AggregateOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.AggregateToCollectionAsync(_session, pipeline, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task AggregateToCollectionAsync<TResult>(
            IClientSessionHandle session,
            PipelineDefinition<TDocument, TResult> pipeline,
#if NETSTANDARD2_1
            AggregateOptions? options = null,
#else
            AggregateOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.AggregateToCollectionAsync(session, pipeline, options, cancellationToken);
        }

        /// <inheritdoc />
        public BulkWriteResult<TDocument> BulkWrite(
            IEnumerable<WriteModel<TDocument>> requests,
#if NETSTANDARD2_1
            BulkWriteOptions? options = null,
#else
            BulkWriteOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.BulkWrite(_session, requests, options, cancellationToken);
        }

        /// <inheritdoc />
        public BulkWriteResult<TDocument> BulkWrite(
            IClientSessionHandle session,
            IEnumerable<WriteModel<TDocument>> requests,
#if NETSTANDARD2_1
            BulkWriteOptions? options = null,
#else
            BulkWriteOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.BulkWrite(session, requests, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<BulkWriteResult<TDocument>> BulkWriteAsync(
            IEnumerable<WriteModel<TDocument>> requests,
#if NETSTANDARD2_1
            BulkWriteOptions? options = null,
#else
            BulkWriteOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.BulkWriteAsync(_session, requests, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<BulkWriteResult<TDocument>> BulkWriteAsync(
            IClientSessionHandle session,
            IEnumerable<WriteModel<TDocument>> requests,
#if NETSTANDARD2_1
            BulkWriteOptions? options = null,
#else
            BulkWriteOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.BulkWriteAsync(session, requests, options, cancellationToken);
        }

        /// <inheritdoc />
        public long Count(
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            CountOptions? options = null,
#else
            CountOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.Count(_session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public long Count(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            CountOptions? options = null,
#else
            CountOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.Count(session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<long> CountAsync(
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            CountOptions? options = null,
#else
            CountOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.CountAsync(_session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<long> CountAsync(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            CountOptions? options = null,
#else
            CountOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.CountAsync(session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public long CountDocuments(
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            CountOptions? options = null,
#else
            CountOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.CountDocuments(_session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public long CountDocuments(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            CountOptions? options = null,
#else
            CountOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.CountDocuments(session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<long> CountDocumentsAsync(
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            CountOptions? options = null,
#else
            CountOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.CountDocumentsAsync(_session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<long> CountDocumentsAsync(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            CountOptions? options = null,
#else
            CountOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.CountDocumentsAsync(session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public DeleteResult DeleteMany(FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default)
        {
            return _collection.DeleteMany(_session, filter, null, cancellationToken);
        }

        /// <inheritdoc />
        public DeleteResult DeleteMany(
            FilterDefinition<TDocument> filter,
            DeleteOptions options,
            CancellationToken cancellationToken = default
        )
        {
            return _collection.DeleteMany(_session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public DeleteResult DeleteMany(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            DeleteOptions? options = null,
#else
            DeleteOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.DeleteMany(session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<DeleteResult> DeleteManyAsync(FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default)
        {
            return _collection.DeleteManyAsync(_session, filter, null, cancellationToken);
        }

        /// <inheritdoc />
        public Task<DeleteResult> DeleteManyAsync(
            FilterDefinition<TDocument> filter,
            DeleteOptions options,
            CancellationToken cancellationToken = default
        )
        {
            return _collection.DeleteManyAsync(_session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<DeleteResult> DeleteManyAsync(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            DeleteOptions? options = null,
#else
            DeleteOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.DeleteManyAsync(session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public DeleteResult DeleteOne(FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default)
        {
            return _collection.DeleteOne(_session, filter, null, cancellationToken);
        }

        /// <inheritdoc />
        public DeleteResult DeleteOne(
            FilterDefinition<TDocument> filter,
            DeleteOptions options,
            CancellationToken cancellationToken = default
        )
        {
            return _collection.DeleteOne(_session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public DeleteResult DeleteOne(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            DeleteOptions? options = null,
#else
            DeleteOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.DeleteOne(session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<DeleteResult> DeleteOneAsync(FilterDefinition<TDocument> filter, CancellationToken cancellationToken = default)
        {
            return _collection.DeleteOneAsync(_session, filter, null, cancellationToken);
        }

        /// <inheritdoc />
        public Task<DeleteResult> DeleteOneAsync(
            FilterDefinition<TDocument> filter,
            DeleteOptions options,
            CancellationToken cancellationToken = default
        )
        {
            return _collection.DeleteOneAsync(_session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<DeleteResult> DeleteOneAsync(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            DeleteOptions? options = null,
#else
            DeleteOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.DeleteOneAsync(session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public IAsyncCursor<TField> Distinct<TField>(
            FieldDefinition<TDocument, TField> field,
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            DistinctOptions? options = null,
#else
            DistinctOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.Distinct(_session, field, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public IAsyncCursor<TField> Distinct<TField>(
            IClientSessionHandle session,
            FieldDefinition<TDocument, TField> field,
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            DistinctOptions? options = null,
#else
            DistinctOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.Distinct(session, field, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<IAsyncCursor<TField>> DistinctAsync<TField>(
            FieldDefinition<TDocument, TField> field,
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            DistinctOptions? options = null,
#else
            DistinctOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.DistinctAsync(_session, field, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<IAsyncCursor<TField>> DistinctAsync<TField>(
            IClientSessionHandle session,
            FieldDefinition<TDocument, TField> field,
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            DistinctOptions? options = null,
#else
            DistinctOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.DistinctAsync(session, field, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public long EstimatedDocumentCount(
#if NETSTANDARD2_1
            EstimatedDocumentCountOptions? options = null,
#else
            EstimatedDocumentCountOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.EstimatedDocumentCount(options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<long> EstimatedDocumentCountAsync(
#if NETSTANDARD2_1
            EstimatedDocumentCountOptions? options = null,
#else
            EstimatedDocumentCountOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.EstimatedDocumentCountAsync(options, cancellationToken);
        }

        /// <inheritdoc />
        public IAsyncCursor<TProjection> FindSync<TProjection>(
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            FindOptions<TDocument, TProjection>? options = null,
#else
            FindOptions<TDocument, TProjection> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.FindSync(_session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public IAsyncCursor<TProjection> FindSync<TProjection>(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            FindOptions<TDocument, TProjection>? options = null,
#else
            FindOptions<TDocument, TProjection> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.FindSync(session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<IAsyncCursor<TProjection>> FindAsync<TProjection>(
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            FindOptions<TDocument, TProjection>? options = null,
#else
            FindOptions<TDocument, TProjection> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.FindAsync(_session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<IAsyncCursor<TProjection>> FindAsync<TProjection>(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            FindOptions<TDocument, TProjection>? options = null,
#else
            FindOptions<TDocument, TProjection> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.FindAsync(session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public TProjection FindOneAndDelete<TProjection>(
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            FindOneAndDeleteOptions<TDocument, TProjection>? options = null,
#else
            FindOneAndDeleteOptions<TDocument, TProjection> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.FindOneAndDelete(_session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public TProjection FindOneAndDelete<TProjection>(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            FindOneAndDeleteOptions<TDocument, TProjection>? options = null,
#else
            FindOneAndDeleteOptions<TDocument, TProjection> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.FindOneAndDelete(session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TProjection> FindOneAndDeleteAsync<TProjection>(
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            FindOneAndDeleteOptions<TDocument, TProjection>? options = null,
#else
            FindOneAndDeleteOptions<TDocument, TProjection> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.FindOneAndDeleteAsync(_session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TProjection> FindOneAndDeleteAsync<TProjection>(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
#if NETSTANDARD2_1
            FindOneAndDeleteOptions<TDocument, TProjection>? options = null,
#else
            FindOneAndDeleteOptions<TDocument, TProjection> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.FindOneAndDeleteAsync(session, filter, options, cancellationToken);
        }

        /// <inheritdoc />
        public TProjection FindOneAndReplace<TProjection>(
            FilterDefinition<TDocument> filter,
            TDocument replacement,
#if NETSTANDARD2_1
            FindOneAndReplaceOptions<TDocument, TProjection>? options = null,
#else
            FindOneAndReplaceOptions<TDocument, TProjection> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.FindOneAndReplace(_session, filter, replacement, options, cancellationToken);
        }

        /// <inheritdoc />
        public TProjection FindOneAndReplace<TProjection>(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
            TDocument replacement,
#if NETSTANDARD2_1
            FindOneAndReplaceOptions<TDocument, TProjection>? options = null,
#else
            FindOneAndReplaceOptions<TDocument, TProjection> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.FindOneAndReplace(session, filter, replacement, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TProjection> FindOneAndReplaceAsync<TProjection>(
            FilterDefinition<TDocument> filter,
            TDocument replacement,
#if NETSTANDARD2_1
            FindOneAndReplaceOptions<TDocument, TProjection>? options = null,
#else
            FindOneAndReplaceOptions<TDocument, TProjection> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.FindOneAndReplaceAsync(_session, filter, replacement, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TProjection> FindOneAndReplaceAsync<TProjection>(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
            TDocument replacement,
#if NETSTANDARD2_1
            FindOneAndReplaceOptions<TDocument, TProjection>? options = null,
#else
            FindOneAndReplaceOptions<TDocument, TProjection> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.FindOneAndReplaceAsync(session, filter, replacement, options, cancellationToken);
        }

        /// <inheritdoc />
        public TProjection FindOneAndUpdate<TProjection>(
            FilterDefinition<TDocument> filter,
            UpdateDefinition<TDocument> update,
#if NETSTANDARD2_1
            FindOneAndUpdateOptions<TDocument, TProjection>? options = null,
#else
            FindOneAndUpdateOptions<TDocument, TProjection> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.FindOneAndUpdate(_session, filter, update, options, cancellationToken);
        }

        /// <inheritdoc />
        public TProjection FindOneAndUpdate<TProjection>(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
            UpdateDefinition<TDocument> update,
#if NETSTANDARD2_1
            FindOneAndUpdateOptions<TDocument, TProjection>? options = null,
#else
            FindOneAndUpdateOptions<TDocument, TProjection> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.FindOneAndUpdate(session, filter, update, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TProjection> FindOneAndUpdateAsync<TProjection>(
            FilterDefinition<TDocument> filter,
            UpdateDefinition<TDocument> update,
#if NETSTANDARD2_1
            FindOneAndUpdateOptions<TDocument, TProjection>? options = null,
#else
            FindOneAndUpdateOptions<TDocument, TProjection> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.FindOneAndUpdateAsync(_session, filter, update, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<TProjection> FindOneAndUpdateAsync<TProjection>(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
            UpdateDefinition<TDocument> update,
#if NETSTANDARD2_1
            FindOneAndUpdateOptions<TDocument, TProjection>? options = null,
#else
            FindOneAndUpdateOptions<TDocument, TProjection> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.FindOneAndUpdateAsync(session, filter, update, options, cancellationToken);
        }

        /// <inheritdoc />
        public void InsertOne(
            TDocument document,
#if NETSTANDARD2_1
            InsertOneOptions? options = null,
#else
            InsertOneOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            _collection.InsertOne(_session, document, options, cancellationToken);
        }

        /// <inheritdoc />
        public void InsertOne(
            IClientSessionHandle session,
            TDocument document,
#if NETSTANDARD2_1
            InsertOneOptions? options = null,
#else
            InsertOneOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            _collection.InsertOne(session, document, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task InsertOneAsync(TDocument document, CancellationToken cancellationToken)
        {
            return _collection.InsertOneAsync(_session, document, null, cancellationToken);
        }

        /// <inheritdoc />
        public Task InsertOneAsync(
            TDocument document,
#if NETSTANDARD2_1
            InsertOneOptions? options = null,
#else
            InsertOneOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.InsertOneAsync(_session, document, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task InsertOneAsync(
            IClientSessionHandle session,
            TDocument document,
#if NETSTANDARD2_1
            InsertOneOptions? options = null,
#else
            InsertOneOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.InsertOneAsync(session, document, options, cancellationToken);
        }

        /// <inheritdoc />
        public void InsertMany(
            IEnumerable<TDocument> documents,
#if NETSTANDARD2_1
            InsertManyOptions? options = null,
#else
            InsertManyOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            _collection.InsertMany(_session, documents, options, cancellationToken);
        }

        /// <inheritdoc />
        public void InsertMany(
            IClientSessionHandle session,
            IEnumerable<TDocument> documents,
#if NETSTANDARD2_1
            InsertManyOptions? options = null,
#else
            InsertManyOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            _collection.InsertMany(session, documents, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task InsertManyAsync(
            IEnumerable<TDocument> documents,
#if NETSTANDARD2_1
            InsertManyOptions? options = null,
#else
            InsertManyOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.InsertManyAsync(_session, documents, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task InsertManyAsync(
            IClientSessionHandle session,
            IEnumerable<TDocument> documents,
#if NETSTANDARD2_1
            InsertManyOptions? options = null,
#else
            InsertManyOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.InsertManyAsync(session, documents, options, cancellationToken);
        }

        /// <inheritdoc />
        public IAsyncCursor<TResult> MapReduce<TResult>(
            BsonJavaScript map,
            BsonJavaScript reduce,
#if NETSTANDARD2_1
            MapReduceOptions<TDocument, TResult>? options = null,
#else
            MapReduceOptions<TDocument, TResult> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.MapReduce(_session, map, reduce, options, cancellationToken);
        }

        /// <inheritdoc />
        public IAsyncCursor<TResult> MapReduce<TResult>(
            IClientSessionHandle session,
            BsonJavaScript map,
            BsonJavaScript reduce,
#if NETSTANDARD2_1
            MapReduceOptions<TDocument, TResult>? options = null,
#else
            MapReduceOptions<TDocument, TResult> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.MapReduce(session, map, reduce, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<IAsyncCursor<TResult>> MapReduceAsync<TResult>(
            BsonJavaScript map,
            BsonJavaScript reduce,
#if NETSTANDARD2_1
            MapReduceOptions<TDocument, TResult>? options = null,
#else
            MapReduceOptions<TDocument, TResult> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.MapReduceAsync(_session, map, reduce, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<IAsyncCursor<TResult>> MapReduceAsync<TResult>(
            IClientSessionHandle session,
            BsonJavaScript map,
            BsonJavaScript reduce,
#if NETSTANDARD2_1
            MapReduceOptions<TDocument, TResult>? options = null,
#else
            MapReduceOptions<TDocument, TResult> options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.MapReduceAsync(session, map, reduce, options, cancellationToken);
        }

        /// <inheritdoc />
        public IFilteredMongoCollection<TDerivedDocument> OfType<TDerivedDocument>() where TDerivedDocument : TDocument
        {
            return _collection.OfType<TDerivedDocument>();
        }

        /// <inheritdoc />
        public ReplaceOneResult ReplaceOne(
            FilterDefinition<TDocument> filter,
            TDocument replacement,
#if NETSTANDARD2_1
            ReplaceOptions? options = null,
#else
            ReplaceOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.ReplaceOne(_session, filter, replacement, options, cancellationToken);
        }

        /// <inheritdoc />
        public ReplaceOneResult ReplaceOne(
            FilterDefinition<TDocument> filter,
            TDocument replacement,
            UpdateOptions options,
            CancellationToken cancellationToken = default
        )
        {
            return _collection.ReplaceOne(_session, filter, replacement, options, cancellationToken);
        }

        /// <inheritdoc />
        public ReplaceOneResult ReplaceOne(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
            TDocument replacement,
#if NETSTANDARD2_1
            ReplaceOptions? options = null,
#else
            ReplaceOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.ReplaceOne(session, filter, replacement, options, cancellationToken);
        }

        /// <inheritdoc />
        public ReplaceOneResult ReplaceOne(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
            TDocument replacement,
            UpdateOptions options,
            CancellationToken cancellationToken = default
        )
        {
            return _collection.ReplaceOne(session, filter, replacement, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ReplaceOneResult> ReplaceOneAsync(
            FilterDefinition<TDocument> filter,
            TDocument replacement,
#if NETSTANDARD2_1
            ReplaceOptions? options = null,
#else
            ReplaceOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.ReplaceOneAsync(_session, filter, replacement, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ReplaceOneResult> ReplaceOneAsync(
            FilterDefinition<TDocument> filter,
            TDocument replacement,
            UpdateOptions options,
            CancellationToken cancellationToken = default
        )
        {
            return _collection.ReplaceOneAsync(_session, filter, replacement, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ReplaceOneResult> ReplaceOneAsync(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
            TDocument replacement,
#if NETSTANDARD2_1
            ReplaceOptions? options = null,
#else
            ReplaceOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.ReplaceOneAsync(session, filter, replacement, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<ReplaceOneResult> ReplaceOneAsync(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
            TDocument replacement,
            UpdateOptions options,
            CancellationToken cancellationToken = default
        )
        {
            return _collection.ReplaceOneAsync(session, filter, replacement, options, cancellationToken);
        }

        /// <inheritdoc />
        public UpdateResult UpdateMany(
            FilterDefinition<TDocument> filter,
            UpdateDefinition<TDocument> update,
#if NETSTANDARD2_1
            UpdateOptions? options = null,
#else
            UpdateOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.UpdateMany(_session, filter, update, options, cancellationToken);
        }

        /// <inheritdoc />
        public UpdateResult UpdateMany(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
            UpdateDefinition<TDocument> update,
#if NETSTANDARD2_1
            UpdateOptions? options = null,
#else
            UpdateOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.UpdateMany(session, filter, update, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UpdateResult> UpdateManyAsync(
            FilterDefinition<TDocument> filter,
            UpdateDefinition<TDocument> update,
#if NETSTANDARD2_1
            UpdateOptions? options = null,
#else
            UpdateOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.UpdateManyAsync(_session, filter, update, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UpdateResult> UpdateManyAsync(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
            UpdateDefinition<TDocument> update,
#if NETSTANDARD2_1
            UpdateOptions? options = null,
#else
            UpdateOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.UpdateManyAsync(session, filter, update, options, cancellationToken);
        }

        /// <inheritdoc />
        public UpdateResult UpdateOne(
            FilterDefinition<TDocument> filter,
            UpdateDefinition<TDocument> update,
#if NETSTANDARD2_1
            UpdateOptions? options = null,
#else
            UpdateOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.UpdateOne(_session, filter, update, options, cancellationToken);
        }

        /// <inheritdoc />
        public UpdateResult UpdateOne(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
            UpdateDefinition<TDocument> update,
#if NETSTANDARD2_1
            UpdateOptions? options = null,
#else
            UpdateOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.UpdateOne(session, filter, update, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UpdateResult> UpdateOneAsync(
            FilterDefinition<TDocument> filter,
            UpdateDefinition<TDocument> update,
#if NETSTANDARD2_1
            UpdateOptions? options = null,
#else
            UpdateOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.UpdateOneAsync(_session, filter, update, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<UpdateResult> UpdateOneAsync(
            IClientSessionHandle session,
            FilterDefinition<TDocument> filter,
            UpdateDefinition<TDocument> update,
#if NETSTANDARD2_1
            UpdateOptions? options = null,
#else
            UpdateOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.UpdateOneAsync(session, filter, update, options, cancellationToken);
        }

        /// <inheritdoc />
        public IChangeStreamCursor<TResult> Watch<TResult>(
            PipelineDefinition<ChangeStreamDocument<TDocument>, TResult> pipeline,
#if NETSTANDARD2_1
            ChangeStreamOptions? options = null,
#else
            ChangeStreamOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.Watch(_session, pipeline, options, cancellationToken);
        }

        /// <inheritdoc />
        public IChangeStreamCursor<TResult> Watch<TResult>(
            IClientSessionHandle session,
            PipelineDefinition<ChangeStreamDocument<TDocument>, TResult> pipeline,
#if NETSTANDARD2_1
            ChangeStreamOptions? options = null,
#else
            ChangeStreamOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.Watch(session, pipeline, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<IChangeStreamCursor<TResult>> WatchAsync<TResult>(
            PipelineDefinition<ChangeStreamDocument<TDocument>, TResult> pipeline,
#if NETSTANDARD2_1
            ChangeStreamOptions? options = null,
#else
            ChangeStreamOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.WatchAsync(_session, pipeline, options, cancellationToken);
        }

        /// <inheritdoc />
        public Task<IChangeStreamCursor<TResult>> WatchAsync<TResult>(
            IClientSessionHandle session,
            PipelineDefinition<ChangeStreamDocument<TDocument>, TResult> pipeline,
#if NETSTANDARD2_1
            ChangeStreamOptions? options = null,
#else
            ChangeStreamOptions options = null,
#endif
            CancellationToken cancellationToken = default
        )
        {
            return _collection.WatchAsync(session, pipeline, options, cancellationToken);
        }

        /// <inheritdoc />
        public IMongoCollection<TDocument> WithReadConcern(ReadConcern readConcern)
        {
            return _collection.WithReadConcern(readConcern);
        }

        /// <inheritdoc />
        public IMongoCollection<TDocument> WithReadPreference(ReadPreference readPreference)
        {
            return _collection.WithReadPreference(readPreference);
        }

        /// <inheritdoc />
        public IMongoCollection<TDocument> WithWriteConcern(WriteConcern writeConcern)
        {
            return _collection.WithWriteConcern(writeConcern);
        }

        /// <inheritdoc />
        public CollectionNamespace CollectionNamespace => _collection.CollectionNamespace;

        /// <inheritdoc />
        public IMongoDatabase Database => _collection.Database;

        /// <inheritdoc />
        public IBsonSerializer<TDocument> DocumentSerializer => _collection.DocumentSerializer;

        /// <inheritdoc />
        public IMongoIndexManager<TDocument> Indexes => _collection.Indexes;

        /// <inheritdoc />
        public MongoCollectionSettings Settings => _collection.Settings;
#pragma warning restore 618
    }
}