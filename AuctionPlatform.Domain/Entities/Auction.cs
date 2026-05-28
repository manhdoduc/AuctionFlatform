using AuctionPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionPlatform.Domain.Entities
{
    public class Auction
    {
        public Guid Id { get; set; } 

        // Khóa ngoại liên kết tới Người bán
        public Guid SellerId { get; set; }
        public virtual User Seller { get; set; } = null!;

        // Khóa ngoại liên kết tới Người thắng cuộc (Cho phép NULL khi chưa chốt phiên)
        public Guid? WinnerId { get; set; }
        public virtual User? Winner { get; set; }

        public string ProductName { get; set; } = null!;

        // NVARCHAR(MAX) cho phép NULL
        public string? Description { get; set; }

        // Lưu danh sách URL dưới dạng chuỗi JSON Array
        public string? ImageUrls { get; set; }

        public decimal StartingPrice { get; set; }

        public decimal PriceStep { get; set; }

        public decimal CurrentPrice { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public AuctionStatus Status { get; set; } = AuctionStatus.Scheduled;

        public int ExtensionCount { get; set; } = 0; // DEFAULT 0

        public string? HangfireJobId { get; set; }

        // Xử lý chống tranh chấp dữ liệu (Optimistic Concurrency) bằng RowVersion
        public byte[] RowVersion { get; set; } = null!;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public virtual Order Orders { get; set; } 
        public ICollection<BidLog> BidLogs { get; set; } = [];
            
    }
}
