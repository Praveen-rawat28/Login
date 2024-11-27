using Login.Models;
using Microsoft.EntityFrameworkCore;

namespace Login.DataContext
{
    public class APIDbContext: DbContext
    {
        public APIDbContext(DbContextOptions<APIDbContext> Option):base(Option)
        {
        }
        public DbSet<Users> Users { get; set; }
        //public DbSet<LoginUser> LoginUser {  get; set; }

    }
}
