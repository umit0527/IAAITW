using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class MemberInfo
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } // 姓名

        [Required]
        public Gender Gender { get; set; } // 性別，下拉選單綁定 Enum

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; } // 生 日，可選填

        [Required]
        public MembershipType MembershipType { get; set; } // 申請類別，RadioButton

        [Phone]
        [StringLength(20)]
        public string Phone { get; set; } // 連絡電話(公)

        [Phone]
        [StringLength(20)]
        public string Mobile { get; set; } // 手機

        [StringLength(200)]
        public string Address { get; set; } // 通訊地址

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } // E-mail

        public bool InternationalMember { get; set; } // 國際會籍，Checkbox

        // 國際會員文件上傳路徑
        [StringLength(255)]
        public string FilePath { get; set; } // 若勾選國際會員則需上傳文件

        // 不儲存進資料庫，只在接收表單時使用
        [NotMapped]
        public HttpPostedFileBase InternationalFile { get; set; }

        [StringLength(100)]
        public string CurrentCompany { get; set; } // 現職單位

        [StringLength(50)]
        public string CurrentJobTitle { get; set; } // 職稱

        [StringLength(50)]
        public string HighestEducation { get; set; } // 最高學歷

        // 外鍵，對應 MemberAccount 的 Id
        public int MemberId { get; set; }
        [ForeignKey("MemberId")]
        public virtual MemberAccount MemberAccounts { get; set; }

    }

    // 性別 Enum
    public enum Gender
    {
        [Display(Name ="男性")]
        Male = 1,
        [Display(Name ="女性")]
        Female = 2,
        [Display(Name ="其他")]
        Other = 3
    }

    // 會員申請類別 Enum
    public enum MembershipType
    {
        [Display(Name = "正式會員")]
        Regular = 1,       // 正式會員
        [Display(Name = "準會員")]
        Probationary = 2,  // 準會員
        [Display(Name = "個人贊助會員")]
        IndividualSponsor = 3, // 個人贊助會員
        [Display(Name = "學生會員")]
        Student = 4        // 學生會員
    }
}