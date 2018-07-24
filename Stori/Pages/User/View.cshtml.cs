using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stori.ObjectModel;

namespace Stori.Pages.User
{
    public class ViewModel : PageModel
    {
        private Stori.DataAccessLayer.Dal db = Stori.DataAccessLayer.Dal.Instance;

        public List<Post> Posts { get; set; }

        public ViewModel()
        {
            Posts = new List<Post>();
        }

        public void OnGet()
        {
            //this.Posts = db.GetAllPosts();
            this.Posts = db.GetPostsByUsername(RouteData.Values["username"].ToString());
            //Post testPost = new Post();
            //testPost.Title = "Hard-coded Title";
            //testPost.CreateDate = DateTime.Now;
            //testPost.Text = "Hard-coded value";
            //this.Posts.Add(testPost);
        }
    }
}