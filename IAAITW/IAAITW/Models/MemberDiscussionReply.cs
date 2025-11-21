using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Models
{
    public class MemberDiscussionReply
    {
        [Key]
        public int Id { get; set; }

        
        [Display(Name = "所屬文章")]
        public int PostId { get; set; }
        // 外鍵，對應 Post 的 Id
        [ForeignKey("PostId")]
        public virtual MemberDiscussionPost Post { get; set; }  

        [Required(ErrorMessage = "回覆內容為必填欄位")]
        [Display(Name = "回覆內容")]
        [AllowHtml]
        public string Content { get; set; }

        [Required(ErrorMessage = "回覆者為必填欄位")]
        [Display(Name = "回覆者")]

        public int ReplierId { get; set; } 
        // 外鍵，對應 MemberInfo 的 Id
        [ForeignKey("ReplierId")]
        public virtual MemberInfo MemberInfo { get; set; }

        [Display(Name = "回覆時間")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "最後更新時間")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;



    }
}