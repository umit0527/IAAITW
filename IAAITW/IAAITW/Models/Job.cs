using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Models
{
    public class Job
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "請輸入內容")]
        [Display(Name = "內容")]
        //[StringLength(1000, ErrorMessage = "內容長度不能超過1000個字")]
        [AllowHtml]
        public string Content { get; set; }

        [Display(Name = "建立時間")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "最後更新時間")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        [Display(Name = "建立人員")]
        public int AdminId { get; set; } 
        [ForeignKey("AdminId")]
        public virtual Admin Admin { get; set; }

        [Display(Name = "最後更新人員")]
        public int UpdatedAdminId { get; set; } 
        [ForeignKey("UpdatedAdminId")]
        public virtual Admin UpdatedAdmin { get; set; }
    }
}