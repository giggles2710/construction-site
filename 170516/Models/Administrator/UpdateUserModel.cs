using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace _170516.Models.Administrator
{
    public class UpdateUserModel
    {
        public string AccountID { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên đăng nhập")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập mật khẩu")]
        [MinLength(8, ErrorMessage ="Mật khẩu phải ít nhất 8 ký tự")]
        [MaxLength(16, ErrorMessage = "Mật khẩu phải nhiều nhất chỉ 16 ký tự")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string PasswordInUpdate { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập họ")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập tên")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại")]
        public string PhoneNumber { get; set; }
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ email")]
        public string EmailAddress { get; set; }
        public Nullable<int> RoleID { get; set; }
        public string HashToken { get; set; }
        public bool IsActive { get; set; }
        public string CreatedUserID { get; set; }
        public string Note { get; set; }       
        public string UserImage { get; set; }
    }
}