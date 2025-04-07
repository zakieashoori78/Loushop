using Loushop.Classes;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Loushop.Models
{
    public class AddEditViewModel
    {
        public int Id { get; set;}
        [Required(ErrorMessage = "وارد کردن نام محصول ضروری است")]
        [Display(Name = "نام محصول")]
        public string Name { get; set;}
        [Required(ErrorMessage = "وارد کردن توضیحات ضروری است")]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
        [Required(ErrorMessage = "وارد کردن قیمت ضروری است")]
        [Range(10000, double.MaxValue, ErrorMessage = "قیمت اشتباه است")]
        [Display(Name = "قیمت")]
        public decimal Price { get; set; }
        [Required(ErrorMessage = "موجودی را وارد نمایید")]
        [Range(1, double.MaxValue, ErrorMessage = "موجودی باید بیش از یک عدد باشد")]
        [Display(Name = "تعداد در سبد")]
        public int QuantityInStock { get; set; }
        [Required(ErrorMessage = "وارد کردن عکس محصول ضروری است")]
        [Display(Name = "عکس محصول")]
        public IFormFile Picture { get; set; }
        [RequiredNonEmptyList(ErrorMessage = "لطفاً حداقل یک دسته‌بندی انتخاب کنید.")]
        public List<Category> Categories { get; set; }
       
    }
}
