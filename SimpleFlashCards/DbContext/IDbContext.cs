using SimpleFlashCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleFlashCards.DbContext
{
    public interface IDbContext
    {
        public Dictionary<string, string> GetRandomQuestionSet(int numberOfCards);
        public void StoreNewUser(UserModel user);
        bool hasRegsitered(string userEmail);
        bool userTableHas(string key, string value);
        void registerEmail(string email);
        bool GoodLoginFor(string userName, string password);
        string CreateSSOUser(GoogleUserModel userDetails);
        Task<string> CreateDeckAsync(string email, string title, string desc);
        void ResetPasswordFor(string email, string password);
        Task<List<DeckModel>> LoadDecksForAsync(string user);
    }
}
