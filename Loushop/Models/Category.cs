using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Loushop.Models
{
    public class Category
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "نام گروه الزامی است.")]
        [StringLength(100, ErrorMessage = "نام گروه نمی‌تواند بیشتر از 100 کاراکتر باشد.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "توضیحات الزامی است.")]
        [StringLength(500, ErrorMessage = "توضیحات نمی‌تواند بیشتر از 500 کاراکتر باشد.")]
        public string Description { get; set; }


        public ICollection<CategoryToProduct> categoryToProducts { get; set; }
    }
}
