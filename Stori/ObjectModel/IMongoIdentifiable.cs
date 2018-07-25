using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stori.ObjectModel
{
    public interface IMongoIdentifiable
    {
        MongoDB.Bson.ObjectId _id { get; set; }
    }
}
