namespace MongoDB.MongoContext
{
    internal interface IDocumentTracker<TDocument>
    {
        TDocument Attach(TDocument document);
    }
}