using Microsoft.EntityFrameworkCore;
using TradeSim.Models.Domain;

namespace TradeSim.Data
{
    public class TradeSimDbContext : DbContext
    {
        public TradeSimDbContext(DbContextOptions<TradeSimDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<TradableInstrument> TradableInstruments { get; set; }

        public DbSet<UserWatchlistItem> UserWatchlistItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserWatchlistItem>()
                .HasOne(w => w.User)
                .WithMany()
                .HasForeignKey(w => w.UserId);

            modelBuilder.Entity<UserWatchlistItem>()
                .HasOne(w => w.TradableInstrument)
                .WithMany()
                .HasForeignKey(w => w.TradableInstrumentId);

            modelBuilder.Entity<UserWatchlistItem>()
                .HasIndex(w => new
                {
                    w.UserId,
                    w.TradableInstrumentId
                })
                .IsUnique();
        }
    }
}