using Microsoft.AspNetCore.Mvc;
using SimpleFlashCards.DbContext;
using SimpleFlashCards.Models;
using System;
using System.Text.RegularExpressions;

namespace SimpleFlashCards.Helpers
{
    internal class RegisterUser
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string RePassword { get; set; }
        public bool Agree { get; set; }
        public IDbContext DB;
        public string BaseUrl { get; set; }

        public RegisterUser(string email, string password, string repword, bool agree)
        {
            Email = email.Trim();
            Password = password.Trim();
            RePassword = repword.Trim();
            Agree = agree;
            DB = new MongoDb();
        }

        public RegisterUser(string email, string password, string repword)
        {
            Email = email;
            Password = password.Trim();
            RePassword = repword.Trim();
            DB = new MongoDb();
        }
        public string AttemptAdd()
        {
            string result = "ok";

            if (!String.Equals(Password, RePassword))
                result = "pw-no-match";

            else if (!paswordFollowsCriteria())
                result = "pw-week";

            else if (!Agree)
                result = "disagree";

            if (DB.userTableHas("Email", Email))
                result = "has-email";

            else competeRegister();

            return result;
        }
        public string ResetPassword()
        {
            string result = "ok";

            if (!String.Equals(Password, RePassword))
                result = "pw-no-match";

            else if (!paswordFollowsCriteria())
                result = "pw-week";

            else
            {
                string securedPw = new Secure().GenerateSHA256(Password);
                DB.ResetPasswordFor(Email, securedPw);
            }
            
            return result;
        }

        private void competeRegister()
        {
            storeUserToDb();
            sendEmail();
        }

        private void sendEmail()
        {
            Email email = new Email(EmailType.Register, Email);
            email.BaseUrl = BaseUrl;
            email.Send();
        }

        private void storeUserToDb()
        {
            Password = new Secure().GenerateSHA256(Password);
            RePassword = Password;
            UserModel user = new UserModel 
            { 
                Decks = null, 
                Password = Password, 
                Email = Email,
                Verified = false,
                CredType = "Email"
            };

            DB.StoreNewUser(user);
        }

        private bool paswordFollowsCriteria()
        {
            bool result = true;
            if (!Regex.Match(Password, @"(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,}").Success)
                result = false;
            return result;
        }
    }
}