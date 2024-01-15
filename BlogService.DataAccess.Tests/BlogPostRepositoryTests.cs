using BlogService.DataAccess.Respositories;
using BlogService.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogService.DataAccess.Tests
{
    public class BlogPostRepositoryTests
    {
        private DbContextOptions<BlogPostDbContext> options;
        private BlogPost expectedBlogPost;

        public BlogPostRepositoryTests()
        {
            expectedBlogPost = new BlogPost
            {
                Id = 1,
                Title = "Test Post",
                Comments = new List<Comment>
                    {
                        new Comment { Id = 1, Text = "Comment 1" },
                        new Comment { Id = 2, Text = "Comment 2" }
                    }
            };
        }

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<BlogPostDbContext>()
                .UseInMemoryDatabase(databaseName: "temp_blogDB", b => b.EnableNullChecks(false)).Options;
        }

        [Test]
        public async Task FindByIdAsync_ExistingId_ReturnsBlogPostWithComments()
        {
            // Arrange
            using (var dbContext = new BlogPostDbContext(options))
            {
                // Add some test data to the in-memory database
                dbContext.BlogPosts.Add(expectedBlogPost);
                dbContext.SaveChanges();
            }

            using (var dbContext = new BlogPostDbContext(options))
            {
                var repository = new BlogPostRepository(dbContext);

                // Act
                var result = await repository.FindByIdAsync(1);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(1, result.Id);
                Assert.AreEqual(2, result.Comments.Count);
            }
        }

        [Test]
        public async Task FindByIdAsync_NonExistentId_ReturnsNull()
        {
            // Arrange
            using (var dbContext = new BlogPostDbContext(options))
            {
                var repository = new BlogPostRepository(dbContext);

                try
                {
                    // Act
                    var result = await repository.FindByIdAsync(999); // Non-existent ID

                    // Assert
                    Assert.IsNull(result);
                }
                catch (Exception ex)
                {
                    // Log or handle the exception as needed
                    Assert.Fail($"Unexpected exception: {ex.Message}");
                }
            }
        }

    }
}
