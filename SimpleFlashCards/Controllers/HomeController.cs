using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SimpleFlashCards.DbContext;
using SimpleFlashCards.Models;
using System;
using System.Collections.Generic;
using SimpleFlashCards.Helpers;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson.Serialization.Attributes;
using SimpleFlashCards.Controllers.Helpers;
using System.Threading.Tasks;

namespace SimpleFlashCards.Controllers
{
    [Route("api/[controller]")]
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("randomset")]
        public List<FactModel> RandomSet()
        {
            return SetNewRandomCard();
        }
        private List<FactModel> SetNewRandomCard()
        {
            List<FactModel> result = new List<FactModel>();
            IDbContext db = new MongoDb();
            var randomSet = db.GetRandomQuestionSet(5);

            foreach (var item in randomSet)
                result.Add(new FactModel 
                    { Question = item.Key, Answer = item.Value }
                );
            
            return result;
        }
        [HttpPost]
        [Route("auth")]
        public JsonResult auth([FromBody] TokenModel token)
        {
            var userDetails = new ProviderUserDetails()
                .GetUserDetails(token.token);
            LoginUser loginUser = new LoginUser();
            loginUser.GoogleSSO(userDetails);

            HttpContext.Session.SetUserId(userDetails.Email);
            return Json("login");
        }
        [HttpPost]
        [Route("createdeck")]
        public async System.Threading.Tasks.Task CreateDeckAsync([FromBody] DeckModel deck)
        {
            string user = HttpContext.Session.GetId();

            if (String.IsNullOrEmpty(user))
                return;


            CreateDeck createDeck = new CreateDeck();
            var result = await createDeck.SubmitDeckToDbAsync
                (user, deck.Title, deck.Desc);
            var test = "";
        }
        [HttpGet]
        [Route("loaddecks")]
        public async Task<List<DeckModel>> LoadDecksAsync()
        {
            string user = HttpContext.Session.GetId();
            
            if (String.IsNullOrEmpty(user))
                return null;

            IDbContext db = new MongoDb();
            List<DeckModel> decks = await db.LoadDecksForAsync(user);
         
            if (decks.Count > 0)
                return decks;
            else
                return null;
        }
    }
}
