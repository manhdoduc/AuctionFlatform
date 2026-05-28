using AuctionPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionPlatform.Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        
        public Guid AuctionId { get; set; }
        public virtual Auction Auction { get; set; } 
        
        public Guid WinnerId { get; set; }
        public virtual User Winner { get; set; }

        public decimal FinalPrice { get; set; }
        public OrderStatus Status { get; set; } // Sử dụng enum vừa tạo
        public DateTime PaymentDeadline { get; set; }
        public DateTime? PaidAt { get; set; }
        public string? ShippingAddress { get; set; }
        public DateTime CreatedAt { get; set; }


    }
}
