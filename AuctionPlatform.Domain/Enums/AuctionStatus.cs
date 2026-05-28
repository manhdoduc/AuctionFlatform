using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuctionPlatform.Domain.Enums
{
    public enum AuctionStatus
    {
        Scheduled,
        Active,
        Ended,
        Completed,
        Cancelled
    }
}
