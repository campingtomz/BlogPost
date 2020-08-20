using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace blog.Models
{
	public class Category
	{
		public int Id { get; set; }
		public virtual ICollection<BlogPost> blogPosts {get; set;}
		public string Name { get; set; }
		public string Description { get; set; }

		public Category()
        {
			blogPosts = new HashSet<BlogPost>();
		}
		////nav
		//public virtual ICollection<BlogPost> BlogPosts { get; set; }
		//public Category(){
		//	BlogPosts = new HashSet<BlogPost>();
		//}
	}
}