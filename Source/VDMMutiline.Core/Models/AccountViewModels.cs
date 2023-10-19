using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Http;
using System.Web.Mvc;


namespace VDMMutiline.Core.Models
{
    public class ExternalLoginConfirmationViewModel
    {
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Thông tin bắt buộc"), Display(Name = "Tên đăng nhập"), RegularExpression("^[a-zA-Z0-9_-]+$", ErrorMessage = "Bạn chỉ được nhập chữ hoặc số!")]
        public string UserName { get; set; }
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
        [Required(ErrorMessage = "Thông tin bắt buộc"), EmailAddress, Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name = "Tên tài khoản")]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [Display(Name = "Ghi nhớ đăng nhập?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterViewModel
    {
        [Display(Name = "Tên tài khoản")]
        [Required(ErrorMessage = "Tên đăng nhập không được trống."),
            StringLength(50, ErrorMessage = "Ít nhất 6 ký tự, nhiều nhất 50 ký tự", MinimumLength = 6),
            RegularExpression("^[a-zA-Z0-9_-]+$", ErrorMessage = "Tên đăng nhập không chưa ký tự đặc biệt")
            ]
        public string UserName { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email không được trống."),
            EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; }

        [Display(Name = "Tên hiện thị")]
        [Required(ErrorMessage = "Tên hiển thị không được trống.")]
        public string DisplayName { get; set; }

        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được trống."),
            StringLength(100, ErrorMessage = "Ít nhất 6 ký tự, nhiều nhất 100 ký tự", MinimumLength = 6),
            DataType(DataType.Password), 
            RegularExpression("^(?=.*[0-9])[0-9a-zA-Z!@#$%^&*0-9]{6,}$",ErrorMessage ="Mật khẩu chỉ chứa ký tự và số")]
        public string Password { get; set; }

        [Display(Name = "Xác nhận mật khẩu")]
        [Required(ErrorMessage = "Mật khẩu không được trống."),DataType(DataType.Password),
            System.ComponentModel.DataAnnotations.Compare("Password",ErrorMessage ="Mật khẩu không giống nhau.")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Điện thoại")]
        [Required(ErrorMessage = "Điện thoại không được trống."),
            Phone(ErrorMessage = "Số điện thoại không hợp lệ")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Web site")]
        public string Website { get; set; }

        [Display(Name = "Facebook")]
        public string Facebook { get; set; }
        [Display(Name = "Địa chỉ")]
        public string Location { get; set; }

        public List<int> RoleGrouplist { get; set; }

        public bool? LaKhachHang { get; set; }
        public string TenCongTy { get; set; }
        public int? IdQuanhuyen { get; set; }
        public int? IdGroup { get; set; }
        public int? IdTinh { get; set; }
        public string Code { get; set; }
        public bool? IsActive { get; set; }

    }

    public class ResetPasswordViewModel
    {
        [Display(Name = "Username")]
        public string Username { get; set; }
        [Required]
        [Display(Name = "User ID")]
        public int UserId { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được trống."),
         StringLength(100, ErrorMessage = "Ít nhất 6 ký tự, nhiều nhất 100 ký tự", MinimumLength = 6),
         DataType(DataType.Password),
         RegularExpression("^(?=.*[0-9])[0-9a-zA-Z!@#$%^&*0-9]{6,}$", ErrorMessage = "Mật khẩu chỉ chứa ký tự và số")]
        public string Password { get; set; }
        [Display(Name = "Confirm password")]
        [Required(ErrorMessage = "Mật khẩu không được trống."), DataType(DataType.Password),
            System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Mật khẩu không giống nhau.")]
        public string ConfirmPassword { get; set; }
        public string Code { get; set; }
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }

    public class ForgotPasswordViewModel
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

}
