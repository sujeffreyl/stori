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
            ImageWithMetadata imageWithMetadata = await ImageWithMetadata.LookupById(imageId);
            
            if (imageWithMetadata == null)
            {
                return RedirectToPage("./Error");
            }

            Image image = await DataAccessLayer.Dal.LookupById<Image>("images", imageWithMetadata.ImageId);

            //Response.Body = new MemoryStream(image.Content);
            //Response.Body.Close();
            //Response.ContentLength = image.Content.Length;
            //Response.Headers.TryAdd("content-disposition", "attachment");    // TODO: FIX ME

            //switch (imageWithMetadata.Filetype)
            //{
            //    case ImageFormat.GIF:
            //        Response.ContentType = "image/gif";
            //        break;
            //    case ImageFormat.PNG:
            //        Response.ContentType = "image/png";
            //        break;
            //    case ImageFormat.JPEG:
            //    case ImageFormat.Unknown:
            //    default:
            //        Response.ContentType = "image/jpeg";
            //        break;
            //}


            //string body = "Hello world!";
            //Response.Body = new MemoryStream();
            //using (var writer = new StreamWriter(Response.Body))
            //{
            //    writer.Write(body);
            //}
            //Response.Body.Close();
            //Response.ContentLength = body.Length;
            //Response.ContentType = "text";

            var result = new FileContentResult(image.Content, "image/jpeg");
            return result;
        }
    }
}