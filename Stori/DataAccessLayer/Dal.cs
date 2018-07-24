using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        // To do: update the connection string with the DNS name
        // or IP address of your server. 
        //For example, "mongodb://testlinux.cloudapp.net
        private string userName = "stori";
        private string host = "stori.documents.azure.com";
        private string password = "FILLMEIN";

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
                return collection.Find(x => x.Author.UserName == username).ToList();
            }
            catch (MongoConnectionException)
            {
                return new List<Post>();
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

        private IMongoCollection<Post> GetPostsCollection()
        {
            var client = GetMongoClient();

            var database = client.GetDatabase(dbName);
            var postCollection = database.GetCollection<Post>(collectionName);
            return postCollection;
        }

        private IMongoCollection<Post> GetPostsCollectionForEdit()
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(host, 10255);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            MongoIdentity identity = new MongoInternalIdentity(dbName, userName);
            MongoIdentityEvidence evidence = new PasswordEvidence(password);

            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

            MongoClient client = new MongoClient(settings);
            var database = client.GetDatabase(dbName);
            var todoPostCollection = database.GetCollection<Post>(collectionName);
            return todoPostCollection;
        }

        private MongoClient GetMongoClient()
        {
            MongoClientSettings settings = new MongoClientSettings();
            settings.Server = new MongoServerAddress(host, 10255);
            settings.UseSsl = true;
            settings.SslSettings = new SslSettings();
            settings.SslSettings.EnabledSslProtocols = SslProtocols.Tls12;

            MongoIdentity identity = new MongoInternalIdentity(dbName, userName);
            MongoIdentityEvidence evidence = new PasswordEvidence(password);

            settings.Credential = new MongoCredential("SCRAM-SHA-1", identity, evidence);

            MongoClient client = new MongoClient(settings);

            return client;
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
