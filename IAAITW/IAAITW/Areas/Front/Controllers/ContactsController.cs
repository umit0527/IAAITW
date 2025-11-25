using IAAITW.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using GoogleRecaptcha;

namespace IAAITW.Areas.Front.Controllers
{
    public class ContactsController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Front/Contacts/Index
        public ActionResult Index()
        {
            return View();
        }

        // POST: Front/Contacts/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Contact contact)
        {
            IRecaptcha<RecaptchaV2Result> recaptcha = new RecaptchaV2(new RecaptchaV2Data()
            {
                Secret = ConfigurationManager.AppSettings["RecaptchaSecret"]
            });

            var result = recaptcha.Verify();

            if (!result.Success)
            {
                ModelState.AddModelError("ReCaptcha", "請勾選「我不是機器人」");
                return View(contact);
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // 先存入資料庫
                    db.Contacts.Add(contact);
                    db.SaveChanges();

                    // 非同步寄信給企業端
                    try
                    {
                        await SendEmailAsync(
                            toEmail: "umit0527@gmail.com",
                            subject: $"新的諮詢信件",
                            body: $"姓名: {contact.Name}<br/>" +
                            $"性別: {contact.Gender}<br/>" +
                            $"電話: {contact.PhoneNumber}<br/>" +
                            $"Email: {contact.Email}<br/>" +
                            $"諮詢標題: {contact.Title}<br/>" +
                            $"諮詢內容:<br/>{contact.Content}"
                        );
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("企業端寄信失敗：" + ex.Message);
                        TempData["ToastMessage"] = "訊息已送出，但企業端郵件通知失敗";
                        TempData["ToastType"] = "warning";
                    }

                    // 非同步寄信給使用者
                    try
                    {
                        await SendEmailAsync(
                            toEmail: contact.Email,
                            subject: "我們已收到您的聯絡表單",
                            body: $"親愛的 {contact.Name} 您好，<br/><br/>我們已收到您的訊息，內容如下：<br/>{contact.Content}<br/><br/>我們會盡快與您聯絡，謝謝！"
                        );
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("客戶端寄信失敗：" + ex.Message);
                        TempData["ToastMessage"] = "訊息已送出，但客戶端郵件通知失敗";
                        TempData["ToastType"] = "warning";
                    }

                    TempData["ToastMessage"] = "訊息已經送出，我們會盡快與您聯絡！";
                    TempData["ToastType"] = "success";
                    return RedirectToAction("Create");
                }
                catch (Exception)
                {
                    TempData["ToastMessage"] = "訊息送出失敗，請確認必填欄位是否正確填寫";
                    TempData["ToastType"] = "danger";
                    return View(contact);
                }
            }
            else
            {
                TempData["ToastMessage"] = "訊息送出失敗，請檢查欄位是否正確！";
                TempData["ToastType"] = "danger";
            }

            return View(contact);
        }

        // 非同步寄信方法
        private async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            var smtpHost = ConfigurationManager.AppSettings["SmtpHost"];
            var smtpPort = int.Parse(ConfigurationManager.AppSettings["SmtpPort"]);
            var smtpUser = ConfigurationManager.AppSettings["SmtpUser"];
            var smtpPass = ConfigurationManager.AppSettings["SmtpPass"];

            using (var message = new MailMessage())
            {
                message.From = new MailAddress(smtpUser, "網站聯絡表單");
                message.To.Add(toEmail);
                message.Subject = subject;
                message.Body = body;
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient(smtpHost, smtpPort))
                {
                    smtp.Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass);
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                }
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
