using System;
using System.Threading.Tasks;
using Xunit;

namespace MongoDB.MongoContext.Tests
{
    public class MongoContextTests : Tests, IClassFixture<ArticlesContextFactoryFixture>
    {
        private readonly ArticlesContext _context;

        public MongoContextTests(ArticlesContextFactoryFixture contextFactory)
        {
            _context = contextFactory.CreateContext(this);
        }

        [Fact]
        public async Task Add_ShouldAddDocument()
        {
            // Arrange
            var article = new Article
            {
                Id = Guid.Parse("01550e61-778c-4f4b-bda8-ff46c0c5235c"),
                Title = "The Title",
                Body = "The Body",
            };

            _context.Articles.Add(article);

            // Act
            await _context.SaveChangesAsync();

            // Assert
            await _context.Articles.Collection.ShouldOnlyContainAsync(article);
        }

        [Fact]
        public async Task Add_ShouldAddDocument2()
        {
            // Arrange
            var article = new Article
            {
                Id = Guid.Parse("01550e61-778c-4f4b-bda8-ff46c0c5235c"),
                Title = "The Title",
                Body = "The Body",
            };

            _context.Articles.Add(article);

            // Act
            await _context.SaveChangesAsync();

            // Assert
            await _context.Articles.Collection.ShouldOnlyContainAsync(article);
        }
    }
}