using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using review_v2.Resources;


namespace review_v2.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(MyTexts))]
        public string Email { get; set; }
    }

    public class ExternalLoginListViewModel
    {
        public string ReturnUrl { get; set; }
    }

    public class SendCodeViewModel
    {
        public string SelectedProvider { get; set; }
        public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
        public string ReturnUrl { get; set; }
        [Display(Name = "RememberMe", ResourceType = typeof(MyTexts))]
        public bool RememberMe { get; set; }
    }

    public class VerifyCodeViewModel
    {
        [Required]
        public string Provider { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; }
        public string ReturnUrl { get; set; }

        [Display(Name = "Remember this browser?")]
        public bool RememberBrowser { get; set; }

        public bool RememberMe { get; set; }
    }

    public class ForgotViewModel
    {
        [Required]
        [Display(Name = "Email", ResourceType = typeof(MyTexts))]
        public string Email { get; set; }
    }
    
    public class LoginViewModel
    {
        [Required]
        [StringLength(255)]
        [Display(Name = "UserName", ResourceType = typeof(MyTexts))]
        public string Username { get; set; }

        public bool Active { get; set; }

        [StringLength(11, ErrorMessage = "The minimum length must be at least 11 number.")]
        [Display(Name = "PhoneNumber", ResourceType = typeof(MyTexts))]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "The minimum length must be at least 11 number.")]
        public string PhoneNum { get; set; }

        [Required]
        [Display(Name = "Email", ResourceType = typeof(MyTexts))]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "PASSWORD", ResourceType = typeof(MyTexts))]
        [StringLength(100, ErrorMessage = "This is a wrong password, Pls try again")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [StringLength(11, ErrorMessage = "The minimum length must be at least 11 number.")]
        [Display(Name = "PhoneNumber", ResourceType = typeof(MyTexts))]
        [DataType(DataType.PhoneNumber)]
        [Phone]
        [RegularExpression(@"^(\d{11})$", ErrorMessage = "The minimum length must be at least 11 number.")]
        public string PhoneNum { get; set; }

        public bool? Active { get; set; }

        public string UserImageURL { get; set; }

        [Required]
        [Display(Name = "UserName", ResourceType = typeof(MyTexts))]
        public string NickName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(MyTexts))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "PASSWORD", ResourceType = typeof(MyTexts))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(MyTexts))]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(MyTexts))]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "PASSWORD", ResourceType = typeof(MyTexts))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(MyTexts))]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email", ResourceType = typeof(MyTexts))]
        public string Email { get; set; }
    }
}
