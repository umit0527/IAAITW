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
        [Display(Name = "標題")]
        public string Title { get; set; }
        [Display(Name = "描述")]
        public string Description { get; set; }

        [Display(Name = "檔案")]
        public string FilePath { get; set; }

        [Display(Name = "上傳日期")]
        public DateTime UploadDate { get; set; } = DateTime.Now;

        [NotMapped]
        [Display(Name = "檔案上傳")]    
        public HttpPostedFileBase FileUpload { get; set; }

        //上傳人員
        [Display(Name = "上傳人員")]
        public int UploadUserId { get; set; } = 1;
    }
}