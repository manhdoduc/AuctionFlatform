using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionPlatform.Domain.Common
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { set; get; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
