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

        [Required(ErrorMessage = "姓名必填")]
        [Display(Name = "姓　名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "性別必填")]
        [Display(Name = "性  別")]
        public string Gender { get; set; }

        [Required(ErrorMessage = "聯絡電話必填")]
        [Display(Name = "聯絡電話")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "E-mail必填")]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [StringLength(100)]
        [Display(Name = "詢問標題")]
        public string Title { get; set; }

        [StringLength(500)]
        [Display(Name = "詢問內容")]
        public string Content { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}