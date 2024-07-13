using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleFlashCards.Models
{
    public class FactModel
    {
        public object _id { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
    }
}
