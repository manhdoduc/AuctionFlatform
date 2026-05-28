using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionPlatform.Domain.Entities
{
    public class BidLog
    {
        public Guid Id { get; set; }
        
        public Guid AuctionId { get; set; }
        public virtual Auction Auction { get; set; } 

        public Guid UserId { get; set; }
        public virtual User User { get; set; }

        public decimal BidAmount { get; set; }
        public DateTime BidTime { get; set; }
        public bool IsWinningBid { get; set; }
        public string IdempotencyKey { get; set; } = string.Empty;
        public string? IpAddress { get; set; }

    }
}
