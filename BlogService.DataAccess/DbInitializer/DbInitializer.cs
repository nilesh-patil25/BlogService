using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlogService.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly BlogPostDbContext _blogPostDbContext;

        public DbInitializer(BlogPostDbContext blogPostDbContext)
        {
            _blogPostDbContext = blogPostDbContext;
        }

        public void Initialize()
        {
            try
            {
                if (_blogPostDbContext.Database.GetPendingMigrations().Count() > 0)
                {
                    _blogPostDbContext.Database.Migrate();
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
