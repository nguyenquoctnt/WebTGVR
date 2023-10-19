using VDMMutiline.SharedComponent.Entities;
using System.ComponentModel.DataAnnotations;

namespace VDMMutiline.SharedComponent.EntityInfo
{
    public class AspNetUserInfo: AspNetUser
    {
        [StringLength(100, ErrorMessage = "Mật khẩu nhiều hơn 6 ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu không hợp lệ.")]
        public string ConfirmPassword { get; set; }
        public string Taikhoancha { get; set; }
    }
}