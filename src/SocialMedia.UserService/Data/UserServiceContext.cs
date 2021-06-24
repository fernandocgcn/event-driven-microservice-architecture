using Microsoft.EntityFrameworkCore;
using SocialMedia.UserService.Entities;

namespace SocialMedia.UserService.Data
{
    public class UserServiceContext : DbContext
    {
        public UserServiceContext(DbContextOptions<UserServiceContext> options)
            : base(options)
        {
        }

        public DbSet<User> User { get; set; }
    }
}
