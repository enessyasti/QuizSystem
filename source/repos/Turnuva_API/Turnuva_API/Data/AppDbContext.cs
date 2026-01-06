using Microsoft.EntityFrameworkCore;
using TournamentAPI.Models;

namespace TournamentAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Tournament> Tournaments { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<Bracket> Brackets { get; set; }
    }
}