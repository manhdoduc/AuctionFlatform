using AuctionPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionPlatform.Infrastructure.Persistence.Configurations
{
    public class BidLogConfiguration : IEntityTypeConfiguration<BidLog>
    {
        public void Configure(EntityTypeBuilder<BidLog> builder)
        {
            // Tên bảng
            builder.ToTable("BidLogs");

            // 1. Khóa chính (Primary Key)
            builder.HasKey(b => b.Id);
            builder.Property(b => b.Id)
                   .HasColumnType("UNIQUEIDENTIFIER");

            // 2. Các thuộc tính số và logic (Numeric / Boolean fields)
            builder.Property(b => b.BidAmount)
                   .HasColumnType("DECIMAL(18,2)")
                   .IsRequired();

            builder.Property(b => b.IsWinningBid)
                   .HasColumnType("BIT")
                   .HasDefaultValue(false) // DEFAULT 0
                   .IsRequired();

            // 3. Thuộc tính chuỗi (String fields)
            builder.Property(b => b.IdempotencyKey)
                   .HasColumnType("NVARCHAR(100)")
                   .IsRequired();

            // Cấu hình ràng buộc UNIQUE cho IdempotencyKey nhằm chống duplicate request
            builder.HasIndex(b => b.IdempotencyKey)
                   .IsUnique();

            builder.Property(b => b.IpAddress)
                   .HasColumnType("NVARCHAR(50)")
                   .IsRequired(false); // Cho phép NULL

            // 4. Thuộc tính thời gian (DateTime field)
            builder.Property(b => b.BidTime)
                   .HasColumnType("DATETIME2")
                   .HasDefaultValueSql("GETUTCDATE()")
                   .IsRequired();

            // =========================================================================
            // Cấu hình các mối quan hệ và Khóa ngoại (Relationships & Foreign Keys)
            // =========================================================================

            // Mối quan hệ với Phiên đấu giá (Auction)
            builder.HasOne(b => b.Auction)
                   .WithMany(a => a.BidLogs) // Điền navigation property của Auction (ví dụ: BidLogs) nếu có
                   .HasForeignKey(b => b.AuctionId)
                   .OnDelete(DeleteBehavior.Cascade) // Thường khi xóa phiên thì lịch sử đấu giá bị xóa theo
                   .IsRequired();

            // Tạo Index cho AuctionId theo như thiết kế yêu cầu để tối ưu hóa truy vấn lịch sử đặt giá
            builder.HasIndex(b => b.AuctionId);

            // Mối quan hệ với Người đặt giá (User)
            builder.HasOne(b => b.User)
                   .WithMany(u => u.BidLogs)
                   .HasForeignKey(b => b.UserId)
                   .OnDelete(DeleteBehavior.Restrict) // Tránh cascade xóa vòng lặp ngoài ý muốn từ phía bảng User
                   .IsRequired();
        }
    }
}
