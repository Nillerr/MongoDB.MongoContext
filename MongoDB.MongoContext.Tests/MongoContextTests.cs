using System;
using System.Threading.Tasks;
using Xunit;

namespace MongoDB.MongoContext.Tests
{
    public class MongoContextTests : Tests, IClassFixture<ArticlesContextFactory>
    {
        private readonly ArticlesContext _context;

        public MongoContextTests(ArticlesContextFactory contextFactory)
        {
            _context = contextFactory.CreateContext(this);
        }

        [Fact]
        public async Task Add_ShouldInsertDocumentsInOrder()
        {
            // Arrange
            var article1 = new Article
            {
                Id = Guid.Parse("01550e61-778c-4f4b-bda8-ff46c0c5235c"),
                Title = "The first article",
                Body = "The body of the first article",
                CreatedAt = Clock.UtcNow,
            };
            
            var article2 = new Article
            {
                Id = Guid.Parse("e9692853-e43f-49d7-8fea-7ec0ef83f3d6"),
                Title = "The second article",
                Body = "The body of the second article",
                CreatedAt = Clock.UtcNow,
            };

            _context.Articles.Add(article1);
            _context.Articles.Add(article2);

            // Act
            await _context.SaveChangesAsync();

            // Assert
            await _context.Articles.Collection.ShouldBeEquivalentToAsync(new[] { article1, article2 });
        }

        [Fact]
        public async Task Remove_WhenNotTracked_ShouldDeleteDocument()
        {
            // Arrange
            var otherArticle = new Article
            {
                Id = Guid.Parse("01550e61-778c-4f4b-bda8-ff46c0c5235c"),
                Title = "The first article",
                Body = "The body of the first article",
                CreatedAt = Clock.UtcNow,
            };

            await _context.Articles.Collection.InsertOneAsync(otherArticle);
            
            var articleId = Guid.Parse("c8cd8f46-4872-406d-ba33-1db7b1426c66");

            await _context.Articles.Collection.InsertOneAsync(new Article
            {
                Id = articleId,
                Title = "The Title",
                Body = "The Body",
                CreatedAt = Clock.UtcNow,
            });
            
            _context.Articles.Remove(new Article { Id = articleId });

            // Act
            await _context.SaveChangesAsync();

            // Assert
            await _context.Articles.Collection.ShouldBeEquivalentToAsync(new[] { otherArticle });
        }

        [Fact]
        public async Task Remove_WhenTracked_ShouldDeleteDocument()
        {
            // Arrange
            var otherArticle = new Article
            {
                Id = Guid.Parse("01550e61-778c-4f4b-bda8-ff46c0c5235c"),
                Title = "The first article",
                Body = "The body of the first article",
                CreatedAt = Clock.UtcNow,
            };

            await _context.Articles.Collection.InsertOneAsync(otherArticle);
            
            var articleId = Guid.Parse("c8cd8f46-4872-406d-ba33-1db7b1426c66");

            await _context.Articles.Collection.InsertOneAsync(new Article
            {
                Id = articleId,
                Title = "The Title",
                Body = "The Body",
                CreatedAt = Clock.UtcNow,
            });

            var articleToDelete = await _context.Articles
                .Find(e => e.Id == articleId)
                .SingleAsync();
            
            _context.Articles.Remove(articleToDelete);

            // Act
            await _context.SaveChangesAsync();

            // Assert
            await _context.Articles.Collection.ShouldBeEquivalentToAsync(new[] { otherArticle });
        }
    }
}