using Microsoft.EntityFrameworkCore;

namespace JWTAuthService.Database
{
    public class MyDatabaseContext : DbContext
    {
        public MyDatabaseContext()
        {
              
        }
        public MyDatabaseContext(DbContextOptions<MyDatabaseContext> options) : base (options) 
        {
            
        }
        public DbSet<Users> Users { get; set; }
    }
}
