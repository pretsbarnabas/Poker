using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    internal class Card
    {
        public int DefaultValue { get; set; }
        public (int lowValue,int highValue) AceValue{get;set;}
        public string Suite { get; set; }
        public bool isAce { get; set; }

        public Card(char val, string suite)
        {
            switch (val)
            {
                case 'A':
                    AceValue = (1, 13);
                    isAce = true;
                    break;
                case 'K':
                    DefaultValue = 13;
                    break;
                case 'Q':
                    DefaultValue = 12;
                    break;
                case 'J':
                    DefaultValue = 11;
                    break;
                default:
                    DefaultValue = (int)val;
                    break;
            }
        }
    }
}
