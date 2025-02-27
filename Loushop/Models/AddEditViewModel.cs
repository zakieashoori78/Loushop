using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Loushop.Models
{
    public class AddEditViewModel
    {
        public int Id { get; set;}
        public string Name { get; set;}
        public string Descripthion { get; set; }
        public decimal Price { get; set; }
        public int QuantityInStoke { get; set; }
        public IFormFile Picture { get; set; }
        public List<Category> Categories { get; set; }
       
    }
}
