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
    [BsonDiscriminator("Day")]
    public class Day
    {
        [BsonElement("Available")]
        public BsonBoolean available { get; set; }
        [BsonElement("Date")]
        public BsonDateTime date { get; set; }

        [BsonElement("Price")]
        public Price price { get; set; }
    }
}
