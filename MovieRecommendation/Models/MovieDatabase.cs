using Microsoft.EntityFrameworkCore;

namespace MovieRecommendation.Models
{
    public class MovieDatabase:DbContext
    {
        public string connstr;
        public MovieDatabase(string connstr) 
        {
            this.connstr = connstr;
        }
        public DbSet<Movie> Movie { get; set; }
        public DbSet<Rating> Rating { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connstr);
        }
    }
}
