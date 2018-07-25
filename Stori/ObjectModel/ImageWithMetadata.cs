using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Stori.ObjectModel
{
    public class ImageWithMetadata : IMongoIdentifiable
    {
        public MongoDB.Bson.ObjectId _id { get; set; }

        public MongoDB.Bson.ObjectId ImageId { get; set; }

        public string Caption { get; set; }

        public DateTime UploadDate { get; set; }

        public ImageFormat Filetype { get; set; }

        public string[] MediaUrls { get; set; }

        // Implements IMongoIdentifiable
        public MongoDB.Bson.ObjectId Id()
        {
            return _id;
        }
        
        internal static string GetMongoDbCollectionName()
        {
            return "ImageWithMetadataList";
        }

        public async Task SaveChangesAsync()
        {
            var dal = Stori.DataAccessLayer.Dal.Instance;
            var db = dal.GetMongoDb();
            var collection = db.GetCollection<ImageWithMetadata>("ImageWithMetadataList");
            await collection.InsertOneAsync(this);
        }
    }
}
