using BeFit.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<ExerciseType> ExerciseTypes { get; set; }
        public DbSet<TrainingSession> TrainingSessions { get; set; }
        public DbSet<ExercisePerformed> ExercisePerformeds { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Relations
            builder.Entity<ExercisePerformed>()
                .HasOne(ep => ep.TrainingSession)
                .WithMany(ts => ts.Exercises)
                .HasForeignKey(ep => ep.TrainingSessionId);

            builder.Entity<ExercisePerformed>()
                .HasOne(ep => ep.ExerciseType)
                .WithMany()
                .HasForeignKey(ep => ep.ExerciseTypeId);

            // Name length limit
            builder.Entity<ExerciseType>()
                .Property(et => et.Name)
                .HasMaxLength(100)
                .IsRequired();

            // TrainingSession - User relation
            builder.Entity<TrainingSession>()
                .HasOne(ts => ts.User)
                .WithMany()
                .HasForeignKey(ts => ts.UserId);
        }
    }

}
