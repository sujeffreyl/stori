using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Stori.ObjectModel;

namespace Stori.Pages
{
    public class NewUserModel : PageModel, IDisposable
    {
        private Stori.DataAccessLayer.Dal db = Stori.DataAccessLayer.Dal.Instance;
        private bool disposed = false;

        public void OnGet()
        {

        }

        public void OnPost()
        {

            RedirectToPage("./Search");
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