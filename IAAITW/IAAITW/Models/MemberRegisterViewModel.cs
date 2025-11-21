using Foolproof;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IAAITW.Models
{
    public class MemberRegisterViewModel
    {
        public int Id { get; set; }

        // 帳號資料
        [Required(ErrorMessage = "帳號必填")]
        [StringLength(50)]
        [Display(Name = "帳  號")]
        public string Account { get; set; }

        //[Required(ErrorMessage = "密碼為必填")]
        [StringLength(255)]
        [Display(Name = "密  碼")]
        public string Password { get; set; }

        //[Required(ErrorMessage = "確認密碼為必填")]
        [StringLength(255)]
        [Compare("Password", ErrorMessage = "與密碼不一致")]
        public string ConfirmPassword { get; set; }

        //[Required]
        [StringLength(255)]
        public string Salt { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime UpdatedDate { get; set; } = DateTime.Now;

        // 基本資料
        [Required(ErrorMessage = "姓名必填")]
        [StringLength(50)]
        [Display(Name = "姓  名")]
        public string Name { get; set; }

        [Required(ErrorMessage = "性別必填")]
        [Display(Name = "性  別")]
        public Gender? Gender { get; set; }

        [Required(ErrorMessage = "生日必填")]
        [DataType(DataType.Date)]
        [Display(Name = "生  日")]
        public DateTime? BirthDate { get; set; }

        [Display(Name = "申請類別")]
        public MembershipType MembershipType { get; set; }= MembershipType.Regular;

        [Required(ErrorMessage = "連絡電話(公)必填")]
        [Phone]
        [StringLength(20)]
        [Display(Name = "連絡電話(公)")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "手機必填")]
        [Phone]
        [StringLength(20)]
        [Display(Name = "手  機")]
        public string Mobile { get; set; }

        [Required(ErrorMessage = "通訊處必填")]
        [StringLength(200)]
        [Display(Name = "通訊處")]
        public string Address { get; set; }

        [Required(ErrorMessage = "E-mail必填")]
        [EmailAddress]
        [StringLength(100)]
        [Display(Name = "E-mail")]
        public string Email { get; set; }

        [Display(Name = "國際會籍")]
        public bool IsInternationalMember { get; set; }

        // 國際會員文件上傳路徑
        [StringLength(255)]
        public string FilePath { get; set; } // 若勾選國際會員則需上傳文件

        // 不儲存進資料庫，只在接收表單時使用
        [NotMapped]
        public HttpPostedFileBase FileUpload { get; set; }

        [Required(ErrorMessage = "現職單位必填")]
        [StringLength(100)]
        [Display(Name = "現職單位")]
        public string CurrentCompany { get; set; }

        [Required(ErrorMessage = "職稱必填")]
        [StringLength(50)]
        [Display(Name = "職  稱")]
        public string CurrentJobTitle { get; set; }

        [Required(ErrorMessage = "最高學歷必填")]
        [StringLength(50)]
        [Display(Name = "最高學歷")]
        public string HighestEducation { get; set; }

        // 總年資
        [Required(ErrorMessage = "合計年資(年)必填")]
        public int? TotalExpYears { get; set; }

        [Required(ErrorMessage = "合計年資(月)必填")]
        public int? TotalExpMonths { get; set; }

        // 服務經歷 (可多筆)
        public List<MemberServiceExp> MemberServiceExps { get; set; } = new List<MemberServiceExp>();

        public int MemberId { get; set; }
    }

    //public class ServiceExpViewModel
    //{
    //    public int Id { get; set; }

    //    [StringLength(100)]
    //    [Display(Name = "服務單位")]
    //    public string Company { get; set; }

    //    [StringLength(50)]
    //    [Display(Name = "職  稱")]
    //    public string ExperienceJobTitle { get; set; }

    //    [Range(1900, 2100)]
    //    public int? StartYear { get; set; }

    //    [Range(1, 12)]
    //    public int? StartMonth { get; set; }

    //    [Range(1900, 2100)]
    //    public int? EndYear { get; set; }

    //    [Range(1, 12)]
    //    public int? EndMonth { get; set; }

    //    public int? TotalYears { get; set; }

    //    public int? TotalMonths { get; set; }
    //}
}