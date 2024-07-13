using MongoDB.Bson.Serialization.Attributes;
using System;

namespace SimpleFlashCards.Models
{
    public class DeckModel
    {
        public string Title { get; set; }
        public string Desc { get; set; }
    }
}


