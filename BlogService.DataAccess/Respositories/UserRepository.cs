using BlogService.DataAccess.Respositories.Interfaces;
using BlogService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogService.DataAccess.Respositories
{
    public class UserRepository : RepositoryBase<AppUser>, IUserRepository
    {
        private readonly BlogPostDbContext _dbContext;

        public UserRepository(BlogPostDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<AppUser> FindByUserNameAndPasswordAsync(string userName, string passsword)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName && u.Password== passsword);
        }
    }
}
