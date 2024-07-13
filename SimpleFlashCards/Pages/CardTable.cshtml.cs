using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SimpleFlashCards.Helpers;

namespace SimpleFlashCards.Pages.Account
{
    public class AccountModel : PageModel
    {
        public IActionResult OnGet()
        {
           return checkCreds();
        }

        private IActionResult checkCreds()
        {
            return null;
            if (!HttpContext.Session.GetId().Contains("@"))
                return LocalRedirect("/");
            else return null;
        }
    }
}
