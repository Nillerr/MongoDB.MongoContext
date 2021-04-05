using System;
using MongoDB.Bson;

namespace MongoDB.MongoContext.Tests
{
    public sealed class GuidRepresentationConvention : RepresentationConvention<Guid>
    {
        public GuidRepresentationConvention(BsonType representation)
            : base(representation)
        {
        }
    }
}