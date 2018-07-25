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

        public int Height { get; set; }

        public int Width { get; set; }

        public string Caption { get; set; }

        public DateTime UploadDate { get; set; }

        public ImageFormat Filetype { get; set; }

        public string[] MediaUrls { get; set; }

        // Implements IMongoIdentifiable
        public MongoDB.Bson.ObjectId Id()
        {
            return _id;
        }

        // note: the id is the id of ImageWithMetadata, not the id of the image inside of it
        public static async Task<ImageWithMetadata> LookupById(string id)
        {
            var collection = LookupCollectionFromDb();
            var cursor = await collection.FindAsync<ImageWithMetadata>(x => x._id == ObjectId.Parse(id));

            ImageWithMetadata match = cursor?.First();
            return match;
        }


        public async Task SaveChangesAsync()
        {
            var dal = Stori.DataAccessLayer.Dal.Instance;
            var db = dal.GetMongoDb();
            var collection = db.GetCollection<ImageWithMetadata>("ImageWithMetadataList");
            await collection.InsertOneAsync(this);
        }

        private static MongoDB.Driver.IMongoCollection<ImageWithMetadata> LookupCollectionFromDb()
        {
            var dal = Stori.DataAccessLayer.Dal.Instance;
            var db = dal.GetMongoDb();
            var collection = db.GetCollection<ImageWithMetadata>("ImageWithMetadataList");

            return collection;
        }
    }
}
