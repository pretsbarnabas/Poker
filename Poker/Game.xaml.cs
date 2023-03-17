using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
        Random random = new Random();
        int currentZseton;
        List<Bot> bots;
        bool raised;

        public Game()
        {
            InitializeComponent();
            ZsetonSlider.Maximum = Menu.settings["Zsetonok"];
            ZsetonSlider.Value = Menu.settings["Zsetonok"] / 2;
            int numberofbots = 3;
            List<Card> cards = File.ReadAllLines("cards.txt").Select(x => new Card(x)).ToList();
            bots = GenerateBots(cards,numberofbots);
            GeneratePlayerCards(cards);
            AddChips(2000);
        }

        private void AddChips(int money)
        {
            int numOfChips = money / 500;
            for (int i = 0; i < numOfChips; i++)
            {
                grid_chips.Children.Add(LoadImage("chip.png", 50, 50));
                Image image = (Image)grid_chips.Children[i];
                image.Margin = new Thickness(i * 10, 0, 0, 0);
            }
        }

        private void GeneratePlayerCards(List<Card>cards)
        {
            Bot player = new Bot(PopRandomCard(cards), PopRandomCard(cards), currentZseton);
            wp_player.Children.Add(LoadImage(player.Cards[0].ImagePath,100,100));
            wp_player.Children.Add(LoadImage(player.Cards[1].ImagePath,100,100));
            lb_playerMoney.Content = $"{player.Money}";
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
            List<Card> allCard = new();
            allCard.AddRange(cardsTable);
            allCard.AddRange(hand);
            allCard = allCard.OrderBy(x => x.DefaultValue).ToList();
            List<Card> pairs = new();
            List<Card> drills = new();
            bool isPoker = false;
            int pokerValue = 0;
            Dictionary<string, int> suites = new();
            int counter = 0;
            for (int i = 0; i < allCard.Count - 1; i++)
            {
                if (allCard[i].DefaultValue == allCard[i + 1].DefaultValue)
                {
                    pairs.Add(allCard[i]);
                    counter++;
                }
                else
                {
                    counter = 0;

                }
                if (counter == 2)
                {
                    drills.Add(allCard[i]);
                }
                if (counter == 3)
                {
                    isPoker = true;
                    pokerValue = allCard[i].DefaultValue;
                }
            }
            //this.pairs = pairs.Distinct().Select(x => x.DefaultValue).ToList();

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

            //Royal flush - jó
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

            //Straight flush - jó
            string straightFlushSuit = "";
            foreach (KeyValuePair<string, int> item in suites)
            {
                if (item.Value == 5)
                {
                    straightFlushSuit = item.Key;
                }
            }
            if (straightFlushSuit != "")
            {
                List<Card> straightFlush = new();
                int process = 0;
                for (int i = 0; i < allCard.Count - 1; i++)
                {
                    if (allCard[i + 1].DefaultValue - allCard[i].DefaultValue == 1)
                    {
                        process++;
                        straightFlush.Add(allCard[i]);
                    }
                    else
                    {
                        process = 0;
                        straightFlush.Clear();
                    }

                }
                if (process >= 4)
                {
                    return (9, CardClassHighestValue(straightFlush));
                }
            }

            //Poker - jó
            if (isPoker)
            {
                return (8, pokerValue);
            }
            //Full house
            pairs = pairs.Where(p => !drills.Any(x => x.DefaultValue == p.DefaultValue && x.Suite == p.Suite)).ToList();
            if (pairs.Count >= 1 && drills.Count >= 1)
            {
                List<Card> pairsDrill = new();
                pairsDrill.AddRange(drills);
                pairsDrill.AddRange(pairs);
                return (7, CardClassHighestValue(drills));
            }

            //Flush - jó

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

            //Straight - jó
            List<Card> straight = new();
            int folyamat = 0;
            for (int i = 0; i < allCard.Count - 1; i++)
            {
                if (allCard[i + 1].DefaultValue - allCard[i].DefaultValue == 1)
                {
                    folyamat++;
                    straight.Add(allCard[i]);
                }
                else
                {
                    folyamat = 0;
                    straight.Clear();
                }

            }
            if (folyamat >= 4)
            {
                return (5, CardClassHighestValue(straight));
            }


            //Drill
            if (drills.Count >= 1)
            {
                return (4, CardClassHighestValue(drills));
            }

            //Pair,two pair - jó

            if (pairs.Count == 1)
            {
                return (2, pairs[0].DefaultValue);
            }
            else if (pairs.Count >= 2)
            {

                return (3, CardClassHighestValue(pairs));
            }

            //High card - jó
            return (1, CardClassHighestValue(allCard));
        }
        
        public Card PopRandomCard(List<Card> list)
        {
            int randomnum = random.Next(0, list.Count);
            Card randomcard = list[randomnum];
            list.RemoveAt(randomnum);
            return randomcard;

        }


        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            currentZseton = Convert.ToInt32(ZsetonSlider.Value);
        }
        
        public Image LoadImage(string path, int height, int width)
        {
            Image image = new Image();
            string packUri = $"pic/{path}";
            image.Source = new ImageSourceConverter().ConvertFromString(packUri) as ImageSource;
            image.Height = height;
            image.Width = width;
            return image;
        }
        public List<Bot> GenerateBots(List<Card> cards, int NumberOfBots)
        {
            List<Bot> bots = new List<Bot>();
            for (int i = 0; i < NumberOfBots; i++)
            {
                Bot bot = new Bot(PopRandomCard(cards),PopRandomCard(cards), currentZseton);
                bots.Add(bot);
                FillWrapPanel((WrapPanel)Board.Children[i + 1], bots[i]);
            }
            return bots;
        }
        public void FillWrapPanel(WrapPanel wp, Bot bot)
        {
            wp.Children.Add(LoadImage("Hátlap.gif",100,100));
            wp.Children.Add(LoadImage("Hátlap.gif",100,100));
        }
        private void Check_Click(object sender, RoutedEventArgs e)
        {
            raised = false;
            for (int i = 0; i < bots.Count; i++)
            {
                bots[i].Move(out raised, HandCheck(new List<Card>(), bots[i].Cards));
                Delay(1000);
            }
        }
        public void Delay(int miliseconds)
        {
            var t = Task.Run(async delegate
            {
                await Task.Delay(miliseconds);
            });
            t.Wait();
        }
    }
}
