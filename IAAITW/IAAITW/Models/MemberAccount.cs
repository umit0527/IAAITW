using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class MemberAccount
    {
        [Key] // 主鍵
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Account { get; set; } // 帳號

        [Required]
        [StringLength(255)]
        public string Password { get; set; } // 密碼 (建議存 Hash)

        [Required]
        [StringLength(255)]
        public string Salt { get; set; } // 密碼用鹽值

        [Required]
        public bool IsActive { get; set; } = true; // 是否啟用，預設啟用

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now; // 建立時間，預設系統時間
    }
}