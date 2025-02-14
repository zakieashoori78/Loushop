using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Loushop.Models
{
    public class Order
    {
        [key]
        public int OrderId { get; set; }
        [Required]

        public int UserId { get; set; }
        [Required]
        public DateTime CreateDate {get; set;}
        public bool IsFinaly { get; set; }

        [ForeignKey(name: "UserId ")]
        public Users Users { get; set; }
        public List<OrderDetail> OrderDetails { get; set; }
    }
}
