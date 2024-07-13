using SimpleFlashCards.DbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleFlashCards.Controllers.Helpers
{
    public class CreateDeck
    {
        private IDbContext DB;

        public CreateDeck()
        {
            DB = new MongoDb();
        }
        public async Task<string> SubmitDeckToDbAsync(string email, string title, string desc)
        {
            string result = "";
            result = await DB.CreateDeckAsync(email, title, desc);
            
            return result;
        }
    }
}
