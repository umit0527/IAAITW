using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class DiscussionListViewModel
    {
        public int Id { get; set; }
        public int PostId { get; set; }

        [Display(Name = "標題")]
        public string Title { get; set; }

        [Display(Name = "發表人")]
        public string PosterName { get; set; }

        [Display(Name = "發表時間")]
        public DateTime CreatedDate { get; set; }

        [Display(Name = "最新回覆人")]
        public string LatestReplierName { get; set; }

        [Display(Name = "最新回覆時間")]
        public DateTime? LatestReplyDate { get; set; }

        [Display(Name = "回覆數")]
        public int ReplyCount { get; set; }
    }
}