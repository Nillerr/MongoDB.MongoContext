namespace MongoDB.MongoContext
{
    public interface IFindFluentSource<TDocument>
    {
        IFindFluent<TDocument> ToFindFluent();
    }
}