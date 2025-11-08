using System;
using System.ComponentModel.DataAnnotations;

namespace IAAITW.Models
{
    public class Admin
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "帳號")]
        public string Account { get; set; }

        [Display(Name = "密碼")]
        public string Password { get; set; }

        [Display(Name = "姓名")]
        public string Name { get; set; }

        [Display(Name = "權限")]
        public int Permission { get; set; }

        [Display(Name = "建立日期")]
        public DateTime CreateDate { get; set; } = DateTime.Now;

        [Display(Name = "最後更新時間")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;
    }
}