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
    [BsonDiscriminator("User")]
    public class UserData
    {
        [BsonElement("UserCreatedDate")]
        public BsonDateTime createddate { get; set; }

        [BsonElement("UserName")]
        public BsonString uname { get; set; }

        [BsonId(IdGenerator = typeof(StringObjectIdGenerator))]
        public ObjectId uid { get; set; }

        [BsonElement("Password")]
        public BsonString upass { get; set; }

        [BsonElement("Email")]
        public BsonString email { get; set; }

        [BsonElement("Question")]
        public BsonString question { get; set; }

        [BsonElement("Answer")]
        public BsonString answer { get; set; }
    }
}
