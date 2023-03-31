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
        public int Num { get; set; }
        public bool isDealer { get; set; }
        public Bot(Card card1, Card card2, int defaultMoney, int num)
        {
            Cards = new List<Card>();
            Cards.Add(card1);
            Cards.Add(card2);
            Money = defaultMoney;
            Num = num;
        }
        public Bot(int defaultMoney)
        {
            Cards = new List<Card>();
            Money = defaultMoney;
            Num = 0;
        }
        public void Move(out bool raised, (int,int) hand)
        {
            if (Cards.Count == 0) { raised = false; return; }
            Random random = new Random();
            double handstrength = ((double)hand.Item1 + (double)hand.Item2  ) / 28;
            double moneypercent = (double)Money / (double)Menu.settings["Zsetonok"];
            double winningchance = (double)handstrength * 0.8 + moneypercent * 0.2;
            if (winningchance < 0.3)
            {
                Fold();
                Debug.WriteLine("folds");
                raised = false;
            }
            else if(winningchance < 0.7)
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
        private void Fold()
        {
            Cards.Clear();
        }
    }
}
