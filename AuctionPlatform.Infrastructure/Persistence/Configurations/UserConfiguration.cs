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
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Tên bảng
            builder.ToTable("Users");

            // 1. Id (Đã có sẵn trong IdentityUser, cấu hình kiểu dữ liệu và giá trị mặc định)
            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                   .HasColumnType("UNIQUEIDENTIFIER")
                   .HasDefaultValueSql("NEWID()");

            // 2. Email (Đã có sẵn, cấu hình thêm độ dài tối đa)
            builder.Property(u => u.Email)
                   .HasMaxLength(256)
                   .IsRequired();

            builder.HasIndex(u => u.Email)
                   .IsUnique();

            // 3. FullName
            builder.Property(u => u.FullName)
                   .HasColumnType("NVARCHAR(200)")
                   .IsRequired();

            // 4. PasswordHash (Đã có sẵn, cho phép NULL phòng trường hợp đăng nhập OAuth)
            builder.Property(u => u.PasswordHash)
                   .IsRequired(false);

            // 5. GoogleId
            builder.Property(u => u.GoogleId)
                   .HasColumnType("NVARCHAR(100)")
                   .IsRequired(false);

            builder.HasIndex(u => u.GoogleId)
                   .IsUnique();

            // 6. AvatarUrl
            builder.Property(u => u.AvatarUrl)
                   .HasColumnType("NVARCHAR(500)")
                   .IsRequired(false);

            // 7. Role (Nếu bạn không dùng bảng IdentityRole riêng mà lưu trực tiếp chuỗi vào bảng User)
            // Lưu ý: Thuộc tính này chưa xuất hiện trong ảnh thực thể C# của bạn, bạn nên thêm `public string Role { get; set; } = "User";` vào class User nếu muốn dùng.
            builder.Property<string>("Role")
                   .HasColumnType("NVARCHAR(20)")
                   .HasDefaultValue("User")
                   .IsRequired();

            // 8. IsActive (Thuộc tính này chưa có trong class C#, bạn nên thêm `public bool IsActive { get; set; }`)
            builder.Property<bool>("IsActive")
                   .HasColumnType("BIT")
                   .HasDefaultValue(true);

            // 9. CreatedAt
            builder.Property(u => u.CreatedAt)
                   .HasColumnType("DATETIME2")
                   .HasDefaultValueSql("GETUTCDATE()");

            // 10. UpdatedAt
            builder.Property(u => u.UpdatedAt)
                   .HasColumnType("DATETIME2")
                   .IsRequired();
        }
    }
}
