using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        int highestHand=0;
        private (int,int) HandCheck(List<Card> cardsTable,List<Card> hand)
        {
            List<Card> allCard = new List<Card>();
            allCard.AddRange(cardsTable);
            allCard.AddRange(hand);
            List<Card> pairs = new();
            List<Card> drills = new();
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
                    if (counter==2)
                    {
                        pairs.Add(card);
                        counter--;
                    }
                    else if (counter == 3)
                    {
                        drills.Add(card);
                        counter = 1;
                    }
                }
            }

            //Royal flush


            //Straight flush

            //Poker

            //Full house


            //Flush

            
            foreach (Card card in allCard)
            {
                if (suites.Keys.Contains(card.Suite))
                {
                    suites[card.Suite]++;
                }
                else
                {
                    suites[card.Suite]=1;
                }
            }

            if (true)
            {

            }

            //Straight

            //Drill

            int maxDrill = 0;
            if (drills.Count==1)
            {
                maxDrill = drills[0].DefaultValue;
                return (4, maxDrill);
            }
            else
            {
                foreach (Card card in drills)
                {
                    if (card.DefaultValue >maxDrill)
                    {
                        maxDrill = card.DefaultValue;
                    }
                }
                return (4, maxDrill);

            }

            //Pair,two pair

            if (pairs.Count==1)
            {
                int max = 0;
                foreach (Card card in pairs)
                {
                    if (card.DefaultValue>max)
                    {
                        max = card.DefaultValue;
                    }
                }
                return (2, max);
            }
            else if (pairs.Count==2)
            {
                int max = 0;
                foreach (Card card in pairs)
                {
                    if (card.DefaultValue > max)
                    {
                        max = card.DefaultValue;
                    }
                }
                return (3, max);
            }

            //High card
            int handValue = 0; 
            foreach (Card card in allCard)
            {
                if(card.DefaultValue>handValue) 
                {
                    handValue = card.DefaultValue;
                }
            }
            return (1,handValue);
        }
    }
}
