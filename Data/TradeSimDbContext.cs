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
        public DbSet<Order> Orders { get; set; }
        public DbSet<PortfolioHolding> PortfolioHoldings { get; set; }
        public DbSet<Position> Positions { get; set; }
        public DbSet<MarketInstrumentProfile> MarketInstrumentProfiles { get; set; }

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
            modelBuilder.Entity<User>()
                .Property(u => u.Balance)
                .HasPrecision(18, 2);

            modelBuilder.Entity<User>()
                .Property(u => u.LockedCapital)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.Price)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.TriggerPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.LimitPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Order>()
                .Property(o => o.ExecutedPrice)
                .HasPrecision(18, 2);
            modelBuilder.Entity<PortfolioHolding>()
                .HasOne(h => h.User)
                .WithMany()
                .HasForeignKey(h => h.UserId);

            modelBuilder.Entity<PortfolioHolding>()
                .HasOne(h => h.TradableInstrument)
                .WithMany()
                .HasForeignKey(h => h.TradableInstrumentId);

            modelBuilder.Entity<PortfolioHolding>()
                .HasIndex(h => new
                {
                    h.UserId,
                    h.TradableInstrumentId
                })
                .IsUnique();

            modelBuilder.Entity<PortfolioHolding>()
                .Property(h => h.AverageBuyPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<Position>()
                .HasOne(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId);

            modelBuilder.Entity<Position>()
                .HasOne(p => p.TradableInstrument)
                .WithMany()
                .HasForeignKey(p => p.TradableInstrumentId);

            modelBuilder.Entity<Position>()
                .Property(p => p.EntryPrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MarketInstrumentProfile>()
                .HasOne(p => p.TradableInstrument)
                .WithOne(i => i.MarketInstrumentProfile)
                .HasForeignKey<MarketInstrumentProfile>(
                    p => p.TradableInstrumentId);

            modelBuilder.Entity<MarketInstrumentProfile>()
                .HasIndex(p => p.TradableInstrumentId)
                .IsUnique();

            modelBuilder.Entity<MarketInstrumentProfile>()
                .Property(p => p.BasePrice)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MarketInstrumentProfile>()
                .Property(p => p.PreviousClose)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MarketInstrumentProfile>()
                .Property(p => p.FiftyTwoWeekHigh)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MarketInstrumentProfile>()
                .Property(p => p.FiftyTwoWeekLow)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MarketInstrumentProfile>()
                .Property(p => p.TickSize)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MarketInstrumentProfile>()
                .Property(p => p.PriceBandPercent)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MarketInstrumentProfile>()
                .Property(p => p.SupportLevel)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MarketInstrumentProfile>()
                .Property(p => p.ResistanceLevel)
                .HasPrecision(18, 2);

            modelBuilder.Entity<MarketInstrumentProfile>()
                .Property(p => p.VolatilityPercent)
                .HasPrecision(18, 4);

            modelBuilder.Entity<MarketInstrumentProfile>()
                .Property(p => p.Beta)
                .HasPrecision(18, 4);

            modelBuilder.Entity<MarketInstrumentProfile>()
                .Property(p => p.TrendBias)
                .HasPrecision(18, 4);

            modelBuilder.Entity<MarketInstrumentProfile>()
                .Property(p => p.MomentumBias)
                .HasPrecision(18, 4);
        }

    }
}