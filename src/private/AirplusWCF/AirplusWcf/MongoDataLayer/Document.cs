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
    [BsonDiscriminator("listings")]
    public class Document
    {
        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public ObjectId uid { get; set; }

        [BsonElement("Month")]
        public BsonString Month { get; set; }

        [BsonElement("Day")]
        public List<Day> days;

        [BsonElement("DateTimeStamp")]
        public BsonDateTime datetaken;

        [BsonElement("Listing")]
        public BsonString ListingId { get; set; }
        public Document()
        {
            days = new List<Day>();
        }
    }
}
