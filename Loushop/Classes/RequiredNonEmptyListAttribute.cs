namespace Loushop.Classes
{
    using Loushop.Models;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;

    public class RequiredNonEmptyListAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var list = value as IList<Category>; // بررسی مقدار لیست
            if (list != null && list.Any())
            {
                return ValidationResult.Success; // اگر لیست حداقل یک آیتم داشته باشد، معتبر است
            }
            return new ValidationResult(ErrorMessage ?? "لطفاً حداقل یک دسته‌بندی انتخاب کنید."); // پیام خطا
        }
    }

}
