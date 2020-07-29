using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace blog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string AuthorId { get; set; }
        [AllowHtml]
        public string Body { get; set; }
        public DateTimeOffset Created { get; set; }
        public DateTimeOffset? Update { get; set; }
        public string UpdateReason { get; set; }
        public virtual ApplicationUser Author { get; set; }
        public virtual BlogPost Post { get; set; }


    }
}