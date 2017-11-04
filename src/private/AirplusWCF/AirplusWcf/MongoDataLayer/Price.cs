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
    [BsonDiscriminator("Price")]
    public class Price
    {
        [BsonElement("DateAvailable")]
        public BsonDateTime date { get; set; }

        [BsonElement("LocalCurrency")]
        public BsonString local_currency { get; set; }

        [BsonElement("LocalPrice")]
        public BsonDouble local_price { get; set; }

        [BsonElement("NativeCurrency")]
        public BsonString native_currency { get; set; }

        [BsonElement("NativePrice")]
        public BsonDouble native_price { get; set; }

        [BsonElement("type")]
        public BsonString type { get; set; }
    }
}
