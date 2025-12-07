using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class MemberServiceExp
    {
        [Key]
        public int Id { get; set; }

        [StringLength(100)]
        public string Company { get; set; } // 服務單位

        [StringLength(50)]
        public string JobTitle { get; set; } // 職稱

        [Range(1900, 2100)]
        public int? StartYear { get; set; } // 開始年分

        [Range(1, 12)]
        public int? StartMonth { get; set; } // 開始月份

        [Range(1900, 2100)]
        public int? EndYear { get; set; } // 結束年分，可空值

        [Range(1, 12)]
        public int? EndMonth { get; set; } // 結束月份，可空值

        public DateTime CreatedDate { get; set; } = DateTime.Now; // 建立時間，預設系統時間
        public DateTime UpdatedDate { get; set; } = DateTime.Now; // 更新時間，預設系統時間
                                                                  // 外鍵，對應 MemberInfo 的 Id
        public int MemberId { get; set; }

        [ForeignKey("MemberId")]
        public virtual MemberInfo MemberInfo { get; set; }
    }
}