using Microsoft.EntityFrameworkCore;
using SportEvents.Core.Entities;

namespace SportEvents.Persistence
{
    public class SportEventsDbContext : DbContext
    {
        public SportEventsDbContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        /// <summary>
        /// Виды спорта
        /// </summary>
        public DbSet<Sport> Sports { get; set; }

        /// <summary>
        /// События
        /// </summary>
        public DbSet<Event> Events { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Sport>()
                .HasKey(s => s.SportId);

            modelBuilder.Entity<Sport>().HasData(new[]
                {
                    new Sport()
                    {
                        SportId = 1,
                        Name = "Football"
                    },
                    new Sport()
                    {
                        SportId = 2,
                        Name = "Basketball"
                    },
                    new Sport()
                    {
                        SportId = 3,
                        Name = "Baseball"
                    },
                    new Sport()
                    {
                        SportId = 4,
                        Name = "Chess"
                    }
                });


            modelBuilder.Entity<Event>()
                .HasKey(ev => ev.EventId);

            modelBuilder.Entity<Event>()
                .HasIndex(ev => ev.SportId);

            modelBuilder.Entity<Event>()
                .Property(e => e.DrawPrice).HasColumnType("decimal(3,2)");
            modelBuilder.Entity<Event>()
                .Property(e => e.Team1Price).HasColumnType("decimal(3,2)");
            modelBuilder.Entity<Event>()
                .Property(e => e.Team2Price).HasColumnType("decimal(3,2)");

            base.OnModelCreating(modelBuilder);
        }
    }
}
