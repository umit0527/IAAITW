using MvcPaging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class DiscussionContentViewModel
    {
        public int PostId { get; set; }
        public string Title { get; set; }

        public string Content { get; set; }

        public string PosterName { get; set; }

        public DateTime CreatedDate { get; set; }

        public IPagedList<ReplyViewModel> Replies { get; set; }
    }

    public class ReplyViewModel
    {
        public string ReplierName { get; set; }
        public DateTime ReplyDate { get; set; }
        public string ReplyContent { get; set; }
    }
}