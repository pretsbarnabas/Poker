using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class Card
    {
        public int DefaultValue { get; set; }
        public (int lowValue,int highValue) AceValue{get;set;}
        public string Suite { get; set; }

        public bool isAce { get; set; }
        public string ImageNumber { get; set; }

        public Card(string raw)
        {
            Suite = raw.Split("-")[0];
            string value = raw.Split("-")[1];
            ImageNumber = raw.Split("-")[2];
            switch (value)
            {
                case "a":
                    AceValue = (1, 14);
                    DefaultValue = 1;
                    isAce = true;
                    break;
                case "k":
                    DefaultValue = 13;
                    break;
                case "q":
                    DefaultValue = 12;
                    break;
                case "j":
                    DefaultValue = 11;
                    break;
                default:
                    DefaultValue = int.Parse(value);
                    break;
            }
        }
    }
}
