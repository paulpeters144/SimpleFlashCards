using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleFlashCards.Helpers;
using SimpleFlashCards.DbContext;
using System.Threading;

namespace SimpleFlashCards.Pages.Account
{
    public class ResetModel : PageModel
    {
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }
        [BindProperty]
        public string RePassword { get; set; }
        public new string PageContext { get; set; }
        public bool ShowEmailSent = false;
        public IDbContext DB;
        public void OnGet()
        {
            if (Request.QueryString.HasValue)
                decideWhatToDoWithQuery();
        }

        public void OnPostRequestReset()
        {
            DB = new MongoDb();
            if (DB.hasRegsitered(Email))
                sendResetEmail();

            ShowEmailSent = true;
        }

        public IActionResult OnPostReset()
        {
            DB = new MongoDb();
            string result = sendResetRequest();

            if (String.Equals(result, "ok"))
            {
                return LocalRedirect("/");
            }
            else
            {
                PageContext = result;
                return null;
            }
        }

        private string sendResetRequest()
        {
            string result = "unknown";
            if (HttpContext.Session.Get("resetEmail") != null)
            {
                if (!String.IsNullOrEmpty(Password) &&
                    !String.IsNullOrEmpty(RePassword))
                {
                    string email = HttpContext.Session.Get("resetEmail");
                    RegisterUser register = new RegisterUser
                    (
                        email,
                        Password,
                        RePassword
                    );
                    result = register.ResetPassword();
                    if (String.Equals(result, "ok"))
                    {
                        Thread.Sleep(100);
                        string securePw = new Secure().GenerateSHA256(Password);
                        result = DB.GoodLoginFor(email, securePw) == false ? "unknown" : "ok";
                    }
                }
            }
            return result;
        }

        private void sendResetEmail()
        {
            Email email = new Email(EmailType.Reset, Email);
            email.BaseUrl = getBaseUrlFrom(Url);
            email.Send();
        }
        private string getBaseUrlFrom(IUrlHelper url)
        {
            string result = "";

            if (url.ActionContext.HttpContext.Request.IsHttps)
                result = $"https://{url.ActionContext.HttpContext.Request.Host.Value}";

            return result;
        }
        private void decideWhatToDoWithQuery()
        {
            if (Request.Query.ContainsKey("token"))
            {
                IDbContext DB = new MongoDb();
                string userEmailEncrypt = Request.Query["token"];
                string userEmail = new Secure().Decrypt(userEmailEncrypt);
                if (DB.hasRegsitered(userEmail))
                {
                    PageContext = "reset";
                    HttpContext.Session.Set("resetEmail",userEmail);
                }
            }
        }
    }
}
