using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Models
{
    public class News
    {
        [Key]
        public int Id { get; set; }

        // 標題
        [Required(ErrorMessage = "請輸入標題")]
        [StringLength(200)]
        [Display(Name = "標題")]
        public string Title { get; set; }

        // 內容
        [Required(ErrorMessage = "請輸入內容")]
        [Display(Name = "內容")]
        [AllowHtml]
        public string Content { get; set; }

        // 封面圖片
        //[Required(ErrorMessage = "請上傳封面圖片")]
        [StringLength(300)]
        [Display(Name = "封面")]
        public string CoverImage { get; set; }

        [NotMapped]
        [Display(Name = "封面")]
        public HttpPostedFileBase CoverImageFile { get; set; }

        // 發布時間
        [Display(Name = "發布時間")]
        public DateTime CreatedDate { get; set; }

        // 最後更新時間
        [Display(Name = "最後更新時間")]
        public DateTime UpdatedDate { get; set; }

        // 發布人員（MemberAccount / MemberInfo FK）
        [Display(Name = "發布人員")]
        public int PublisherId { get; set; } 

        // 最後更新人員
        [Display(Name = "最後更新人員")]
        public int UpdaterId { get; set; } 

        // 是否置頂
        [Display(Name = "置頂")]
        public bool IsPinned { get; set; } = false;

        // 與 Admin 的關聯
        [ForeignKey("PublisherId")]
        public virtual Admin Publisher { get; set; } 

        [ForeignKey("UpdaterId")]
        public virtual Admin Updater { get; set; }
    }
}