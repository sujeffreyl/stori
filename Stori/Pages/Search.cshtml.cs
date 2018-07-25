using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stori.ObjectModel;

namespace Stori.Pages
{
    public class SearchModel : PageModel
    {
        private Stori.DataAccessLayer.Dal db = Stori.DataAccessLayer.Dal.Instance;

        public List<Post> Results { get; set; }

        public SearchModel()
        {
            Results = new List<Post>();
        }

        public void OnGet()
        {
            this.Results = db.GetPostsByTag(HttpContext.Request.Query["q"].ToString());
        }
    }
}
