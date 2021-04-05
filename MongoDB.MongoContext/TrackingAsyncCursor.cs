using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MongoDB.Driver;

namespace MongoDB.MongoContext
{
    internal sealed class TrackingAsyncCursor<TDocument> : IAsyncCursor<TDocument>
    {
        private readonly IAsyncCursor<TDocument> _cursor;
        private readonly IDocumentTracker<TDocument> _documentTracker;
        
        public TrackingAsyncCursor(IAsyncCursor<TDocument> cursor, IDocumentTracker<TDocument> documentTracker)
        {
            _cursor = cursor;
            _documentTracker = documentTracker;

            Current = _cursor.Current;
        }

        public void Dispose()
        {
            _cursor.Dispose();
        }

        public bool MoveNext(CancellationToken cancellationToken = default)
        {
            var result = _cursor.MoveNext(cancellationToken);
            TrackDocuments(_cursor.Current);
            return result;
        }

        public async Task<bool> MoveNextAsync(CancellationToken cancellationToken = default)
        {
            var result = await _cursor.MoveNextAsync(cancellationToken);
            TrackDocuments(_cursor.Current);
            return result;
        }

        private void TrackDocuments(IEnumerable<TDocument> documents)
        {
            var current = new List<TDocument>();
            foreach (var document in documents)
            {
                var tracked = _documentTracker.Attach(document);
                current.Add(tracked);
            }

            Current = current;
        }

        public IEnumerable<TDocument> Current { get; private set; }
    }
}