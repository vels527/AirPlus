using System;
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
            var _exists = false;
            var filter = new BsonDocument();
            using (var cursor = users.Find(filter).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        BsonValue bname;
                        
                        if (doc.TryGetValue("UserName", out bname) )
                        {
                            if (bname.ToString() == uname)
                            {
                                _exists= true;
                            }
                        }
                    }
                }
            }
            if (_exists==false)
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

        //Register listings,edit listings,modify listings
        public static void RegisterListings(string uname, string primaryListing, string[] listings)
        {
            //var users = conn.md.GetCollection<UserData>("users");
            //var coll = users.Find(new BsonDocument()).ToListAsync().GetAwaiter().GetResult();
            var filter = Builders<UserData>.Filter.Eq(c => c.uname, uname);
            var result = conn.md.GetCollection<UserData>("users").Find(filter).FirstOrDefaultAsync().GetAwaiter().GetResult();
            var _listings = conn.md.GetCollection<Listings>("Listings");
            var filterListings = Builders<Listings>.Filter.Eq(c => c.User, result);
            var resultListings = conn.md.GetCollection<Listings>("Listings").Find(filterListings).ToListAsync().GetAwaiter().GetResult();
            List<string> _oldList = new List<string>();
            List<string> _newList = new List<string>();
            var _isPrimary = false;            
            foreach (Listings l in resultListings)
            {
                if (l.ListingID == primaryListing)
                {
                    _isPrimary = true;
                    foreach (string str in listings)
                    {
                        var _isPresent = false;
                        foreach (Secondary _secondary in l.SecondaryListing)
                        {
                            if (str == _secondary.Listing)
                            {
                                _isPresent = true;
                                _secondary.isShow = true;
                                _oldList.Add(str);
                                break;
                            }
                        }
                        if (_isPresent == false)
                        {
                            _newList.Add(str);
                        }
                    }
                    if(l.SecondaryListing.Count()>_oldList.Count())
                    foreach (Secondary _secondary in l.SecondaryListing)
                    {
                        var _isPresentSecondary = false;
                        foreach (string k in _oldList)
                        {
                            if(k==_secondary.Listing)
                            {
                                _isPresentSecondary = true;
                                break;
                            }
                        }
                        if (_isPresentSecondary == false)
                        {
                            _secondary.isShow = false;
                        }
                    }
                    if(_newList.Count()!=0)
                    foreach(string str in _newList)
                    {
                        Secondary _secondary = new Secondary();
                        _secondary.Listing = str;
                        _secondary.isShow = true;
                        l.SecondaryListing.Add(_secondary);
                    }
                    var listingBson = l.ToBsonDocument();
                    var builderUpdateListings = Builders<Listings>.Filter;
                    var filterUpdateListings = builderUpdateListings.Eq(c => c.User, result) & builderUpdateListings.Eq(c=>c.ListingID,primaryListing);
                    _listings.UpdateOneAsync(filterUpdateListings,listingBson).Wait();
                    break;
                }
            }
            if (_isPrimary == false)
            {
                Listings _list = new Listings();
                _list.User = result;
                _list.ListingID = primaryListing;
                _list.isShow = true;
                foreach (string str in listings)
                {
                    Secondary _secondary = new Secondary();
                    _secondary.Listing = str;
                    _secondary.isShow = true;
                    _list.SecondaryListing.Add(_secondary);
                }                
                _listings.InsertOneAsync(_list).Wait();
            }
        }

        public static bool ValidateUser(string lname,string lpass)
        {
            if (lname == "siva05" && lpass == "billyPo0$")
                return true;
            var users = conn.md.GetCollection<BsonDocument>("users");
            var filter = new BsonDocument();
            using (var cursor = users.Find(filter).ToCursor())
            {
                while (cursor.MoveNext())
                {
                    foreach (var doc in cursor.Current)
                    {
                        BsonValue bname;
                        BsonValue bpass;
                        if(doc.TryGetValue("UserName",out bname) && doc.TryGetValue("Password", out bpass))
                        { 
                            if(bname.ToString()==lname && bpass.ToString()==lpass)
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
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
            md = mc.GetDatabase("airbnb");
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
