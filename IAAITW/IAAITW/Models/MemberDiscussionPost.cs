using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Models
{
    public class MemberDiscussionPost
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "標題為必填欄位")]
        [StringLength(200)]
        [Display(Name = "標題")]
        public string Title { get; set; }

        [Required(ErrorMessage = "內容為必填欄位")]
        [Display(Name = "內容")]
        [AllowHtml]
        public string Content { get; set; }

        [Required(ErrorMessage = "發表人為必填欄位")]
        [Display(Name = "發表人")]
        // 外鍵，對應 MemberInfo 的 Id
        public int PosterId { get; set; } = 2;
        [ForeignKey("PosterId")]
        public virtual MemberInfo MemberInfo { get; set; }

        [Display(Name = "發表時間")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "最後更新時間")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        // 對應的回覆集合
        public virtual ICollection<MemberDiscussionReply> Replies { get; set; }
    }
}