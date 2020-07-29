using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace blog.Controllers
{
    [RequireHttps]
    public class CaptchaController : Controller
    {
        // GET: Captcha
        public ActionResult Index()
        {
            return View();
        }
    }
}