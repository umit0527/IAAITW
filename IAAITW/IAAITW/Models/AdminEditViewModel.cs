using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class AdminEditViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "請輸入帳號")]
        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Display(Name = "密碼")]
        [MinLength(4, ErrorMessage = "密碼需至少 4 個字元")]
        public string Password { get; set; }

        [Compare("NewPassword", ErrorMessage = "與密碼不相符")]
        [Display(Name = "確認密碼")]
        [MinLength(4, ErrorMessage = "密碼需至少 4 個字元")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "密碼")]
        [MinLength(4, ErrorMessage = "密碼需至少 4 個字元")]
        public string NewPassword { get; set; }

        [Display(Name = "密碼鹽")]
        public string PasswordSalt { get; set; }

        [Required(ErrorMessage = "請輸入姓名")]
        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "請輸入 Email")]
        [EmailAddress(ErrorMessage = "請輸入正確的 Email 格式")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Display(Name = "權限")]
        public int Permission { get; set; } = 1;

        [Display(Name = "建立日期")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "最後更新時間")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}