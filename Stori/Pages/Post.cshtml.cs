using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stori.ObjectModel;

namespace Stori.Pages
{
    public class PostModel : PageModel
    {
        private Stori.DataAccessLayer.Dal db = Stori.DataAccessLayer.Dal.Instance;

        public Post Post { get; set; }

        public PostModel()
        {
            Post = new Post();
        }

        public void OnGet()
        {
            this.Post = db.GetPostById(RouteData.Values["id"].ToString());
        }
    }
}
