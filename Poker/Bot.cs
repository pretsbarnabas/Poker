using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class Bot
    {
        public List<Card> Cards { get; set; }
        public int Money { get; set; }
        public bool isDealer { get; set; }
        public Bot(Card card1, Card card2, int defaultMoney)
        {
            Cards = new List<Card>();
            Cards.Add(card1);
            Cards.Add(card2);
            Money = defaultMoney;
        }

    }
}
