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
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public ObjectId uid { get; set; }

        [BsonElement("User")]
        public UserData User { get; set; }

        [BsonElement("Primary")]
        public BsonString ListingID { get; set; }

        [BsonElement("Show")]
        public BsonBoolean isShow { get; set; }

        [BsonElement("Secondary")]
        public List<Secondary> SecondaryListing { get; set; }
    }
}