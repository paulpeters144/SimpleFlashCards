using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleFlashCards.DbContext;
using SimpleFlashCards.Helpers;
using SimpleFlashCards.Models;

namespace SimpleFlashCards.Pages
{
    public class RegisteredModel : PageModel
    {
        public enum Context
        {
            newReg,
            Verified,
            AlreadyVerified
        }
        IDbContext DB;
        public Context Message = Context.newReg;
        public void OnGet()
        {
            DB = new MongoDb();
            if (Request.QueryString.HasValue)
                decideWhatToDoWithQuery();
        }

        private void decideWhatToDoWithQuery()
        {
            if (Request.Query.ContainsKey("token"))
            {
                string userEmailEncrypt = Request.Query["token"];
                string userEmail = new Secure().Decrypt(userEmailEncrypt);
                if (!DB.hasRegsitered(userEmail))
                {
                    DB.registerEmail(userEmail);
                    Message = Context.Verified;
                    HttpContext.Session.SetUserId(userEmail);
                }
                else
                {
                    Message = Context.AlreadyVerified;
                }
            }
        }
    }
}
