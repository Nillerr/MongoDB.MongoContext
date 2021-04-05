namespace MongoDB.MongoContext
{
    internal sealed class TrackedDocument<TDocument>
    {
        public TDocument Document { get; }
        public DocumentState State { get; set; }

        public TrackedDocument(TDocument document, DocumentState state)
        {
            Document = document;
            State = state;
        }
    }
}