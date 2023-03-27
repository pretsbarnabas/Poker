using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using static System.Windows.Controls.Menu;
namespace Poker
{
    /// <summary>
    /// Interaction logic for Game.xaml
    /// </summary>
    public partial class Game : Page
    {
        Random random = new Random();
        int currentZseton;
        int startingMoney;
        List<Bot> bots;
        bool raised;
        int moneyInPlay = 0;
        int baseMoney = 20;
        Bot player;
        bool IsCall = true;
        bool CanAdvance = false;

        public Game()
        {
            InitializeComponent();
            InitalizeStartingMoney();
            int numberofbots = 3;
            List<Card> cards = File.ReadAllLines("cards.txt").Select(x => new Card(x)).ToList();
            bots = GenerateBots(cards,numberofbots);
            GeneratePlayerCards(cards);
            for (int i = 0; i < 4; i++)
            {
                Image img = (LoadImage("10.gif", 50, 50));
                ColumnDefinition c = new();
                grCards.ColumnDefinitions.Add(c);
                Grid.SetColumn(img, i);
                grCards.Children.Add(img);
            }


            AddChips(Menu.settings["Zsetonok"]);
            int playerMoney = Menu.settings["Zsetonok"];
        }

        private void InitalizeStartingMoney()
        {
            startingMoney = Menu.settings["Zsetonok"];
            ZsetonSlider.Maximum = Menu.settings["Zsetonok"];
            ZsetonSlider.Value = Menu.settings["Zsetonok"] / 2;
        }

        private void AddChips(int money)
        {
            int privateMoney = money;
            while (privateMoney != 0)
            {
                int numOfChips = money / 1000;
                privateMoney = privateMoney - numOfChips*1000;
                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("dark_red.png","chips", 50, 50));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(i * 10, 0, 0, 0);
                }    
                numOfChips = privateMoney / 500;
                privateMoney = privateMoney - numOfChips * 500;

                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("red.png", "chips", 50, 50));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(i * 10, 0, 0, 0);
                }
                numOfChips = privateMoney / 200;
                privateMoney = privateMoney - numOfChips * 200;

                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("dark_blue.png", "chips", 50, 50));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(i * 10, 0, 0, 0);
                }
                numOfChips = privateMoney / 100;
                privateMoney = privateMoney - numOfChips * 100;

                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("blue.png", "chips", 50, 50));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(i * 10, 0, 0, 0);
                }
                numOfChips = privateMoney / 50;
                privateMoney = privateMoney - numOfChips * 50;

                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("purple.png", "chips", 50, 50));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(i * 10, 0, 0, 0);
                }
                numOfChips = privateMoney / 25;
                privateMoney = privateMoney - numOfChips * 25;

                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("green.png", "chips", 50, 50));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(i * 10, 0, 0, 0);
                }
                numOfChips = privateMoney / 10;
                privateMoney = privateMoney - numOfChips * 10;

                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("orange.png", "chips", 50, 50));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(i * 10, 0, 0, 0);
                }
                numOfChips = privateMoney / 5;
                privateMoney = privateMoney - numOfChips * 5;

                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("yellow.png", "chips", 50, 50));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(i * 10, 0, 0, 0);
                }
                numOfChips = privateMoney;
                privateMoney = privateMoney - numOfChips;

                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("pink.png", "chips", 50, 50));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(i * 10, 0, 0, 0);
                }
            }
            
            
        }

        private void GeneratePlayerCards(List<Card>cards)
        {
            player = new Bot(PopRandomCard(cards), PopRandomCard(cards), startingMoney, 0);
            wp_player.Children.Add(LoadImage(player.Cards[0].ImagePath,100,100));
            wp_player.Children.Add(LoadImage(player.Cards[1].ImagePath,100,100));
            lb_playermoney.Content = $"{player.Money}";
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Menu.xaml", UriKind.Relative));
        }

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
        public Image LoadImage(string filename, string directory, int height, int width)
        {
            Image image = new Image();
            string packUri = $"{directory}/{filename}";
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
                Bot bot = new Bot(PopRandomCard(cards),PopRandomCard(cards), startingMoney, i+1);
                bots.Add(bot);
                FillWrapPanel((WrapPanel)Board.Children[i + 1], bots[i]);
            }
            return bots;
        }
        public void FillWrapPanel(WrapPanel wp, Bot bot)
        {
            wp.Children.Add(LoadImage("Hátlap.gif",100,100));
            wp.Children.Add(LoadImage("Hátlap.gif",100,100));
            Label label = (Label)wp.Children[0];
            label.Content = $"{bot.Money}";
        }
        private void Check_Click(object sender, RoutedEventArgs e)
        {
            ToggleButtons(false);
            if (IsCall)
            {
                WagerMoney(player);  
            }
            raised = false;
            Thread thread = new Thread(BotsMove); // megse lesz ez a 13. okom
            thread.Start();
        }
        private void Raise_Click(object sender, RoutedEventArgs e)
        {
            ToggleButtons(false);
            baseMoney = currentZseton;
            WagerMoney(player);
            IsCall = true;
            Thread thread = new Thread(BotsMove);
            thread.Start();
        }

        private void WagerMoney(Bot Player)
        {
            moneyInPlay += baseMoney;
            Player.Money -= baseMoney;
            if(Player == player)
            {
                ZsetonSlider.Maximum = player.Money;
            }
            WrapPanel wp = (WrapPanel)Board.Children[Player.Num];
            Label label = (Label)wp.Children[0];
            lb_moneyInPlay.Content = $"{moneyInPlay}";
            label.Content = $"{Player.Money}";
        }

        private void ToggleButtons(bool IsEnabled)
        {
            btn_raise.IsEnabled = IsEnabled;
            btn_check.IsEnabled = IsEnabled;
            btn_fold.IsEnabled = IsEnabled;
        }

        private void BotsMove()
        {
            bool DidTheBotsRaise = false;
            for (int i = 0; i < bots.Count; i++)
            {
                Delay(1000);
                bots[i].Move(out raised, HandCheck(new List<Card>(), bots[i].Cards));
                if (raised)
                {
                    IsCall = true;
                    DidTheBotsRaise = true;
                }
                if (bots[i].Cards.Count == 0)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        WrapPanel wp = (WrapPanel)Board.Children[i + 1];
                        wp.Children.Clear();
                    });
                }
                else if (raised == true)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        if(baseMoney == 20)
                        {
                            baseMoney = random.Next(baseMoney, bots[i].Money / 4);
                        }
                        WagerMoney(bots[i]);
                        btn_check.Content = "Call";
                    });

                }
                else
                {
                    if (IsCall)
                    {
                        this.Dispatcher.Invoke(() =>
                        { 
                            WagerMoney(bots[i]);
                        });
                    }
                }
            }
            if (!DidTheBotsRaise)
            {
                if (!IsCall)
                {
                    CanAdvance = true;
                }
                Dispatcher.Invoke(() =>
                {
                    btn_check.Content = "Check";
                });
                IsCall = false;
                baseMoney = 20;
            }
            if (CanAdvance)
            {
                Debug.WriteLine("Advanced");
            }
            this.Dispatcher.Invoke(() =>
            {
                ToggleButtons(true);
            });
        }
        public void Delay(int milliseconds)
        {
            var t = Task.Run(async delegate
            {
                await Task.Delay(milliseconds);
            });
            t.Wait();
        }
        
        private void gridSizeChange(object sender, SizeChangedEventArgs e)
        {
            double gridWidth = Table.ActualWidth;
            double gridHeight = Table.ActualHeight;
            double cardGridHeight = grCards.ActualHeight;
            double ratio = 0.688;
            Ellipse ellipse = Table.Children[0] as Ellipse;
            ellipse.Width = gridWidth;
            ellipse.Height = gridHeight;
            double height;
            foreach (Image img in grCards.Children)
            {
                
                img.Height = grCards.ActualHeight;
                img.Width = grCards.ActualHeight * ratio;
                height = grCards.ActualHeight;
            }
            foreach (Image img in grid_playerchips.Children)
            {
                img.Height = grid_playerchips.ActualHeight;
                img.Width = grid_playerchips.ActualHeight * ratio;
            }

            foreach (Image img in wp_player.Children)
            {
                img.Height = grCards.ActualHeight;
                img.Width = grCards.ActualHeight * ratio;
            }
            foreach (Image img in wp_bot0.Children)
            {
                img.Height = grCards.ActualHeight;
                img.Width = grCards.ActualHeight * ratio;
            }
            foreach (Image img in wp_bot1.Children)
            {
                img.Height = grCards.ActualHeight;
                img.Width = grCards.ActualHeight * ratio;
            }
            foreach (Image img in wp_bot2.Children)
            {
                img.Height = grCards.ActualHeight;
                img.Width = grCards.ActualHeight * ratio;
            }
            wp_player.HorizontalAlignment = HorizontalAlignment.Center;
            wp_player.VerticalAlignment = VerticalAlignment.Center;
            wp_bot0.HorizontalAlignment = HorizontalAlignment.Center;
            wp_bot0.VerticalAlignment = VerticalAlignment.Center;
            wp_bot1.HorizontalAlignment = HorizontalAlignment.Center;
            wp_bot1.VerticalAlignment = VerticalAlignment.Center;
            wp_bot2.HorizontalAlignment = HorizontalAlignment.Center;
            wp_bot2.VerticalAlignment = VerticalAlignment.Center;
        }
    }
}
