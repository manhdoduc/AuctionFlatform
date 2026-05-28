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
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            // Tên bảng
            builder.ToTable("Orders");

            // 1. Khóa chính (Primary Key)
            builder.HasKey(o => o.Id);
            builder.Property(o => o.Id)
                   .HasColumnType("UNIQUEIDENTIFIER");

            // 2. Thuộc tính số và trạng thái (Numeric / Enum fields)
            builder.Property(o => o.FinalPrice)
                   .HasColumnType("DECIMAL(18,2)")
                   .IsRequired();

            // Chuyển đổi Enum OrderStatus sang dạng chuỗi VARCHAR/NVARCHAR khi lưu DB
            builder.Property(o => o.Status)
                   .HasConversion<string>()
                   .HasColumnType("NVARCHAR(30)")
                   .IsRequired();

            // 3. Thuộc tính chuỗi (String fields)
            builder.Property(o => o.ShippingAddress)
                   .HasColumnType("NVARCHAR(500)")
                   .IsRequired(false); // NULL

            // 4. Các thuộc tính thời gian (DateTime fields)
            builder.Property(o => o.PaymentDeadline)
                   .HasColumnType("DATETIME2")
                   .IsRequired();

            builder.Property(o => o.PaidAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired(false); // NULL

            builder.Property(o => o.CreatedAt)
                   .HasColumnType("DATETIME2")
                   .HasDefaultValueSql("GETUTCDATE()")
                   .IsRequired();

            // =========================================================================
            // Cấu hình các mối quan hệ và Khóa ngoại (Relationships & Foreign Keys)
            // =========================================================================

            // Mối quan hệ 1 - 1 với Phiên đấu giá (Auction)
            builder.HasOne(o => o.Auction)
                   .WithOne(a => a.Orders) // Bản thân Auction cũng chỉ có tối đa 1 Order điều hướng ngược lại
                   .HasForeignKey<Order>(o => o.AuctionId) // Chỉ định rõ bảng chứa khóa ngoại chính là Order
                   .OnDelete(DeleteBehavior.Restrict) // Giữ lại đơn hàng để làm Audit/Đối soát kể cả khi ẩn/xóa phiên (nếu cần)
                   .IsRequired();

            // Ràng buộc UNIQUE cho AuctionId (EF Core tự sinh khi dùng HasOne-WithOne, nhưng khai báo thêm cho rõ ràng)
            builder.HasIndex(o => o.AuctionId)
                   .IsUnique();

            // Mối quan hệ với Người thắng cuộc / Người mua (User)
            builder.HasOne(o => o.Winner)
                   .WithMany(u => u.Orders) // Một user có thể thắng nhiều đơn hàng ở các phiên khác nhau
                   .HasForeignKey(o => o.WinnerId)
                   .OnDelete(DeleteBehavior.Restrict) // Không cascade delete để tránh lỗi vòng lặp từ bảng User
                   .IsRequired();
        }
    }
}
