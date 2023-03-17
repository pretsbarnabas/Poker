using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Poker
{
    public class Bot
    {
        public List<Card> Cards { get; set; }
        public int Money { get; set; }
        public Bot(Card card1, Card card2, int defaultMoney)
        {
            Cards = new List<Card>();
            Cards.Add(card1);
            Cards.Add(card2);
            Money = defaultMoney;
        }
        public void Move(out bool raised, (int,int) hand)
        {
            Random random = new Random();
            double handstrength = ((double)hand.Item1 + (double)hand.Item2  ) / 28;
            double moneypercent = (double)Money / (double)Menu.settings["Zsetonok"];
            double winningchance = (double)handstrength * 0.8 + moneypercent * 0.2;
            Debug.WriteLine(winningchance);
            if (winningchance < 0.3)
            {
                Debug.WriteLine("folds");
                raised = false;
            }
            else if(winningchance < 0.8)
            {
                Debug.WriteLine("checks");
                raised = false;
            }
            else
            {
                Debug.WriteLine("raises");
                raised = true;
            }

        }

    }
}
