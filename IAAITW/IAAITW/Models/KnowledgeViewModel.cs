using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class KnowledgeViewModel
    {
        //資料資訊
        public int Id { get; set; }

        [Display(Name = "標題")]
        public string Title { get; set; }
        [Display(Name = "描述")]
        public string Description { get; set; }

        [Display(Name = "檔案")]
        public string FilePath { get; set; }

        [Display(Name = "置頂")]
        public bool IsTop { get; set; } = false;

        [Display(Name = "上傳日期")]
        public DateTime UploadDate { get; set; } = DateTime.Now;

        [Display(Name = "最後更新時間")]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        [NotMapped]
        [Display(Name = "檔案上傳")]    
        public HttpPostedFileBase FileUpload { get; set; }

        //上傳人員
        [Display(Name = "上傳人員")]
        public int AdminId { get; set; } = 1;
        public virtual Admin Admin { get; set; }  //讓view可以直接使用admin的name

        [Display(Name = "最後更新人員")]
        public int UpdatedAdminId { get; set; } = 1;
        public virtual Admin UpdatedAdmin { get; set; }  //讓view可以直接使用admin的name
    }
}