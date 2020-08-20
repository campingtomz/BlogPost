using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using blog.Models;
namespace blog.Helpers
{
    public class BlogPostHelper
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        public Category GetCategory(int id)
        {
            return db.Categories.Find(id);
        }
        public string PrintCategories(List<Category> categoriesList)
        {
            if(categoriesList.Count < 1)
            {
                return "";
            }
            if (categoriesList.Count == 1)
            {
                return categoriesList.First().Name;
            }
            else
            {
                return String.Join(", ", categoriesList.Select(c => c.Name));
            }
        }

    }
}