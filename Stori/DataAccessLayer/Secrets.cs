using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stori.DataAccessLayer
{
    internal class Secrets
    {
        internal string UserName  { get; set; }
        internal string Host { get; set; }
        internal string Password { get; set; }

        internal Secrets()
        {
            this.UserName = "TODO";
            this.Host = "TODO";
            this.Password = "TODO";
        }
    }
}
