using GoogleRecaptcha;
using IAAITW.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace IAAITW.Areas.Front.Controllers
{
    public class MemberController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Front/Member
        [HttpGet]
        public ActionResult Index()
        {

            return View();
        }

        // POST: Front/Member
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(MemberAccount model) 
        {
            return View();
        }

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
                var user = db.MemberAccounts.FirstOrDefault(
                             m => m.Account == model.Account && 
                             m.Password == model.Password);

                if (user != null)
                {
                    // 登入成功，設定 Session 
                    Session["Member"] = user;

                    // 跳轉到會員首頁或指定頁
                    return RedirectToAction("Index", "Member");
                }
                else
                {
                    // 帳號或密碼錯誤
                    ModelState.AddModelError("", "帳號或密碼錯誤");
                }
            }

            // 驗證失敗或登入失敗，回傳原頁面
            return View(model);
        }

        // GET: Front/Member/Register
        public ActionResult Register()
        {
            var model = new MemberRegisterViewModel
            {
                ServiceExperiences = new List<ServiceExpViewModel>
                {
                    new ServiceExpViewModel(),
                    new ServiceExpViewModel(),
                    new ServiceExpViewModel()
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

            if (ModelState.IsValid)
            {
                MemberAccount memberAccount = new MemberAccount
                {
                    Account = model.Account,
                    Password = model.Password,
                    Salt = model.Salt,
                    IsActive = model.IsActive,
                    CreatedAt = model.CreatedAt
                };
                db.MemberAccounts.Add(memberAccount);
                db.SaveChanges(); // 先存入資料庫取得 Id

                MemberInfo memberInfo = new MemberInfo
                {
                    Name = model.Name,
                    Gender= model.Gender.Value,
                    BirthDate = model.BirthDate,
                    MembershipType= model.MembershipType,
                    Phone = model.Phone,
                    Mobile = model.Mobile,
                    Address = model.Address,
                    Email = model.Email,
                    InternationalMember = model.InternationalMember,
                    CurrentCompany = model.CurrentCompany,
                    CurrentJobTitle = model.CurrentJobTitle,
                    HighestEducation = model.HighestEducation
                };
                db.MemberInfoes.Add(memberInfo);

                // 若是國際會員，處理檔案上傳
                if (model.InternationalMember && model.InternationalFile != null)
                {
                    string folder = Server.MapPath("~/Uploads/InternationalMembers/");
                    // 如果資料夾不存在就建立
                    if (!Directory.Exists(folder))
                        Directory.CreateDirectory(folder);

                    var fileName = System.IO.Path.GetFileName(model.InternationalFile.FileName);
                    string fullPath = System.IO.Path.Combine(folder, fileName);
                    try
                    {
                        model.InternationalFile.SaveAs(fullPath);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", "檔案上傳失敗: " + ex.Message);
                        return View(model);
                    }
                    // 存到資料庫
                    model.FilePath = "/Uploads/InternationalMembers/" + fileName;
                }
                foreach (var exp in model.ServiceExperiences)
                {
                    MemberServiceExp memberServiceExp = new MemberServiceExp
                    {
                        Company = model.CurrentCompany,
                        JobTitle = model.CurrentJobTitle,
                        StartYear = DateTime.Now.Year,
                        StartMonth = DateTime.Now.Month,
                        TotalYears = model.TotalYears,
                        TotalMonths = model.TotalMonths
                    };
                    db.MemberServiceExps.Add(memberServiceExp);
                }

                db.SaveChanges();
                return RedirectToAction("Create");
            }

            return View(model);
        }
    }
}
