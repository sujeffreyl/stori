using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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

            // TODO: Implement uploading of images
            //post.Text = Request.GetMultipartBoun

            await Task.Delay(0);
            return RedirectToPage("./User/View/", new { username = username } );
        }

        public async Task<ActionResult> OnPostUploadAsync(Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ImageWithMetadata captionedImage = new ImageWithMetadata();
            captionedImage.Caption = "testCaption";
            captionedImage.UploadDate = DateTime.Now;
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                // TODO: Maybe we should read block by block later
                string content = await reader.ReadToEndAsync();
                byte[] bytes = Encoding.ASCII.GetBytes(content);
                var image = new Image(bytes);
                captionedImage.ImageId = image._id;
            }
            await captionedImage.SaveChangesAsync();

            string username = "testaccount2";

            Post post = new Post();
            post.Text = Request.Form["postText"];
            post.Title = Request.Form["title"];
            post.CreateDate = DateTime.Now;
            post.Author = new ObjectModel.User(username);

            post.CaptionedImages = new ImageWithMetadata[] { captionedImage };
            SaveChanges(post);

            return RedirectToPage("./User/View/", new { username = username });
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