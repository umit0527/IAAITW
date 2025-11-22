using Ganss.Xss;
using IAAITW.Models;
using Microsoft.Ajax.Utilities;
using MvcPaging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Drawing.Printing;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Services.Description;
using System.Web.UI;

namespace IAAITW.Areas.Front.Controllers
{
    [Authorize]
    public class MemberDiscussionController : Controller
    {
        private DBMdoelContext db = new DBMdoelContext();

        // GET: Front/MemberDiscussion
        public ActionResult Index(int? page)
        {
            //指令化
            var memberDiscussion = db.MemberDiscussionPosts.AsQueryable();

            //一頁幾筆資料
            var pageSize = 5;

            //目前第幾頁
            ///避免page是null的時候
            ///page-1是為了與後端的值對齊
            ///當前端是第一頁 value=1、後端value應該要是0，從0開始計算
            if (page.HasValue)
            {
                page = page - 1;
            }
            else
            {
                page = 0;
            }

            //用套件一定要有 orderby 排序            
            var result = memberDiscussion
                .OrderByDescending(p => p.CreatedDate)
                .Select(p => new DiscussionListViewModel
                {
                    PostId = p.Id,
                    Title = p.Title,
                    PosterName = p.MemberInfo.Name,
                    CreatedDate = p.CreatedDate,
                    LatestReplierName = p.Replies
                        .OrderByDescending(r => r.CreatedDate)
                        .Select(r => r.MemberInfo.Name)
                        .FirstOrDefault(),
                    LatestReplyDate = p.Replies
                        .OrderByDescending(r => r.UpdatedDate)
                        .Select(r => (DateTime?)r.UpdatedDate)
                        .FirstOrDefault(),
                    ReplyCount = p.Replies.Count()
                }).ToPagedList(page.Value, pageSize);

            return View(result);
        }

        // GET: Front/MemberDiscussion/Details/5
        public ActionResult Details(int? id, int? page)
        {
            if (id == null)
                return HttpNotFound();

            // 查詢文章（
            var post = db.MemberDiscussionPosts
                .Include(p => p.MemberInfo)
                .FirstOrDefault(p => p.Id == id);

            if (post == null)
                return HttpNotFound();

            // 分頁設定
            int pageSize = 5;
            // 0-Based 頁碼
            int zeroBasedPageNumber = (page.HasValue && page.Value > 0) ? (page.Value - 1) : 0;

            // 建立 IQueryable 查詢
            // 不再需要 .AsQueryable()，因為 db.MemberDiscussionReplies 預設就是 IQueryable
            var queryReplies = db.MemberDiscussionReplies
                .Where(r => r.PostId == id)
                .Include(r => r.MemberInfo)
                .OrderByDescending(r => r.CreatedDate)
                .Select(r => new ReplyViewModel
                {
                    ReplierName = r.MemberInfo.Name,
                    ReplyDate = r.CreatedDate,
                    ReplyContent = r.Content
                }).ToPagedList(zeroBasedPageNumber, pageSize);

            // 組成 ViewModel
            var model = new DiscussionContentViewModel
            {
                PostId = post.Id,
                Title = post.Title,
                PosterName = post.MemberInfo?.Name,
                CreatedDate = post.CreatedDate,
                Content = post.Content,
                Replies = queryReplies
            };

            return View(model);
        }

        // GET: Front/MemberDiscussion/Create
        public ActionResult Create()
        {
            ViewBag.PosterId = new SelectList(db.MemberAccounts, "Id", "Account");
            return View();
        }

        // POST: Front/MemberDiscussion/Create
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MemberDiscussionPost post)
        {
            if (ModelState.IsValid)
            {
                // 取得登入者帳號
                var accountName = User.Identity.Name; // ASP.NET Identity 的登入名稱
                // 先查出 account 表的 id
                var member = db.MemberAccounts.FirstOrDefault(m => m.Account == accountName);

                if (member == null)
                {
                    TempData["ErrorMessage"] = "找不到登入會員資訊。";
                    return View(post);
                }

                // 再查出 info 表的 id
                var memberInfo = db.MemberInfoes.FirstOrDefault(mi => mi.MemberId == member.Id);
                if (memberInfo == null)
                {
                    TempData["ErrorMessage"] = "找不到會員資訊。";
                    return View(post);
                }

                // 設定外鍵 PosterId
                post.PosterId = memberInfo.Id;


                // 安全過濾 HTML
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 保留顏色或字體
                post.Content = sanitizer.Sanitize(post.Content);

                db.MemberDiscussionPosts.Add(post);
                db.SaveChanges();

                TempData["SuccessMessage"] = "新增成功！";

                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Index");

                return View(post);
            }
            else
            {
                TempData["ErrorMessage"] = "新增失敗，請檢查輸入的資料。";
            }

            // ViewBag.PosterId = new SelectList(db.MemberAccounts, "Id", "Account", memberDiscussionPost.PosterId);

            return View(post);
        }

        // GET: Front/MemberDiscussion/CreateRe/5
        public ActionResult CreateRe(int? id)
        {
            if (id == null)
                return HttpNotFound();

            // 查詢主文章（用 id 來找）
            var post = db.MemberDiscussionPosts.FirstOrDefault(p => p.Id == id);
            if (post == null)
                return HttpNotFound();

            // 建立回覆模型，帶入主文章資訊
            var model = new MemberDiscussionReply
            {
                PostId = post.Id,
                Post = post   // ← 這行是關鍵，讓 View 可以顯示 Post.Title
            };

            return View(model);
        }

        // POST: Front/MemberDiscussion/CreateRe/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateRe(MemberDiscussionReply reply, int id)
        {
            if (ModelState.IsValid)
            {
                // 取得登入者帳號
                var accountName = User.Identity.Name; // ASP.NET Identity 的登入名稱
                
                // 先查出 account 表的 id
                var member = db.MemberAccounts.FirstOrDefault(m => m.Account == accountName);

                if (member == null)
                {
                    TempData["ErrorMessage"] = "找不到登入會員資訊。";
                    return View(reply);
                }

                // 再查出 info 表的 id
                var memberInfo = db.MemberInfoes.FirstOrDefault(mi => mi.MemberId == member.Id);
                if (memberInfo == null)
                {
                    TempData["ErrorMessage"] = "找不到會員資訊。";
                    return View(reply);
                }

                // 安全過濾 HTML
                var sanitizer = new HtmlSanitizer();
                sanitizer.AllowedAttributes.Add("style"); // 保留顏色或字體
                reply.Content = sanitizer.Sanitize(reply.Content);
                reply.PostId = id;
                reply.ReplierId=memberInfo.Id;

                db.MemberDiscussionReplies.Add(reply);
                db.SaveChanges();

                TempData["SuccessMessage"] = "新增成功！";

                // 把要導向的 URL 給 View
                ViewBag.RedirectUrl = Url.Action("Details", "MemberDiscussion", new { id = reply.PostId });

                return View(reply);
            }
            else
            {
                TempData["ErrorMessage"] = "新增失敗，請檢查輸入的資料。";
            }

            // ViewBag.PosterId = new SelectList(db.MemberAccounts, "Id", "Account", memberDiscussionPost.PosterId);

            return View(reply);
        }

        // GET: Front/MemberDiscussion/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MemberDiscussionPost memberDiscussionPost = db.MemberDiscussionPosts.Find(id);
            if (memberDiscussionPost == null)
            {
                return HttpNotFound();
            }
            ViewBag.PosterId = new SelectList(db.MemberAccounts, "Id", "Account", memberDiscussionPost.PosterId);
            return View(memberDiscussionPost);
        }

        // POST: Front/MemberDiscussion/Edit/5
        // 若要避免過量張貼攻擊，請啟用您要繫結的特定屬性。
        // 如需詳細資料，請參閱 https://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MemberDiscussionPost memberDiscussionPost)
        {
            if (ModelState.IsValid)
            {
                db.Entry(memberDiscussionPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.PosterId = new SelectList(db.MemberAccounts, "Id", "Account", memberDiscussionPost.PosterId);
            return View(memberDiscussionPost);
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
