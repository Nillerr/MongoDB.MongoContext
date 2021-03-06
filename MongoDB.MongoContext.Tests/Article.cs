using System;
using System.Diagnostics.CodeAnalysis;
using Generator.Equals;

namespace MongoDB.MongoContext.Tests
{
    [Equatable]
    [SuppressMessage("ReSharper", "PartialTypeWithSinglePart", Justification = "Source Generator: Generator.Equals")]
    public sealed partial class Article : MongoAggregate<Article>
    {
        public Guid Id { get; set; }

        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;

        public DateTime CreatedAt { get; set; }

        public void ChangeTitle(string title)
        {
            Title = title;

            Update(update => update
                .Set(e => e.Title, title));
        }
    }
}