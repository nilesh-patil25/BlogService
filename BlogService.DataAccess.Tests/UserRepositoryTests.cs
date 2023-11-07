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
    public class UserRepositoryTests
    {
        private DbContextOptions<BlogPostDbContext> options;
        private AppUser user;
        private string userName;
        private string password;

        public UserRepositoryTests()
        {
            userName = "testUser";
            password = "password123";

            user = new AppUser
            {
                Id = 1,
                UserName = userName,
                Password = password
            };
        }

        [SetUp]
        public void Setup()
        {
            options = new DbContextOptionsBuilder<BlogPostDbContext>()
                .UseInMemoryDatabase(databaseName: "temp_blogDB", b => b.EnableNullChecks(false)).Options;
        }

        [Test]
        public async Task FindByUserNameAndPasswordAsync_ExistingUser_ReturnsUser()
        {
            // Arrange
            using (var dbContext = new BlogPostDbContext(options))
            {
                var userRepository = new UserRepository(dbContext);

                dbContext.Users.Add(user);
                dbContext.SaveChanges();

                // Act
                var result = await userRepository.FindByUserNameAndPasswordAsync(userName, password);

                // Assert
                Assert.IsNotNull(result);
                Assert.AreEqual(user.Id, result.Id);
            }
        }

        [Test]
        public async Task FindByUserNameAndPasswordAsync_NonExistentUser_ReturnsNull()
        {
            // Arrange
            using (var dbContext = new BlogPostDbContext(options))
            {
                var userRepository = new UserRepository(dbContext);

                var userName = "nonExistentUser";
                var password = "wrongPassword";

                // Act
                var result = await userRepository.FindByUserNameAndPasswordAsync(userName, password);

                // Assert
                Assert.IsNull(result);
            }
        }
    }
}
