using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace blog.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string AuthorId { get; set; }
        public string Body{ get; set; }
            public DateTimeOffset Created { get; set; }
            public DateTimeOffset? Update { get; set; }
        public string UpdateReason { get; set; }
        public virtual ApplicationUser Autor { get; set; }


    }
}