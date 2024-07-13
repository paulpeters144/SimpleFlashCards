using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SimpleFlashCards.DbContext;
using System.ComponentModel.DataAnnotations;
using SimpleFlashCards.Models;
using SimpleFlashCards.Helpers;

namespace SimpleFlashCards.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private IDbContext Db;
        public string RegMessage;
        public string LoginMessage;
        public string Token;
        [BindProperty]
        public RegisterModel Register { get; set; }
        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
            Db = new MongoDb();
        }
        public void OnGet()
        {

        }
        public void OnPostRegister()
        {
            try
            {
                RegisterUser register = new RegisterUser
                (
                    Register.Email,
                    Register.Password,
                    Register.RePassword,
                    Register.Agree
                );
                register.BaseUrl = getBaseUrlFrom(Url);
                RegMessage = register.AttemptAdd();
            }
            catch (Exception)
            {

            }
        }

        public IActionResult OnPostLogin()
        {
            string password = new Secure().GenerateSHA256(Register.Password);
            LoginUser login = new LoginUser
            {
                Email = Register.Email,
                Password = password
            };
            LoginMessage = login.attempt();

            if (String.Equals(LoginMessage, "ok"))
            {
                HttpContext.Session.SetUserId(Register.Email);
                return LocalRedirect("~/cardtable");
            }

            return null;
        }

        private string getBaseUrlFrom(IUrlHelper url)
        {
            string result = "";

            if (url.ActionContext.HttpContext.Request.IsHttps)
                result = $"https://{url.ActionContext.HttpContext.Request.Host.Value}";

            return result;
        }
    }
}
