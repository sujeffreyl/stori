using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Bson;
using Stori.ObjectModel;
using System.Security.Authentication;

namespace Stori.DataAccessLayer
{
    public sealed class Dal : IDisposable
    {
        private static readonly Dal instance = new Dal();

        //private MongoServer mongoServer = null;
        private bool disposed = false;

        private Secrets secrets;

        // This sample uses a database named "Tasks" and a 
        //collection named "TasksList".  The database and collection 
        //will be automatically created if they don't already exist.
        private string dbName = "Posts";
        private string collectionName = "PostsList";

        public static Dal Instance
        {
            get
            {
                return instance;
            }
        }


        // Default constructor.        
        private Dal()
        {
            secrets = new Secrets();
        }

        ~Dal()
        {
            this.Dispose(false);
        }

        // Gets all Post items from the MongoDB server.        
        public List<Post> GetAllPosts()
        {
            try
            {
                var collection = GetPostsCollection();
                return collection.Find(new BsonDocument()).ToList();
            }
            catch (MongoConnectionException)
            {
                return new List<Post>();
            }
        }

        public List<Post> GetPostsByUsername(string username)
        {
            try
            {
                var collection = GetPostsCollection();
                return collection.Find(x => x.Author.UserName == username).Sort(Builders<Post>.Sort.Descending("CreateDate")).ToList();
            }
            catch (MongoConnectionException)
            {
                return new List<Post>();
            }
        }

        public List<Post> GetPostsByTag(string tag)
        {
            try
            {
                var collection = GetPostsCollection();
                return collection.Find(x => x.Tags.Contains<string>(tag)).Sort(Builders<Post>.Sort.Descending("CreateDate")).ToList();
            }
            catch (MongoConnectionException)
            {
                return new List<Post>();
            }
        }

        public Post GetPostById(string id)
        {
            try
            {
                var collection = GetPostsCollection();
                return collection.Find(x => x._id == ObjectId.Parse(id)).FirstOrDefault();
            }
            catch (MongoConnectionException)
            {
                return new Post();
            }
        }

        // Creates a Post and inserts it into the collection in MongoDB.
        public void CreatePost(Post post)
        {
            var collection = GetPostsCollectionForEdit();
            try
            {
                collection.InsertOne(post);
            }
            catch (MongoCommandException ex)
            {
                string msg = ex.Message;
            }
        }

        // TODO: Actually, after waiting a while and refreshing, maybe it does?!?!?!
        // note: I don't think AzureCosmosDB actually supports Grid FS :( :( :(
        // Just store it in BSON for now
        /// <summary>
        /// Upload a file (probably a large binary file that could feasibly exceed 16 MB) to Mongo's Grid FileSystem
        /// </summary>
        /// <param name="stream">Stream representing the file to be uploaded</param>
        /// <param name="filename">The destination filename of the new file to be created in GridFS</param>
        /// <returns>A task with the ID of the newly created file.</returns>
        //public async Task<MongoDB.Bson.ObjectId> Upload(Stream stream, string filename)
        //{
        //    var database = GetMongoDb();
        //    var gridFileSystemBucket = new MongoDB.Driver.GridFS.GridFSBucket(database);

        //    var id = await gridFileSystemBucket.UploadFromStreamAsync(filename, stream);
        //    return id;
        //}


        public static async Task<T> LookupById<T>(string collectionName, string id) where T : IMongoIdentifiable
        {
            return await LookupById<T>(collectionName, ObjectId.Parse(id));
        }

        public static async Task<T> LookupById<T>(string collectionName, MongoDB.Bson.ObjectId idToLookup) where T : IMongoIdentifiable
        {
            var collection = LookupCollectionFromDb<T>(collectionName);
            var cursor = await collection.FindAsync<T>(x => x._id == idToLookup);

            if (cursor == null)
                return default(T);

            T match = cursor.First();
            return match;
        }

        private static MongoDB.Driver.IMongoCollection<T> LookupCollectionFromDb<T>(string collectionName)
        {
            var dal = Stori.DataAccessLayer.Dal.Instance;
            var db = dal.GetMongoDb();
            var collection = db.GetCollection<T>(collectionName);

            return collection;
        }

        private IMongoCollection<Post> GetPostsCollection()
        {
            var database = GetMongoDb();

            var postCollection = database.GetCollection<Post>(collectionName);
            return postCollection;
        }

        private IMongoCollection<Post> GetPostsCollectionForEdit()
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(this.secrets.Host, 10255);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            MongoIdentity identity = new MongoInternalIdentity(dbName, this.secrets.UserName);
            MongoIdentityEvidence evidence = new PasswordEvidence(this.secrets.Password);

            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase(dbName);
            var todoPostCollection = database.GetCollection<Post>(collectionName);
            return todoPostCollection;
        }
        
        private MongoClient GetMongoClient()
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(this.secrets.Host, 10255);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            MongoIdentity identity = new MongoInternalIdentity(dbName, this.secrets.UserName);
            MongoIdentityEvidence evidence = new PasswordEvidence(this.secrets.Password);

            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

            MongoClient client = new MongoClient(settings);

            return client;
        }

        public IMongoDatabase GetMongoDb()
        {
            return GetMongoDb(this.dbName);
        }

        private IMongoDatabase GetMongoDb(string databaseName)
        {
            var client = GetMongoClient();
            return client.GetDatabase(databaseName);
        }

        # region IDisposable

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                }
            }

            this.disposed = true;
        }

        # endregion
    }
}
