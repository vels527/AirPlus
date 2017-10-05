using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson.Serialization.IdGenerators;

namespace MongoDataLayer
{
    [BsonDiscriminator("Listings")]
    public class Listings
    {
        [BsonElement("User")]
        public UserData User { get; set; }


    }
}