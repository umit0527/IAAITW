using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class AdminLoginViewModel
    {
        [Required(ErrorMessage = "請輸入帳號")]
        public string Account { get; set; }

        [Required(ErrorMessage = "請輸入密碼")]
        [MinLength(4, ErrorMessage = "密碼至少 4 個字元")]
        public string Password { get; set; }
    }
}