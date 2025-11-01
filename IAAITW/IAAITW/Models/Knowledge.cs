using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class Knowledge
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "標題")]
        public string Title { get; set; }

        [Display(Name = "描述")]
        public string Description { get; set; }
 

        [Display(Name = "檔案")]
        public string FilePath { get; set; }

        [Display(Name = "置頂")]
        public bool IsTop { get; set; } = false;    

        public int UploadUserId { get; set; }=1;
        [ForeignKey("UploadUserId")]
        public virtual Admin Admin { get; set; }

        [Display(Name = "上傳日期")]
        public DateTime UploadDate { get; set; } = DateTime.Now;
    }
}