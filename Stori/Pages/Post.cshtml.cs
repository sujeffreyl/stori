using System;
using System.Collections.Generic;
using System.Drawing;
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

        public async Task<ActionResult> OnPostAddAsync(Microsoft.AspNetCore.Http.IFormFile file)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }


            // TODO: Get the username for real.
            string username = Request.Form["usernameTemp"];
            if (String.IsNullOrEmpty(username))
                username = "testaccount2";

            Post post = new Post();
            post.Text = Request.Form["postText"];
            post.Title = Request.Form["title"];
            post.Tags = Request.Form["tags"].ToString().Split(',').Select(s => s.Trim().ToLowerInvariant()).ToArray();
            post.CreateDate = DateTime.Now;
            post.Author = new ObjectModel.User(username);

            if (file != null)
            {
                ImageWithMetadata captionedImage = new ImageWithMetadata();
                captionedImage.Caption = "testCaption";
                captionedImage.UploadDate = DateTime.Now;
                if (file.FileName.EndsWith(".png"))
                {
                    captionedImage.Filetype = ImageFormat.PNG;
                }
                else if (file.FileName.EndsWith(".gif"))
                {
                    captionedImage.Filetype = ImageFormat.GIF;
                }
                else
                {
                    captionedImage.Filetype = ImageFormat.JPEG;
                }

                var image = System.Drawing.Image.FromStream(file.OpenReadStream());
                using (MemoryStream mStream = new MemoryStream())
                {
                    image.Save(mStream, image.RawFormat);
                    byte[] bytes = mStream.ToArray();

                    var imageInDb = new Stori.ObjectModel.Image(bytes);
                    await imageInDb.SaveChangesAsync();
                    captionedImage.ImageId = imageInDb._id;

                    captionedImage.Width = image.Width;
                    captionedImage.Height = image.Height;
                }

                await captionedImage.SaveChangesAsync();

                post.CaptionedImages = new ImageWithMetadata[] { captionedImage };  // TODO: Alternatively, just store the identifiers
            }
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