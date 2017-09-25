using System;

//using MongoDB;
//using MongoDB.Driver;
//using MongoDB.Driver.Core;
//using MongoDB.Bson;
//using MongoDB.Bson.Serialization.Attributes;

namespace DataLayer
{
    public static class MongoCQRS
    {
        public static string CreateUser(string uname, string upass, string email, string question, string answer)
        {
            string msg = "";
            //BsonString Name = new BsonString(uname);
            //BsonString Passwd = new BsonString(upass);
            //BsonString Email = new BsonString(email);
            //BsonString Qn = new BsonString(question);
            //BsonString Ans = new BsonString(answer);

            return msg;
        }
    }
    public class Connection
    {
        private static Connection _conn;
        private Connection() { }
        //public MongoClient client = new MongoClient("");
        public static Connection conn
        {
            get
            {
                if (_conn == null)
                {
                    _conn = new Connection();
                }
                return _conn;
            }
        }
    }
}

