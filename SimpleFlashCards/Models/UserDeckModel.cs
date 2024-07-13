using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SimpleFlashCards.Models
{
    public class UserDeckModel
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string _id { get; set; }
        public string Email { get; set; }
        public DeckModel[] Decks { get; set; }
    }
}
