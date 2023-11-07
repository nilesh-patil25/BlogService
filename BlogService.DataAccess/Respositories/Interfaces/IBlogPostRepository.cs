using BlogService.Models;

namespace BlogService.DataAccess.Respositories.Interfaces
{
    public interface IBlogPostRepository : IRepositoryBase<BlogPost>
    {
        Task<BlogPost> FindByIdAsync(int id);
    }
}
