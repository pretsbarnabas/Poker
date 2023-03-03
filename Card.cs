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
        public bool isAce { get; set; } = false;

        public Card(char val, string suite)
        {
            switch (val)
            {
                case 'a':
                    AceValue = (1, 14);
                    DefaultValue = 14;
                    isAce = true;
                    break;
                case 'k':
                    DefaultValue = 13;
                    break;
                case 'q':
                    DefaultValue = 12;
                    break;
                case 'j':
                    DefaultValue = 11;
                    break;
                default:
                    DefaultValue = (int)val;
                    break;
            }
        }
    }
}
