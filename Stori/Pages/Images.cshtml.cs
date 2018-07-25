using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stori.ObjectModel;

namespace Stori.Pages
{
    public class ImagesModel : PageModel
    {
        public async Task<ActionResult> OnGetAsync()
        {
            string imageId = RouteData.Values["imageId"].ToString();
            ImageWithMetadata imageWithMetadata = await DataAccessLayer.Dal.LookupById<ImageWithMetadata>(ImageWithMetadata.GetMongoDbCollectionName(), imageId);


            if (imageWithMetadata == null)
            {
                return RedirectToPage("./Error");
            }

            Image image = await DataAccessLayer.Dal.LookupById<Image>("images", imageWithMetadata.ImageId);

            string contentType;
            switch (imageWithMetadata.Filetype)
            {
                case ImageFormat.GIF:
                    contentType = "image/gif";
                    break;
                case ImageFormat.PNG:
                    contentType = "image/png";
                    break;
                case ImageFormat.JPEG:
                case ImageFormat.Unknown:
                default:
                    contentType = "image/jpeg";
                    break;
            }


            var result = new FileContentResult(image.Content, contentType);
            return result;
        }
    }
}