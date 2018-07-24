using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stori.ObjectModel;

namespace Stori.Pages
{
    public class PostModel : PageModel, IDisposable
    {
        private Stori.DataAccessLayer.Dal db = Stori.DataAccessLayer.Dal.Instance;
        private bool disposed = false;

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAddAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            string username = "testaccount";    // TODO: FIX ME

            Post post = new Post();
            post.Text = Request.Form["postText"];
            post.Title = Request.Form["title"];
            post.CreateDate = DateTime.Now;
            post.Author = new ObjectModel.User(username);
            SaveChanges(post);

            await Task.Delay(0);
            return RedirectToPage("./User/View/", new { username = username } );
        }

        public bool SaveChanges(Post post)
        {
            db.CreatePost(post);
            return true;
        }




        #region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.db.Dispose();
                }
            }

            this.disposed = true;
        }

        # endregion

    }
}