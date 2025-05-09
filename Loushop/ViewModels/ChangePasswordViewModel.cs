using System.ComponentModel.DataAnnotations;

namespace Loushop.ViewModels
{
    public class ChangePasswordViewModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور فعلی")]
        public string CurrentPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "رمز عبور جدید")]
        public string NewPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "تایید رمز عبور جدید")]
        [Compare("NewPassword", ErrorMessage = "رمز عبور جدید و تایید رمز عبور با هم مطابقت ندارند.")]
        public string ConfirmPassword { get; set; }
    }
}
