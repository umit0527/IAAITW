using GoogleRecaptcha;
using IAAITW.Models;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.UI.WebControls;

namespace IAAITW.Areas.Front.Controllers
{
    public class MemberController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Front/Member
        [HttpGet]
        public ActionResult Login()
        {

            return View();
        }

        // POST: Front/Member
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(MemberAccount model)
        {
            if (ModelState.IsValid)
            {
                // 驗證帳號密碼
                var user = ValidateUser(model.Account, model.Password);

                if (user == null)
                {
                    // 帳號或密碼錯誤
                    ModelState.AddModelError("", "帳號或密碼錯誤");
                    return View(model);
                }
                // 登入成功，產生表單驗證
                string userData = JsonConvert.SerializeObject(user);
                Utility.SetAuthenTicket(userData, model.Account);
                // 跳轉到會員討論區
                return RedirectToAction("Index", "MemberDiscussion");
            }
            // 驗證失敗或登入失敗，回傳原頁面
            return View(model);
        }

        /// <summary>
        /// 驗證使用者
        /// </summary>
        /// <param name="account">輸入帳號</param>
        /// <param name="password">輸入密碼</param>
        /// <returns></returns>
        private MemberAccount ValidateUser(string account, string password)
        {
            //確認帳號是否存在
            MemberAccount member = db.MemberAccounts.FirstOrDefault(m => m.Account == account);

            if (member == null)
            {
                return null;
            }

            //確認密碼是否正確
            //資料庫資料
            string dbPassword = member.Password;
            string salt = member.Salt;
            //產生雜湊密碼
            var hashPassword = Utility.GenerateHashWithSalt(password, salt);

            if (hashPassword != dbPassword)
            {
                return null;
            }
            return member;
        }

        [Authorize]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }

        // GET: Front/Member/Register
        public ActionResult Register()
        {
            var model = new MemberRegisterViewModel
            {
                MemberServiceExps = new List<MemberServiceExp>
                {
                    new MemberServiceExp(),
                    new MemberServiceExp(),
                    new MemberServiceExp()
                }
            };
            return View(model);
        }

        // POST: Front/Member/Register
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(MemberRegisterViewModel model)
        {
            IRecaptcha<RecaptchaV2Result> recaptcha = new RecaptchaV2(new RecaptchaV2Data()
            {
                Secret = ConfigurationManager.AppSettings["RecaptchaSecret"]
            });

            var result = recaptcha.Verify();

            if (!result.Success)
            {
                ModelState.AddModelError("ReCaptcha", "請勾選「我不是機器人」");
                return View(model);
            }

            // 檢查帳號是否重複
            if (db.MemberAccounts.Any(m => m.Account == model.Account))
            {
                ModelState.AddModelError("Account", "此帳號已被註冊");
                return View(model);
            }

            // 檢查是否為國際會員
            if (model.IsInternationalMember && model.FileUpload == null)
            {
                ModelState.AddModelError("FileUpload", "請上傳有效會員證影本");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                //建立帳號密碼
                string salt = Utility.CreateSalt();

                MemberAccount memberAccount = new MemberAccount
                {
                    //建立帳號密碼
                    Account = model.Account,
                    Salt = salt,
                    Password = Utility.GenerateHashWithSalt(model.Password, salt),
                    CreatedDate = DateTime.Now
                };
                db.MemberAccounts.Add(memberAccount);
                db.SaveChanges(); // 先存入資料庫取得 Id

                Models.MemberInfo memberInfo = new Models.MemberInfo
                {
                    Name = model.Name,
                    Gender = model.Gender.Value,
                    BirthDate = model.BirthDate,
                    MembershipType = model.MembershipType,
                    Phone = model.Phone,
                    Mobile = model.Mobile,
                    Address = model.Address,
                    Email = model.Email,
                    IsInternationalMember = model.IsInternationalMember,
                    CurrentCompany = model.CurrentCompany,
                    CurrentJobTitle = model.CurrentJobTitle,
                    HighestEducation = model.HighestEducation,
                    MemberId = memberAccount.Id,
                    TotalExpYears = model.TotalExpYears,
                    TotalExpMonths = model.TotalExpMonths
                };
                db.MemberInfoes.Add(memberInfo);
                db.SaveChanges();

                // 若是國際會員

                if (model.IsInternationalMember && model.FileUpload != null)
                {
                    // 處理檔案上傳
                    // 允許的副檔名（圖片 + 文件）
                    var allowedExtensions = new[] {
                        ".jpg", ".jpeg", ".png", ".gif",   // 圖片
                        ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx" // 文件
                    };

                    var extension = Path.GetExtension(model.FileUpload.FileName).ToLower();

                    if (!allowedExtensions.Contains(extension))
                    {
                        TempData["ErrorMessage"] = "檔案格式不正確，請上傳圖片或文件檔";
                        return View(model);
                    }

                    string folder = Server.MapPath("~/Uploads/InternationalMembers/");
                    // 如果資料夾不存在就建立
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    var fileName = System.IO.Path.GetFileName(model.FileUpload.FileName);
                    string fullPath = System.IO.Path.Combine(folder, fileName);
                    try
                    {
                        model.FileUpload.SaveAs(fullPath);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "檔案上傳失敗: " + ex.Message);
                        return View(model);
                    }
                    // 存到資料庫
                    memberInfo.FilePath = "/Uploads/InternationalMembers/" + fileName;
                    db.SaveChanges();
                }


                foreach (var exp in model.MemberServiceExps)
                {
                    // 判斷是否有填寫資料（至少公司或職稱有填）
                    if (string.IsNullOrWhiteSpace(exp.Company) && string.IsNullOrWhiteSpace(exp.JobTitle))
                    {
                        continue; // 跳過這筆，避免新增空資料
                    }

                    MemberServiceExp memberServiceExp = new MemberServiceExp
                    {
                        MemberId = memberInfo.Id,
                        Company = exp.Company,
                        JobTitle = exp.JobTitle,
                        StartYear = exp.StartYear,
                        StartMonth = exp.StartMonth,
                        EndYear = exp.EndYear,
                        EndMonth = exp.EndMonth,
                        CreatedDate = DateTime.Now,
                    };
                    db.MemberServiceExps.Add(memberServiceExp);
                }
                db.SaveChanges();

                TempData["SuccessMessage"] = "註冊成功！";

                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Index", "MemberDiscussion", new { area = "Front" });

                return View(model);
            }
            else
            {
                TempData["ErrorMessage"] = "註冊失敗，請檢查輸入的資料。";
            }

            return View(model);
        }

        // GET: Front/Member/Edit
        [Authorize]
        public ActionResult Edit()
        {
            // 取得登入者帳號
            var accountName = User.Identity.Name; // ASP.NET Identity 的登入名稱
            var memberInfo = db.MemberInfoes
                .Include("MemberAccount")
                .Include("MemberServiceExps")
                .FirstOrDefault(m => m.MemberAccount.Account == accountName);
            if (memberInfo == null)
                return HttpNotFound();

            //轉成MemberEditViewModel
            var viewModel = new MemberEditViewModel
            {
                Id = memberInfo.Id,
                Account = memberInfo.MemberAccount?.Account,
                Name = memberInfo.Name,
                Gender = memberInfo.Gender,
                BirthDate = memberInfo.BirthDate,
                MembershipType = memberInfo.MembershipType,
                Phone = memberInfo.Phone,
                Mobile = memberInfo.Mobile,
                Address = memberInfo.Address,
                Email = memberInfo.Email,
                IsInternationalMember = memberInfo.IsInternationalMember,
                CurrentCompany = memberInfo.CurrentCompany,
                CurrentJobTitle = memberInfo.CurrentJobTitle,
                HighestEducation = memberInfo.HighestEducation,
                TotalExpYears = memberInfo.TotalExpYears,
                TotalExpMonths = memberInfo.TotalExpMonths,
                MemberServiceExps = memberInfo.MemberServiceExps.Select(exp => new MemberServiceExp
                {
                    Id = exp.Id,
                    MemberId = exp.MemberId,
                    Company = exp.Company,
                    JobTitle = exp.JobTitle,
                    StartYear = exp.StartYear,
                    StartMonth = exp.StartMonth,
                    EndYear = exp.EndYear,
                    EndMonth = exp.EndMonth
                }).ToList()
            };

            // 補足空的 service experiences 到 3 筆
            while (viewModel.MemberServiceExps.Count < 3)
            {
                viewModel.MemberServiceExps.Add(new MemberServiceExp());
            }

            return View(viewModel);
        }

        // POST: Front/Member/Edit
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MemberEditViewModel model, string newPassword)
        {
            var memberInfo = db.MemberInfoes.Find(model.Id);
            if (memberInfo == null)
            {
                TempData["ErrorMessage"] = "找不到會員資料";
                return View(model);
            }  

            // 取出會員帳號與會員資料
            var memberAccount = db.MemberAccounts.Find(memberInfo.MemberId);
            if (memberAccount == null)
            {
                TempData["ErrorMessage"] = "找不到帳號資料";
                return View(model);
            }

            if (!ModelState.IsValid)
            {
                TempData["ErrorMessage"] = "請確認欄位是否正確填寫。";
                return View(model);
            }

            // 更新密碼 (若有輸入新密碼)
            if (!string.IsNullOrWhiteSpace(newPassword))
            {
                memberAccount.Password = Utility.GenerateHashWithSalt(newPassword, memberAccount.Salt);       
            }

            // 更新帳號資料
            memberAccount.Account = model.Account;
            memberAccount.UpdatedDate = DateTime.Now;

            // 更新會員基本資料
            memberInfo.Name = model.Name;
            memberInfo.Gender = model.Gender.Value;
            memberInfo.BirthDate = model.BirthDate.Value;
            memberInfo.MembershipType = model.MembershipType;
            memberInfo.Phone = model.Phone;
            memberInfo.Mobile = model.Mobile;
            memberInfo.Address = model.Address;
            memberInfo.Email = model.Email;
            memberInfo.IsInternationalMember = model.IsInternationalMember;
            memberInfo.CurrentCompany = model.CurrentCompany;
            memberInfo.CurrentJobTitle = model.CurrentJobTitle;
            memberInfo.HighestEducation = model.HighestEducation;
            memberInfo.TotalExpYears = model.TotalExpYears;
            memberInfo.TotalExpMonths = model.TotalExpMonths;

            // 處理國際會員檔案
            if (model.IsInternationalMember && model.FileUpload != null)
            {
                // 允許的副檔名（圖片 + 文件）
                var allowedExtensions = new[] {
                        ".jpg", ".jpeg", ".png", ".gif",   // 圖片
                        ".pdf", ".doc", ".docx", ".xls", ".xlsx", ".ppt", ".pptx" // 文件
                    };

                var extension = Path.GetExtension(model.FileUpload.FileName).ToLower();

                if (!allowedExtensions.Contains(extension))
                {
                    TempData["ErrorMessage"] = "檔案格式不正確，請上傳圖片或文件檔";
                    return View(model);
                }

                string folder = Server.MapPath("~/Uploads/InternationalMembers/");
                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Path.GetFileName(model.FileUpload.FileName);
                string fullPath = Path.Combine(folder, fileName);
                model.FileUpload.SaveAs(fullPath);
                model.FilePath = "/Uploads/InternationalMembers/" + fileName;
                memberInfo.FilePath = model.FilePath;
            }

            // 取得原本的服務經歷列表
            var existingExps = db.MemberServiceExps.Where(e => e.MemberId == memberInfo.Id).ToList();

            // 更新服務經歷
            foreach (var expVM in model.MemberServiceExps)
            {
                bool empty = string.IsNullOrWhiteSpace(expVM.Company)
                          && string.IsNullOrWhiteSpace(expVM.JobTitle);

                // -------------------------------------------
                // 1. 使用者沒有填任何內容 → 不做動作
                // -------------------------------------------
                if (empty)
                {
                    // 若是空白但原本有資料 → 刪除
                    var original = existingExps.FirstOrDefault(x => x.Id == expVM.Id);
                    if (original != null)
                    {
                        db.MemberServiceExps.Remove(original);
                    }
                    continue;
                }

                // -------------------------------------------
                // 2. 有 ID → 修改原資料
                // -------------------------------------------
                if (expVM.Id > 0)
                {
                    var original = existingExps.FirstOrDefault(x => x.Id == expVM.Id);
                    if (original != null)
                    {
                        original.Company = expVM.Company;
                        original.JobTitle = expVM.JobTitle;
                        original.StartYear = expVM.StartYear;
                        original.StartMonth = expVM.StartMonth;
                        original.EndYear = expVM.EndYear;
                        original.EndMonth = expVM.EndMonth;
                    }
                }
                else
                {
                    // -------------------------------------------
                    // 3. ID == 0 且 有填內容 → 新增
                    // -------------------------------------------
                    var newExp = new MemberServiceExp
                    {
                        MemberId = memberInfo.Id,
                        Company = expVM.Company,
                        JobTitle = expVM.JobTitle,
                        StartYear = expVM.StartYear,
                        StartMonth = expVM.StartMonth,
                        EndYear = expVM.EndYear,
                        EndMonth = expVM.EndMonth
                    };
                    db.MemberServiceExps.Add(newExp);
                }
            }

            // 儲存至資料庫
            try
            {
                db.Entry(memberAccount).State = EntityState.Modified;
                db.Entry(memberInfo).State = EntityState.Modified;

                db.SaveChanges();
                TempData["SuccessMessage"] = "修改成功！";
                ViewBag.RedirectUrl = Url.Action("Index", "MemberDiscussion", new { area = "Front" });
            }
            catch (DbEntityValidationException ex)
            {
                // 列出 EF 驗證錯誤
                foreach (var eve in ex.EntityValidationErrors)
                {
                    foreach (var ve in eve.ValidationErrors)
                    {
                        ModelState.AddModelError(ve.PropertyName, ve.ErrorMessage);
                    }
                }
                TempData["ErrorMessage"] = "修改失敗，請檢查欄位資料";
                return View(model);
            }

            return View(model);

        }
    }
}