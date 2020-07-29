using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.IO;
using System.Web;
using System.Web.Mvc;
using blog.Helpers;
using blog.Models;
using blog.ViewModels;
using PagedList;
using PagedList.Mvc;

namespace blog.Controllers
{
    [RequireHttps]
    public class BlogPostsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
      

        // GET: BlogPosts
        public ActionResult Index(int? page, string searchStr)
        {
            ViewBag.Search = searchStr;
            var blogList = IndexSearch(searchStr);
            int pageSize = 10; //Specifies the number of posts per page
            int pageNumber = ( page ?? 1); //?? null coalescing operator
            var model = blogList.ToPagedList(pageNumber, pageSize);
            //var allBlogPosts = db.BlogPosts.OrderBy(b => b.Created).ToPagedList(pageNumber, pageSize);
            return View(model);

        }
        public IQueryable<BlogPost> IndexSearch(string searchStr)
        {
            IQueryable<BlogPost> result = null;
            if(searchStr != null)
            {
                result = db.BlogPosts.AsQueryable();
                result = result.Where(b => b.Title.Contains(searchStr) ||
                                           b.Body.Contains(searchStr) ||
                                           b.Comments.Any(c => c.Body.Contains(searchStr) ||
                                           c.Author.FirstName.Contains(searchStr) ||
                                           c.Author.LastName.Contains(searchStr) ||
                                           c.Author.DisplayName.Contains(searchStr) ||
                                           c.Author.Email.Contains(searchStr)));
            }
            else
            {
                result = db.BlogPosts.AsQueryable();
            }
            return result.OrderByDescending(p => p.Created);
        }

        // GET: BlogPosts/Details/5
        public ActionResult Details(string Slug)
        {
            var model = new BlogPostDetailsVM();
            if (String.IsNullOrWhiteSpace(Slug))
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            model.BlogPost = db.BlogPosts.Include(b => b.Comments).FirstOrDefault(p => p.Slug == Slug);
            var blogCategories = db.CategoryBlogPosts.Where(c => c.BlogPostId == model.BlogPost.Id).ToList();
            foreach(var category in blogCategories)
            {
                model.BlogPost.Categories.Add(db.Categories.Find(category.CategoryId));
            }
            if (model.BlogPost == null)
            {
                return HttpNotFound();
            }
            model.SidePosts = db.BlogPosts.ToList();
            return View(model);
        }


        // GET: BlogPosts/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewBag.CategoryIds = new MultiSelectList(db.Categories.ToList(), "Id", "Name");
            return View();
        }

        // POST: BlogPosts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Create([Bind(Include = "Title, Updated, Body, Abstract, Categories, Published")] BlogPost blogPost, HttpPostedFileBase image, List<int> categoryIds)
        {
            if (ModelState.IsValid)
            {
               
                var Slug = StringUtilities.URLFriendly(blogPost.Title);
                if (String.IsNullOrWhiteSpace(Slug))
                {
                    ModelState.AddModelError("Title", "Invalid title");
                    return View(blogPost);
                }
                if (db.BlogPosts.Any(p => p.Slug == Slug))
                {
                    ModelState.AddModelError("Title", "The title must be unique");
                    return View(blogPost);
                }
                
                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    var justFileName = Path.GetFileNameWithoutExtension(fileName);
                    justFileName = StringUtilities.URLFriendly(justFileName);
                    fileName = $"{justFileName}_{DateTime.Now.Ticks}{Path.GetExtension(fileName)}";
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    blogPost.MediaURL = "/Uploads/" + fileName;
                }
                blogPost.Slug = Slug;
                blogPost.Created = DateTime.Now;
                db.BlogPosts.Add(blogPost);
                db.SaveChanges();

                if (categoryIds != null && categoryIds.Count > 0)
                {
                    foreach (var categoryId in categoryIds)
                    {
                        db.CategoryBlogPosts.Add(new CategoryBlogPost
                        {
                            BlogPostId = blogPost.Id,
                            CategoryId = categoryId
                        });
                    }
                }
                db.SaveChanges();
                    return RedirectToAction("Index");
            }

            return View(blogPost);
        }

        // GET: BlogPosts/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public ActionResult Edit([Bind(Include = "Id,Created,Title,Body, Abstract,Categories,MediaURL,Published, Slug")] BlogPost blogPost, HttpPostedFileBase image)
        {
            if (ModelState.IsValid)
            {
                var Slug = StringUtilities.URLFriendly(blogPost.Title);
                if (Slug != blogPost.Slug)
                {
                    if (String.IsNullOrWhiteSpace(Slug))
                    {
                        ModelState.AddModelError("Title", "Invalid title");
                        return View(blogPost);
                    }
                    if (db.BlogPosts.Any(p => p.Slug == Slug))
                    {
                        ModelState.AddModelError("Title", "The title must be unique");
                        return View(blogPost);
                    }
                    blogPost.Slug = Slug;
                }

                if (ImageUploadValidator.IsWebFriendlyImage(image))
                {
                    var fileName = Path.GetFileName(image.FileName);
                    image.SaveAs(Path.Combine(Server.MapPath("~/Uploads/"), fileName));
                    blogPost.MediaURL = "/Uploads/" + fileName;
                }

                blogPost.Updated = DateTime.Now;
                db.Entry(blogPost).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(blogPost);
        }

        // GET: BlogPosts/Delete/5
        [Authorize(Roles = "Admin")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            BlogPost blogPost = db.BlogPosts.Find(id);
            if (blogPost == null)
            {
                return HttpNotFound();
            }
            return View(blogPost);
        }

        // POST: BlogPosts/Delete/5
        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            BlogPost blogPost = db.BlogPosts.Find(id);
            db.BlogPosts.Remove(blogPost);
            db.SaveChanges();
            return RedirectToAction("Index");
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
