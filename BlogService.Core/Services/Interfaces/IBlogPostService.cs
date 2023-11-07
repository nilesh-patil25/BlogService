using BlogService.Models;
using BlogService.Shared.DTOs;
using System.Linq.Expressions;

namespace BlogService.Core.Services.Interfaces
{
    public interface IBlogPostService
    {
        Task<BlogPost> CreatePostAsync(BlogPostDTO blogPost);
        Task<BlogPost> GetPostByIdAsync(int id);
        Task<IEnumerable<BlogPost>> GetAllBlogsAsync(Expression<Func<BlogPost, bool>>? filter = null, string? includeProperties = null);
    }
}
