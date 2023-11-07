using BlogService.DataAccess.Respositories.Interfaces;
using BlogService.Models;
using Microsoft.EntityFrameworkCore;

namespace BlogService.DataAccess.Respositories
{
    public class BlogPostRepository : RepositoryBase<BlogPost>, IBlogPostRepository
    {

        private readonly BlogPostDbContext _dbContext;

        public BlogPostRepository(BlogPostDbContext dbContext) : base(dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<BlogPost>FindByIdAsync(int id)
        {
            return await _dbContext.BlogPosts.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == id);
        }

    }
}
