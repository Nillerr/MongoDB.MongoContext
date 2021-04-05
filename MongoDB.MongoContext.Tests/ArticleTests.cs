using System;
using Xunit;

namespace MongoDB.MongoContext.Tests
{
    public class ArticleTests : Tests
    {
        [Fact]
        public void ChangeTitle_ShouldChangeTitle()
        {
            // Arrange
            var changedArticle = new Article
            {
                Id = Guid.Parse("b2eb027a-2ab8-4733-ac09-bfdf72490d7d"),
                Title = "The changed title",
                Body = "The body",
                CreatedAt = Clock.UtcNow,
            };
            
            var article = new Article
            {
                Id = Guid.Parse("b2eb027a-2ab8-4733-ac09-bfdf72490d7d"),
                Title = "The original title",
                Body = "The body",
                CreatedAt = Clock.UtcNow,
            };
            
            // Act
            article.ChangeTitle("The changed title"); // Mutates `article` by setting `Title`
            
            // Assert
            Assert.Equal(changedArticle, article);
        }
    }
}