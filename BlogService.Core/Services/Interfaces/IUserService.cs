using BlogService.Models;

namespace BlogService.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<string> Login(string userName, string password);
        Task<AppUser> Register(AppUser user);
    }
}
