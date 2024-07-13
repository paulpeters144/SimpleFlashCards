using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using MySql.Data.MySqlClient;
using SimpleFlashCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleFlashCards.DbContext
{
    public class MongoDb: IDbContext
    {
        public string CS { get; set; }
        public string DataBase { get; set; }
        public MongoClient Client { get; set; }
        public IMongoDatabase DB { get; set; }
        public MongoDb()
        {
            CS = new AppSettings().GetSetting("ConnectionStrings:Stage-SFC");
            DataBase = "Stage-SFC";
            Client = new MongoClient(CS);
            DB = Client.GetDatabase(DataBase);
        }
        public Dictionary<string,string> GetRandomQuestionSet(int numberOfCards)
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            var collection = DB.GetCollection<FactModel>("RandomFacts");
            var facts = collection.AsQueryable().Sample(numberOfCards).ToList();

            foreach (var fact in facts)
                result.Add(fact.Question, fact.Answer);

            return result;
        }
        public void StoreNewUser(UserModel user)
        {
            var client = new MongoClient(CS);
            IMongoDatabase db = client.GetDatabase(DataBase);
            var collection = db.GetCollection<UserModel>("Users");
            collection.InsertOneAsync(user);
        }
        public bool hasRegsitered(string email)
        {
            bool result = false;
            var collection = DB.GetCollection<UserModel>("Users");
            var filter = Builders<UserModel>.Filter.Eq("Email", email);
            var userRecord = collection.Find(filter).FirstOrDefault();
            
            if (userRecord != null)
                result = userRecord.Verified;
            
            return result;
        }
        public bool userTableHas(string key, string value)
        {
            var collection = DB.GetCollection<UserModel>("Users");
            var filter = Builders<UserModel>.Filter.Eq(key, value);
            var firstRecord = collection.Find(filter).FirstOrDefault();
            return firstRecord == null ? false : true;
        }
        public UserModel GetUser(string email)
        {
            var collection = DB.GetCollection<UserModel>("Users");
            var filter = Builders<UserModel>.Filter.Eq("Email", email);
            var userRecord = collection.Find(filter).FirstOrDefault();
            return userRecord;
        }
        public void registerEmail(string email)
        {
            var collection = DB.GetCollection<UserModel>("Users");
            var filter = Builders<UserModel>.Filter.Eq("Email", email);
            var update = Builders<UserModel>.Update.Set("Verified", true);
            var result = collection.UpdateOneAsync(filter, update);
        }
        public bool GoodLoginFor(string email, string password)
        {
            bool result = false;
            var collection = DB.GetCollection<UserModel>("Users");
            var filter = Builders<UserModel>.Filter.Eq("Email", email);
            var record = collection.Find(filter).FirstOrDefault();

            if (record != null)
                result = String.Equals(password, record.Password) ? true : false;

            return result;
        }
        public string CreateSSOUser(GoogleUserModel userDetails)
        {
            string result = "";
            if (!userTableHas("Email", userDetails.Email))
            {
                UserModel user = new UserModel
                {
                    Email = userDetails.Email,
                    CredType = "Google",
                    Name = userDetails.FirstName,
                    Verified = true
                };
                result = "registered";
                StoreNewUser(user);
            }
            else
            {
                result = "login";
            }
            return result;
        }
        public void ResetPasswordFor(string email, string password)
        {
            var collection = DB.GetCollection<UserModel>("Users");
            var filter = Builders<UserModel>.Filter.Eq("Email", email);
            var update = Builders<UserModel>.Update.Set("Password", password);
            var result = collection.UpdateOneAsync(filter, update);
        }

        public async Task<List<DeckModel>> LoadDecksForAsync(string user)
        {
            List<DeckModel> result = new List<DeckModel>();

            try
            {
                var collection = DB.GetCollection<UserDeckModel>("Decks");
                var filter = Builders<UserDeckModel>.Filter.Eq("Email", user);
                var dbResult = await collection.Find(filter).FirstOrDefaultAsync();

                foreach (var deck in dbResult.Decks)
                {
                    result.Add(deck);
                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }
        public async Task<string> CreateDeckAsync(string email, string title, string desc)
        {
            string result = "ok";
            
            if (await userHasDeckTitleAsync(email, title))
            {
                result = "has-deck";
            }
            else
            {
                try
                {
                    var collection = DB.GetCollection<UserDeckModel>("Decks");
                    var filter = Builders<UserDeckModel>.Filter.Eq("Email", email);
                    var update = Builders<UserDeckModel>.Update.Push("Decks", new DeckModel { Title = title, Desc = desc });
                    await collection.UpdateOneAsync(filter, update);
                }
                catch (Exception ex)
                {
                    
                }
            }
            return result;
        }

        private async Task<bool> userHasDeckTitleAsync(string email, string title)
        {
            try
            {
                var collection = DB.GetCollection<UserDeckModel>("Decks");
                var filter = Builders<UserDeckModel>.Filter.Eq("Email", email);
                var dbResult = await collection.Find(filter).FirstOrDefaultAsync();

                if (dbResult == null)
                    return false;

                foreach (var deck in dbResult.Decks)
                {
                    if (String.Equals(deck.Title, title))
                        return true;
                }
            }
            catch (Exception ex)
            {

            }
            
            return false;
        }
    }
}
