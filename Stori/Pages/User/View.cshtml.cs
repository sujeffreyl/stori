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

        public Stori.ObjectModel.User User { get; set; }

        public ViewModel()
        {
            Posts = new List<Post>();
        }

        public void OnGet()
        {
            this.Posts = db.GetPostsByUsername(RouteData.Values["username"].ToString());

            if (this.Posts.Any())
            {
                var nonBlankUserProfiles = this.Posts.Where(post => !String.IsNullOrEmpty(post.Author.ProfilePictureUrl));
                if (nonBlankUserProfiles.Any())
                {
                    this.User = nonBlankUserProfiles.First().Author;
                }
                else
                {
                    var firstPost = this.Posts.First();
                    this.User = firstPost.Author;
                }

                if (String.IsNullOrEmpty(this.User.BannerImageUrl))
                    this.User.BannerImageUrl = "https://images.unsplash.com/photo-1486427944299-d1955d23e34d?ixlib=rb-0.3.5&ixid=eyJhcHBfaWQiOjEyMDd9&s=07df4ad552d3627b7cbc7836321d9a03&auto=format&fit=crop&w=1350&q=80";

                if (String.IsNullOrEmpty(this.User.ProfilePictureUrl))
                    this.User.ProfilePictureUrl = "https://images.unsplash.com/photo-1522075469751-3a6694fb2f61?ixlib=rb-0.3.5&ixid=eyJhcHBfaWQiOjEyMDd9&s=3ccc1801fd91dc9cf869fa6a09360c09&auto=format&fit=crop&w=1000&q=80";
            }
        }
    }
}