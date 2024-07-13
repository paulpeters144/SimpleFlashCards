using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleFlashCards.Models
{
    public class UserModel
    {
        [BsonId]
        public Guid ID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public bool Verified { get; set; }
        public string CredType { get; set; }
        public DeckModel[] Decks { get; set; }
    }
}
