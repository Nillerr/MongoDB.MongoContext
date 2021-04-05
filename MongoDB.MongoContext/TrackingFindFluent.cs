using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    internal sealed class TrackingFindFluent<TDocument> : IOrderedFindFluent<TDocument>
    {
        private readonly IFindFluent<TDocument, TDocument> _find;
        private readonly IDocumentTracker<TDocument> _documentTracker;

        public TrackingFindFluent(
            IFindFluent<TDocument, TDocument> find,
            IDocumentTracker<TDocument> documentTracker)
        {
            _find = find;
            _documentTracker = documentTracker;
        }

        public FilterDefinition<TDocument> Filter
        {
            get => _find.Filter;
            set => _find.Filter = value;
        }

        public FindOptions<TDocument, TDocument> Options => _find.Options;

        public Task<long> CountAsync(CancellationToken cancellationToken = default)
        {
            return _find.CountDocumentsAsync(cancellationToken);
        }

        public IFindFluent<TDocument> Limit(int? limit)
        {
            return new TrackingFindFluent<TDocument>(_find.Limit(limit), _documentTracker);
        }

        public IFindFluent<TDocument> Skip(int? skip)
        {
            return new TrackingFindFluent<TDocument>(_find.Skip(skip), _documentTracker);
        }

        public IOrderedFindFluent<TDocument> Sort(SortDefinition<TDocument> sort)
        {
            return new TrackingFindFluent<TDocument>(_find.Sort(sort), _documentTracker);
        }

        public IFindFluent<TDocument, TDocument> AsNoTracking()
        {
            return _find;
        }

        public IAsyncCursor<TDocument> ToCursor(CancellationToken cancellationToken = default)
        {
            var cursor = _find.ToCursor(cancellationToken);
            return new TrackingAsyncCursor<TDocument>(cursor, _documentTracker);
        }

        public async Task<IAsyncCursor<TDocument>> ToCursorAsync(CancellationToken cancellationToken = default)
        {
            var cursor = await _find.ToCursorAsync(cancellationToken);
            return new TrackingAsyncCursor<TDocument>(cursor, _documentTracker);
        }

        public IFindFluent<TDocument> ToFindFluent()
        {
            return this;
        }
    }
}