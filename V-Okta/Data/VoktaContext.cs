using Microsoft.EntityFrameworkCore;
using V_Okta.Data.Entities;

namespace V_Okta.Data
{
    public class VoktaContext : DbContext
    {
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<User> Users { get; set; }

        public VoktaContext(DbContextOptions options) : base(options)
        {
        }
    }
}
