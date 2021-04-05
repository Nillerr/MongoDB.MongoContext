using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Conventions;

namespace MongoDB.MongoContext.Tests
{
    public sealed class ArticlesContext : MongoContext
    {
        static ArticlesContext()
        {
            // Conventions
            var managedTypes = new HashSet<Type> { typeof(Article) };
            
            var conventions = new ConventionPack();
            conventions.Add(new GuidRepresentationConvention(BsonType.String));

            ConventionRegistry.Register("ManagedTypeConventions", conventions, managedTypes.Contains);
        }

        public ArticlesContext(DatabaseContextOptions options)
            : base(options)
        {
            Articles = Collection<Article>("articles", (fb, article) => fb.Where(e => e.Id == article.Id))
                .HasIndex(
                    idx => idx.Combine(
                        idx.Text(e => e.Title),
                        idx.Text(e => e.Body)
                    ),
                    (build, index) =>
                    {
                        index.Weights = build.Weights()
                            .Assign(e => e.Title, 5);
                    }
                )
                .HasIndex(idx => idx.Ascending(e => e.CreatedAt))
                .ToDbCollection();
        }

        public IDbCollection<Article> Articles { get; }
    }
}