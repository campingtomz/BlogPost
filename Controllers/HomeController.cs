using blog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace blog.Controllers
{
    [RequireHttps]
    public class HomeController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        public ActionResult Index()
        {
            var allBlogPosts = db.BlogPosts.Where(b => b.Published).ToList();//Where(b=> b.Published).
            return View(allBlogPosts);
        }

        public ActionResult About()
        {
            ViewBag.Message = "this is a test";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}