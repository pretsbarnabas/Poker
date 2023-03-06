using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Poker
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        public Game()
        {
            InitializeComponent();
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Menu.xaml", UriKind.Relative));
        }

        int highestHand = 0;

        private int CardClassHighestValue(List<Card> cardList)
        {
            int max = 0;
            foreach (Card card in cardList)
            {
                if (card.DefaultValue > max)
                {
                    max = card.DefaultValue;
                }
            }
            return max;
        }
        private (int, int) HandCheck(List<Card> cardsTable, List<Card> hand)
        {
            List<Card> allCard = new List<Card>();
            allCard.AddRange(cardsTable);
            allCard.AddRange(hand);
            allCard = allCard.OrderBy(x => x.DefaultValue).ToList();
            List<Card> pairs = new();
            List<Card> drills = new();
            bool isPoker = false;
            int pokerValue = 0;
            Dictionary<string, int> suites = new();
            foreach (Card card in allCard)
            {
                for (int i = 0; i < allCard.Count; i++)
                {
                    int counter = 1;
                    if (allCard[i].DefaultValue == card.DefaultValue)
                    {
                        counter++;
                        
                    }
                    else if (counter == 2)
                    {
                        pairs.Add(card);
                        counter--;
                    }
                    else if (counter == 3)
                    {
                        drills.Add(card);
                        counter++;
                    }
                    else if(counter == 4)
                    {
                        isPoker = true;
                        pokerValue = card.DefaultValue;
                        counter = 1;
                    }
                }
            }

            foreach (Card card in allCard)
            {
                if (suites.Keys.Contains(card.Suite))
                {
                    suites[card.Suite]++;
                }
                else
                {
                    suites[card.Suite] = 1;
                }
            }

            //Royal flush
            string flushSuit = "";
            foreach (KeyValuePair<string, int> item in suites)
            {
                if (item.Value == 5)
                {
                    flushSuit = item.Key;
                }
            }
            if (flushSuit != "")
            {
                List<int> RFlushValues = new();
                foreach (Card c in allCard)
                {
                    if (c.DefaultValue >= 10)
                    {
                        RFlushValues.Add(c.DefaultValue);
                    }
                }
                if (RFlushValues.Contains(10) && RFlushValues.Contains(11) && RFlushValues.Contains(12) && RFlushValues.Contains(13) && RFlushValues.Contains(14))
                {
                    return (10, 14);
                }
            }

            //Straight flush

            //Poker
            if (isPoker)
            {
                return (9, pokerValue);
            }
            //Full house
            if (pairs.Count >= 1 && drills.Count >= 1)
            {
                List<Card> pairsDrill = new();
                pairsDrill.AddRange(drills);
                pairsDrill.AddRange(pairs);
                return (7, CardClassHighestValue(pairs));
            }

            //Flush

            flushSuit = "";
            foreach (KeyValuePair<string, int> item in suites)
            {
                if (item.Value == 5)
                {
                    flushSuit = item.Key;
                }
            }
            if (flushSuit != "")
            {
                int max = 0;
                foreach (Card c in allCard)
                {
                    if (c.Suite == flushSuit && c.DefaultValue > max)
                    {
                        max = c.DefaultValue;
                    }
                }
                return (6, max);
            }

            //Straight



            //Drill
            if (drills.Count == 1)
            {
                return (4, drills[0].DefaultValue);
            }
            else
            {
                return (4, CardClassHighestValue(drills));
            }

            //Pair,two pair

             if (pairs.Count == 1)
            {
                return (2, pairs[0].DefaultValue);
            }
            else if (pairs.Count == 2)
            {

                return (3, CardClassHighestValue(pairs));
            }

            //High card
            return (1, CardClassHighestValue(allCard));
        }
    }
}
