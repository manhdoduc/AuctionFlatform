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
    public class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            // Tên bảng
            builder.ToTable("OutboxMessages");

            // 1. Khóa chính (Primary Key)
            builder.HasKey(m => m.Id);
            builder.Property(m => m.Id)
                   .HasColumnType("UNIQUEIDENTIFIER");

            // 2. Thuộc tính chuỗi (String / Text fields)
            builder.Property(m => m.EventType)
                   .HasColumnType("NVARCHAR(200)")
                   .IsRequired();

            builder.Property(m => m.Payload)
                   .HasColumnType("NVARCHAR(MAX)")
                   .IsRequired(); // Thường payload không được rỗng để worker có dữ liệu xử lý

            builder.Property(m => m.Error)
                   .HasColumnType("NVARCHAR(MAX)")
                   .IsRequired(false); // NULL nếu chưa/không lỗi

            // 3. Thuộc tính số và bộ đếm
            builder.Property(m => m.RetryCount)
                   .HasColumnType("INT")
                   .HasDefaultValue(0)
                   .IsRequired();

            // 4. Các thuộc tính thời gian (DateTime fields)
            builder.Property(m => m.CreatedAt)
                   .HasColumnType("DATETIME2")
                   .HasDefaultValueSql("GETUTCDATE()")
                   .IsRequired();

            builder.Property(m => m.ProcessedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired(false); // NULL nếu message chưa được xử lý thành công

            // =========================================================================
            // Tối ưu hóa hiệu năng (Performance Optimization Indexing)
            // =========================================================================

            // Background worker thường xuyên quét các tin nhắn CHƯA xử lý (ProcessedAt IS NULL)
            // Việc tạo một Index kèm điều kiện Filter ở đây sẽ giúp câu lệnh quét tăng tốc vượt trội
            builder.HasIndex(m => m.ProcessedAt)
                   .HasFilter("[ProcessedAt] IS NULL");
        }
    }
}
