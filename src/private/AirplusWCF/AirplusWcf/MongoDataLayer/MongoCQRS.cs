﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using MongoDB;
using MongoDB.Driver;
using MongoDB.Driver.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace MongoDataLayer
{
    public static class MongoCQRS
    {
        static Connection conn;
        static MongoCQRS()
        {
            conn = Connection.Instance;
        }
        public static bool RegisterUser(string uname, string upass, string email, string question, string answer)
        {
            bool mesg=true;
            var users=conn.md.GetCollection<BsonDocument>("users");
            UserData _user = new UserData();
            _user.uname = new BsonString(uname);
            _user.upass = new BsonString(upass);
            _user.email = new BsonString(email);
            _user.question = new BsonString(question);
            _user.answer = new BsonString(answer);
            _user.createddate = new BsonDateTime(DateTime.Now);
            var _exists = users.Find(x => uname == _user.uname);
            if(_exists.Count()==0)
            {
                var userDocument = _user.ToBsonDocument();
                users.InsertOneAsync(userDocument).Wait();
            }
            else
            {
                mesg =false;
            }
            return mesg;
        }
    }
    public class Connection
    {
        private static Connection instance;
        
        private Connection() { }
        private MongoClient mc;
        public IMongoDatabase md;
        public void Init()
        {
            mc = new MongoClient(ConfigurationManager.AppSettings["mongoconn"]);
            md = mc.GetDatabase("Airbnb");
        }
        public static Connection Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Connection();
                    instance.Init();
                }
                return instance;
            }
        }
    }
}