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
    public class AuctionConfiguration : IEntityTypeConfiguration<Auction>
    {
        public void Configure(EntityTypeBuilder<Auction> builder)
        {
            // Tên bảng
            builder.ToTable("Auctions");

            // 1. Khóa chính (Primary Key)
            builder.HasKey(a => a.Id);
            builder.Property(a => a.Id)
                   .HasColumnType("UNIQUEIDENTIFIER");

            // 2. Các thuộc tính chuỗi (String / Text fields)
            builder.Property(a => a.ProductName)
                   .HasColumnType("NVARCHAR(300)")
                   .IsRequired();

            builder.Property(a => a.Description)
                   .HasColumnType("NVARCHAR(MAX)")
                   .IsRequired(false); // Cho phép NULL

            builder.Property(a => a.ImageUrls)
                   .HasColumnType("NVARCHAR(MAX)")
                   .IsRequired(false); // JSON array string

            builder.Property(a => a.HangfireJobId)
                   .HasColumnType("NVARCHAR(100)")
                   .IsRequired(false);

            // 3. Các thuộc tính số thực (Decimal / Numeric fields)
            // Cấu hình rõ ràng độ chính xác Decimal(18,2) theo đúng thiết kế DB
            builder.Property(a => a.StartingPrice)
                   .HasColumnType("DECIMAL(18,2)")
                   .IsRequired();

            builder.Property(a => a.PriceStep)
                   .HasColumnType("DECIMAL(18,2)")
                   .IsRequired();

            builder.Property(a => a.CurrentPrice)
                   .HasColumnType("DECIMAL(18,2)")
                   .IsRequired();

            // 4. Trạng thái và các bộ đếm (Status / Enum / Int)
            // Chuyển đổi Enum thành chuỗi NVARCHAR(20) khi lưu vào Database
            builder.Property(a => a.Status)
                   .HasConversion<string>()
                   .HasColumnType("NVARCHAR(20)")
                   .IsRequired();

            builder.Property(a => a.ExtensionCount)
                   .HasColumnType("INT")
                   .HasDefaultValue(0)
                   .IsRequired();

            // 5. Thuộc tính thời gian (DateTime fields)
            builder.Property(a => a.StartTime)
                   .HasColumnType("DATETIME2")
                   .IsRequired();

            builder.Property(a => a.EndTime)
                   .HasColumnType("DATETIME2")
                   .IsRequired();

            builder.Property(a => a.CreatedAt)
                   .HasColumnType("DATETIME2")
                   .HasDefaultValueSql("GETUTCDATE()")
                   .IsRequired();

            builder.Property(a => a.UpdatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired();

            // 6. Quản lý tranh chấp dữ liệu đồng thời (Optimistic Concurrency)
            // Cấu hình trường RowVersion ánh xạ thành kiểu ROWVERSION (Timestamp) trong SQL Server
            builder.Property(a => a.RowVersion)
                   .IsRowVersion()
                   .IsConcurrencyToken();

            // =========================================================================
            // Cấu hình các mối quan hệ (Relationships & Foreign Keys)
            // =========================================================================

            // Mối quan hệ với Người bán (Seller) - Bắt buộc (Not Null)
            builder.HasOne(a => a.Seller)
                   .WithMany(b => b.Auctions)
                   .OnDelete(DeleteBehavior.Restrict) // Tránh cascade xóa hàng loạt ngoài ý muốn
                   .IsRequired();

            // Mối quan hệ với Người thắng cuộc (Winner) - Không bắt buộc (Nullable)
            builder.HasOne(a => a.Winner)
                   .WithMany(u => u.Auctions)
                   .HasForeignKey(a => a.WinnerId)
                   .OnDelete(DeleteBehavior.Restrict) // Không xóa phiên khi user bị xóa
                   .IsRequired(false);
        }
    }
}
