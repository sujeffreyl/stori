using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stori.ObjectModel
{
    // This class should be as simple as possible so that we can fit as many bytes into the 16 MB limit
    public class Image : IMongoIdentifiable
    {
        public MongoDB.Bson.ObjectId _id { get; set; }

        public byte[] Content { get; set; }

        public Image(byte[] content)
        {
            _id = new MongoDB.Bson.ObjectId();
            this.Content = content;
        }

        // todo: probably unnecessary, just use imageWithmetadata instead
        public async Task SaveChangesAsync()
        {
            var dal = Stori.DataAccessLayer.Dal.Instance;
            var db = dal.GetMongoDb();
            var collection = db.GetCollection<Image>("images");
            await collection.InsertOneAsync(this);
        }
    }
}
