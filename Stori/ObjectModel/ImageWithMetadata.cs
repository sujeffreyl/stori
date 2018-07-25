using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stori.ObjectModel
{
    public class ImageWithMetadata
    {
        public MongoDB.Bson.ObjectId ImageId;
        public string Caption { get; set; }
        public DateTime UploadDate { get; set; }

        public ImageFormat Filetype { get; set; }

        public async Task SaveChangesAsync()
        {
            var dal = Stori.DataAccessLayer.Dal.Instance;
            var db = dal.GetMongoDb();
            var collection = db.GetCollection<ImageWithMetadata>("ImageWithMetadataList");
            await collection.InsertOneAsync(this);
        }
    }
}
