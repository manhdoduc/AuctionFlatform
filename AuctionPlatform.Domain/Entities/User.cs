using AuctionPlatform.Domain.Common;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionPlatform.Domain.Entities
{
    public class User : IdentityUser<Guid>
    {
        public string FullName { get; set; } = null!;
        public string? GoogleId { get; set; }
        public string? AvatarUrl { get; set; }

        public DateTime CreatedAt { set; get; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public bool IsActive { get; set; } 

        public ICollection<Auction> Auctions { get; set; } = [];
        public ICollection<Order> Orders { get; set; } = [];
        public ICollection<BidLog> BidLogs { get; set; } = [];
    }
}
