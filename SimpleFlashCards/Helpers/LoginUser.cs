using SimpleFlashCards.DbContext;
using SimpleFlashCards.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleFlashCards.Helpers
{
    public class LoginUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public IDbContext DB { get; set; }
        public LoginUser()
        {
            DB = new MongoDb();
        }
        public string attempt()
        {
            string result = "ok";

            if (!DB.GoodLoginFor(Email, Password))
                result = "invalid";

            return result;
        }
        public string GoogleSSO(GoogleUserModel userDetails)
        {
            string result = "signin";

            if (!DB.hasRegsitered(userDetails.Email))
                result = DB.CreateSSOUser(userDetails);

            return result;
        }
    }
}
