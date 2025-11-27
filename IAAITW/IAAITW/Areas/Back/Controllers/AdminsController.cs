using AngleSharp.Text;
using GoogleRecaptcha;
using IAAITW.Filter;

//using GoogleRecaptchaMvc;
using IAAITW.Models;
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
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace IAAITW.Areas.Back.Controllers
{
    public class AdminsController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Back/Admins/Register
        public ActionResult Register()
        {
            return View(new AdminRegisterViewModel());
        }

        // POST: Front/Admins/Register
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(AdminRegisterViewModel model)
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
            if (db.Admins.Any(m => m.Account == model.Account))
            {
                ModelState.AddModelError("Account", "此帳號已被註冊");
                return View(model);
            }

            if (ModelState.IsValid)
            {
                //建立帳號密碼
                string salt = Utility.CreateSalt();

                Admin admin = new Admin
                {
                    //建立帳號密碼
                    Account = model.Account,
                    PasswordSalt = salt,
                    Password = Utility.GenerateHashWithSalt(model.Password, salt),
                    Name = model.Name,
                    Email = model.Email,
                    CreatedDate = DateTime.Now,
                    UpdatedDate = DateTime.Now
                };

                db.Admins.Add(admin);
                db.SaveChanges();

                TempData["SuccessMessage"] = "註冊成功！";

                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Login", "Admins", new { area = "Back" });

                return View(model);
            }
            else
            {
                TempData["ErrorMessage"] = "註冊失敗，請檢查輸入的資料。";
            }

            return View(model);
        }

        // GET: Back/Admins
        public ActionResult Login()
        {
            // 如果已經登入
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Abouts", new { area = "Back" });
            }

            return View();
        }

        // POST: Back/Admins/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(AdminLoginViewModel model)
        {
            // 如果已經登入
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Abouts", new { area = "Back" });
            }

            if (ModelState.IsValid)
            {
                // 驗證帳號密碼
                var user = ValidateUser(model.Account, model.Password);

                if (user == null)
                {
                    // 帳號或密碼錯誤
                    TempData["ErrorMessage"] = "帳號或密碼錯誤。";
                    return View(model);
                }
                // 登入成功，產生表單驗證
                string userData = JsonConvert.SerializeObject(user);
                Utility.SetAuthenTicket(userData, model.Account);

                TempData["SuccessMessage"] = "登入成功！";

                // 把要導向的 URL 給 View 跳轉到關於我們頁面
                ViewBag.RedirectUrl = Url.Action("Index", "Abouts", new { area = "Back" });

                return View(model);
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
        private Admin ValidateUser(string account, string password)
        {
            //確認帳號是否存在
            Admin admin = db.Admins.FirstOrDefault(m => m.Account == account);

            if (admin == null)
            {
                return null;
            }

            //確認密碼是否正確
            //資料庫資料
            string dbPassword = admin.Password;
            string salt = admin.PasswordSalt;
            //產生雜湊密碼
            var hashPassword = Utility.GenerateHashWithSalt(password, salt);

            if (hashPassword != dbPassword)
            {
                return null;
            }
            return admin;
        }

        [CustomAuthorize(LoginUrl = "~/Back/Admins/Login")]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            // 放入一次性提示訊息
            TempData["LogoutMessage"] = "登出成功！";
            return RedirectToAction("Login", "Admins", new { area = "Back" });
        }
    }
}
