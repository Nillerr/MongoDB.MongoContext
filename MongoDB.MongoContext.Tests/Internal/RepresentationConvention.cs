using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;

namespace MongoDB.MongoContext.Tests
{
    public abstract class RepresentationConvention<T> : ConventionBase, IMemberMapConvention
    {
        public RepresentationConvention(BsonType representation)
        {
            Representation = representation;
        }

        public BsonType Representation { get; }

        public void Apply(BsonMemberMap memberMap)
        {
            var memberType = memberMap.MemberType;
            if (memberType == typeof(T))
            {
                if (!(memberMap.GetSerializer() is IRepresentationConfigurable serializer))
                    return;
                
                var serializerWithRepresentation = serializer.WithRepresentation(Representation);
                memberMap.SetSerializer(serializerWithRepresentation);
            }
            else
            {
                if (Nullable.GetUnderlyingType(memberType) != typeof(T) || !(memberMap.GetSerializer() is IChildSerializerConfigurable serializer) || !(serializer.ChildSerializer is IRepresentationConfigurable childSerializer))
                    return;
                
                var childSerializerWithRepresentation = childSerializer.WithRepresentation(Representation);
                var serializerWithRepresentation = serializer.WithChildSerializer(childSerializerWithRepresentation);
                memberMap.SetSerializer(serializerWithRepresentation);
            }
        }
    }
}