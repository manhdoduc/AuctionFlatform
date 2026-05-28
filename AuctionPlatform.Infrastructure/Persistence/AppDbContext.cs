using AuctionPlatform.Domain.Entities;
using AuctionPlatform.Infrastructure.Persistence.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AuctionPlatform.Infrastructure.Persistence
{
    public class AppDbContext : IdentityDbContext<User, IdentityRole<Guid>, Guid>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Auction> Auctions { get; set; } = null!;
        public DbSet<BidLog> BidLogs { get; set; } = null!;
        public DbSet<Order> Orders { get; set; } = null!;
        public DbSet<OutboxMessage> OutboxMessages { get; set; } = null!;


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            // Đăng ký cấu hình cho bảng Users
            modelBuilder.ApplyConfiguration(new UserConfiguration());

            // Đăng ký cấu hình cho bảng Auctions
            modelBuilder.ApplyConfiguration(new AuctionConfiguration());

            // Đăng ký cấu hình cho bảng BidLogs
            modelBuilder.ApplyConfiguration(new BidLogConfiguration());

            // Đăng ký cấu hình cho bảng Orders
            modelBuilder.ApplyConfiguration(new OrderConfiguration());

            // Đăng ký cấu hình cho bảng OutboxMessages
            modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());

        }
    }
}
