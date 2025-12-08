using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class Contact
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "請輸入姓名")]
        [Display(Name = "姓　名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "請選擇性別")]
        [Display(Name = "性  別")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "請輸入聯絡電話")]
        [Display(Name = "聯絡電話")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "請輸入E-mail")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "請輸入詢問標題")]
        [StringLength(100)]
        [Display(Name = "詢問標題")]
        public string Title { get; set; }

        [Required(ErrorMessage = "請輸入詢問內容")]
        [StringLength(500)]
        [Display(Name = "詢問內容")]
        public string Content { get; set; }

        [Display(Name = "寄出時間")]
        public DateTime SentDate { get; set; } = DateTime.Now;
    }
}