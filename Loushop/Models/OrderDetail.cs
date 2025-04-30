using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Loushop.Models
{
    public class OrderDetail
    {  
        [key]
        public int DetailId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [Required]
        public decimal Price { get; set; }
        [Required]
        public int Count { get; set; }
        public Order Order { get; set; }
        [ForeignKey(name: "ProductId")]
        public Product Product { get; set; }


       
    }
}
