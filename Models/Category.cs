using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace blog.Models
{
	public class Category
	{
		public int Id { get; set; }

		public string Name { get; set; }
		public string Description { get; set; }
		////nav
		//public virtual ICollection<BlogPost> BlogPosts { get; set; }
		//public Category(){
		//	BlogPosts = new HashSet<BlogPost>();
		//}
	}
}