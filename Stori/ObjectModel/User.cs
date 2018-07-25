using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stori.ObjectModel
{
    public class User
    {
        public MongoDB.Bson.ObjectId _id;

        public string UserName { get; set; }

        public string ProfilePictureUrl { get; set; }

        public string BannerImageUrl { get; set; }

        public string Occupation { get; set; }

        public string Interests { get; set; }

        public string Location { get; set; }

        public User(string username)
        {
            this.UserName = username;
        }
    }
}
