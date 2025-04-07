using System.Collections.Generic;

namespace Loushop.Dtos.requestDto.Zibal
{
    public class ZibalRequestDto
    {
        public required string merchant { get; set; }
        public string? orderId { get; set; }
        public long amount { get; set; }
        public required string callbackUrl { get; set; }
        public string? description { get; set; }
        public string? mobile { get; set; }

    }

   
}
