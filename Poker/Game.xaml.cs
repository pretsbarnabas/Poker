using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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
        int startingMoney;
        int numberofbots;
        int raisedMoney = 0;
        List<Bot> bots;
        List<Card> cards;
        double ratio = 0.688;
        bool raised;
        int moneyInPlay = 0;
        int baseMoney = 20;
        int turnPhase = 1;
        Bot player;
        Bot dealer;
        bool IsCall = true;
        bool CanAdvance = false;
        bool isPlayerFolded = false;

        public Game()
        {
            InitializeComponent();
            InitalizeStartingMoney();
            numberofbots = 3;
            cards = File.ReadAllLines("cards.txt").Select(x => new Card(x)).ToList();
            bots = GenerateBots(cards,numberofbots);
            GeneratePlayerCards(cards);
            dealer = new Bot(0);
            AddChips(Menu.settings["Zsetonok"]);
            int playerMoney = Menu.settings["Zsetonok"];
        }

        private void InitalizeStartingMoney()
        {
            startingMoney = Menu.settings["Zsetonok"];
            ZsetonSlider.Maximum = Menu.settings["Zsetonok"];
            ZsetonSlider.Minimum = baseMoney;
            ZsetonSlider.Value = baseMoney;
        }

        private void AddChips(int money)
        {
            
            grid_playerchips.Children.Clear();
            int privateMoney = money;
            int coinNumber = 0;
            while (privateMoney != 0)
            {
                int numOfChips = money / 1000;
                privateMoney = privateMoney - numOfChips * 1000;
                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("dark_red.png", "chips", wp_responsive.ActualHeight*ratio, (wp_responsive.ActualWidth * ratio) / 1.2));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(coinNumber * ((wp_responsive.ActualWidth * ratio) / 7), 0, 0, 0);
                    coinNumber++;
                }
                numOfChips = privateMoney / 500;
                privateMoney = privateMoney - numOfChips * 500;

                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("red.png", "chips", wp_responsive.ActualHeight*ratio, (wp_responsive.ActualWidth * ratio) / 1.2));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(coinNumber * ((wp_responsive.ActualWidth * ratio) / 7), 0, 0, 0);
                    coinNumber++;
                }
                numOfChips = privateMoney / 200;
                privateMoney = privateMoney - numOfChips * 200;

                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("dark_blue.png", "chips", wp_responsive.ActualHeight * ratio, (wp_responsive.ActualWidth * ratio) / 1.2));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(coinNumber * ((wp_responsive.ActualWidth * ratio) / 7), 0, 0, 0);
                    coinNumber++;
                }
                numOfChips = privateMoney / 100;
                privateMoney = privateMoney - numOfChips * 100;

                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("blue.png", "chips", wp_responsive.ActualHeight * ratio, (wp_responsive.ActualWidth * ratio) / 1.2));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(coinNumber * ((wp_responsive.ActualWidth * ratio) / 7), 0, 0, 0);
                    coinNumber++;
                }
                numOfChips = privateMoney / 50;
                privateMoney = privateMoney - numOfChips * 50;

                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("purple.png", "chips", wp_responsive.ActualHeight * ratio, (wp_responsive.ActualWidth * ratio) / 1.2));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(coinNumber * ((wp_responsive.ActualWidth * ratio) / 7), 0, 0, 0);
                    coinNumber++;
                }
                numOfChips = privateMoney / 25;
                privateMoney = privateMoney - numOfChips * 25;

                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("green.png", "chips", wp_responsive.ActualHeight * ratio, (wp_responsive.ActualWidth * ratio) / 1.2));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(coinNumber * ((wp_responsive.ActualWidth * ratio) / 7), 0, 0, 0);
                    coinNumber++;
                }
                numOfChips = privateMoney / 10;
                privateMoney = privateMoney - numOfChips * 10;

                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("orange.png", "chips", wp_responsive.ActualHeight * ratio, (wp_responsive.ActualWidth * ratio) / 1.2));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(coinNumber * ((wp_responsive.ActualWidth * ratio) / 7), 0, 0, 0);
                    coinNumber++;
                }
                numOfChips = privateMoney / 5;
                privateMoney = privateMoney - numOfChips * 5;

                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("yellow.png", "chips", wp_responsive.ActualHeight*ratio, (wp_responsive.ActualWidth * ratio) / 1.2));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(coinNumber * ((wp_responsive.ActualWidth * ratio) / 7), 0, 0, 0);
                    coinNumber++;
                }
                numOfChips = privateMoney;
                privateMoney = privateMoney - numOfChips;

                for (int i = 0; i < numOfChips; i++)
                {
                    grid_playerchips.Children.Add(LoadImage("pink.png", "chips", wp_responsive.ActualHeight * ratio, (wp_responsive.ActualWidth * ratio) / 1.2));
                    Image image = (Image)grid_playerchips.Children[i];
                    image.Margin = new Thickness(coinNumber * ((wp_responsive.ActualWidth * ratio) / 7), 0, 0, 0);
                    coinNumber++;
                }
            }
            int coinCounter = 0;
            foreach (Image img in grid_playerchips.Children)
            {
                img.Height = wp_responsive.ActualHeight * ratio;
                img.Width = (wp_responsive.ActualWidth * ratio) / 1.2;
                img.Margin = new Thickness(coinCounter * ((wp_responsive.ActualWidth * ratio) / 7), 0, 0, 0);
                coinCounter++;
            }




        }

        private void GiveNewCards()
        {
            for (int i = 0; i < 2; i++)
            {
                player.Cards.Add(PopRandomCard(cards));
                foreach (Bot bot in bots)
                {
                    bot.Cards.Add(PopRandomCard(cards));
                }
            }
            wp_player.Children.Add(LoadImage(player.Cards[0].ImagePath, wp_responsive.ActualHeight, (wp_responsive.ActualWidth * ratio) / 3.5));
            wp_player.Children.Add(LoadImage(player.Cards[1].ImagePath, wp_responsive.ActualHeight, (wp_responsive.ActualWidth * ratio) / 3.5));
            lb_playermoney.Content = $"{player.Money}";
            for (int i = 0; i < bots.Count; i++)
            {
                FillWrapPanel((WrapPanel)Board.Children[i + 1], bots[i], true);
            }
        }
        
        private void GeneratePlayerCards(List<Card> cards)
        {
            player = new Bot(PopRandomCard(cards), PopRandomCard(cards), startingMoney, 0);
            wp_player.Children.Add(LoadImage(player.Cards[0].ImagePath, 100, 100));
            wp_player.Children.Add(LoadImage(player.Cards[1].ImagePath, 100, 100));
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

        private Bot GetWinner(List<Bot> allPlayer)
        {
            Bot winner = null;
            (int, int) highestHand = (0, 0);

            for (int i = 0; i < allPlayer.Count; i++)
            {
                (int, int) actualHandStrength = HandCheck(dealer.Cards, allPlayer[i].Cards);
                if (actualHandStrength.Item1 > highestHand.Item1)
                {
                    highestHand = actualHandStrength;
                    winner = allPlayer[i];
                }
                else if (actualHandStrength.Item1 == highestHand.Item1 && actualHandStrength.Item2 > highestHand.Item2)
                {
                    highestHand = actualHandStrength;
                    winner = allPlayer[i];
                }
            }

            return winner;
        }

        private void GameEnd(Bot winner)
        {
            winner.Money += int.Parse(lb_moneyInPlay.Content.ToString());
            lb_moneyInPlay.Content = '0';
            WrapPanel wp = (WrapPanel)Board.Children[winner.Num];
            Label lb = (Label)wp.Children[0];
            AddChips(int.Parse(lb_playermoney.Content.ToString()));
            lb.Content = "Winner";
           
        }
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            currentZseton = Convert.ToInt32(ZsetonSlider.Value);
        }

        public Image LoadImage(string path, double height, double width)
        {
            Image image = new Image();
            string packUri = $"pic/{path}";
            image.Source = new ImageSourceConverter().ConvertFromString(packUri) as ImageSource;
            image.Height = height;
            image.Width = width;
            return image;
        }
        public Image LoadImage(string filename, string directory, double height, double width)
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
                Bot bot = new Bot(PopRandomCard(cards), PopRandomCard(cards), startingMoney, i + 1);
                bots.Add(bot);
                FillWrapPanel((WrapPanel)Board.Children[i + 1], bots[i], false);
            }
            return bots;
        }
        public void FillWrapPanel(WrapPanel wp, Bot bot, bool responsive)
        {
            if (responsive)
            {
                wp.Children.Add(LoadImage("Hátlap.gif", wp_responsive.ActualHeight, (wp_responsive.ActualWidth * ratio) / 3.5));
                wp.Children.Add(LoadImage("Hátlap.gif", wp_responsive.ActualHeight, (wp_responsive.ActualWidth * ratio) / 3.5));
            }
            else
            {
            wp.Children.Add(LoadImage("Hátlap.gif", 100, 100));
            wp.Children.Add(LoadImage("Hátlap.gif", 100, 100));
            }
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
            AddChips(int.Parse(lb_playermoney.Content.ToString()));

        }
        private void Raise_Click(object sender, RoutedEventArgs e)
        {
            if (player.Money>0)
            {
                    ToggleButtons(false);
                    baseMoney = currentZseton;
                    WagerMoney(player);
                    IsCall = true;
                    Thread thread = new Thread(BotsMove);
                    thread.Start();
                    AddChips(int.Parse(lb_playermoney.Content.ToString()));

            }
        }
        private void Fold_Click(object sender, RoutedEventArgs e)
        {
            ToggleButtons(false);
            wp_player.Children.RemoveRange(1, wp_player.Children.Count);
            raised = false;
            isPlayerFolded= true;
            Thread thread = new Thread(BotsMove);
            thread.Start();
        }

        private void WagerMoney(Bot Player)
        {
            if (raisedMoney>0)
            {
                if (player.Money-raisedMoney>0)
                {
                    moneyInPlay += raisedMoney;
                    Player.Money -= raisedMoney;
                    if (Player == player)
                    {
                        ZsetonSlider.Maximum = player.Money;
                    }
                    WrapPanel wp = (WrapPanel)Board.Children[Player.Num];
                    Label label = (Label)wp.Children[0];
                    lb_moneyInPlay.Content = $"{moneyInPlay}";
                    label.Content = $"{Player.Money}";         
                }
                else if (player.Money-raisedMoney<0)
                {
                    moneyInPlay += player.Money;
                    Player.Money -= player.Money;
                    if (Player == player)
                    {
                        ZsetonSlider.Maximum = player.Money;
                    }
                    WrapPanel wp = (WrapPanel)Board.Children[Player.Num];
                    Label label = (Label)wp.Children[0];
                    lb_moneyInPlay.Content = $"{moneyInPlay}";
                    label.Content = $"{Player.Money}";
                    raised = false;

                }
            }
            else
            {
                if (player.Money>=20)
                {
                    moneyInPlay += baseMoney;
                    Player.Money -= baseMoney;
                    if (Player == player)
                    {
                        ZsetonSlider.Maximum = player.Money;
                    }
                    WrapPanel wp = (WrapPanel)Board.Children[Player.Num];
                    Label label = (Label)wp.Children[0];
                    lb_moneyInPlay.Content = $"{moneyInPlay}";
                    label.Content = $"{Player.Money}";
                    ZsetonSliderValue.Content = player.Money;

                }
                else
                {
                    moneyInPlay += player.Money;
                    Player.Money -= player.Money;
                    if (Player == player)
                    {
                        ZsetonSlider.Maximum = player.Money;
                    }
                    WrapPanel wp = (WrapPanel)Board.Children[Player.Num];
                    Label label = (Label)wp.Children[0];
                    lb_moneyInPlay.Content = $"{moneyInPlay}";
                    label.Content = $"{Player.Money}";
                    ZsetonSliderValue.Content = player.Money;
                }
            }
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
                bots[i].Move(out raised, HandCheck(dealer.Cards, bots[i].Cards));
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
                        wp.Children.RemoveRange(1,2);
                    });
                }
                else if (raised == true)
                {
                    this.Dispatcher.Invoke(() =>
                    {
                        if (baseMoney == 20)
                        {
                            baseMoney = random.Next(baseMoney, bots[i].Money / 4);
                            raisedMoney= baseMoney;
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
                else
                {
                    CanAdvance = false;
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
                bool isItOver = false;
                Dispatcher.Invoke(() =>
                {
                    if (wp_dealer.Children.Count == 5)
                    {
                        Debug.WriteLine("End");
                        Thread thread = new Thread(ResetRound);
                        thread.Start();
                        isItOver = true;
                    }
                });
                if (isItOver) { return; }
                Dispatcher.Invoke(() =>
                {
                    DealerTurn();
                });
            }
            this.Dispatcher.Invoke(() =>
            {
                ZsetonSlider.Minimum = baseMoney;
                if(wp_player.Children.Count == 1)
                {
                    Thread thread = new Thread(BotsMove);
                    thread.Start();
                    return;
                }
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

        public void DealerTurn()
        {
            int numOfCards = 0;
            switch (turnPhase)
            {
                case 1:
                    numOfCards = 3;
                    break;
                case 2:
                    numOfCards = 1;
                    break;
                case 3:
                    numOfCards = 1;
                    break;
            }
            turnPhase++;
            for (int i = 0; i < numOfCards; i++)
            {
                dealer.Cards.Add(PopRandomCard(cards));
            }
            wp_dealer.Children.Clear();
            foreach (Card card in dealer.Cards)
            {
                wp_dealer.Children.Add(LoadImage(card.ImagePath, wp_responsive.ActualHeight, (wp_responsive.ActualWidth * ratio) / 3.5));
            }
        }

        public void ResetRound()
        {
            Dispatcher.Invoke(RevealCards);
            Delay(5000);
            Dispatcher.Invoke(ResetCards);
            Delay(1000);
            turnPhase = 1;
            IsCall = true;
            CanAdvance = false;
            moneyInPlay = 0;
            Dispatcher.Invoke(() =>
            {
                lb_moneyInPlay.Content = "0";
                GiveNewCards();
                ToggleButtons(true);
                btn_check.Content = "Call";
            });
        }

        private void RevealCards()
        {
            List<Bot> players = new(bots);
            if (!isPlayerFolded)
            {
            players.Add(player);
            }
            Bot wnr = GetWinner(players);
            GameEnd(wnr);
            for (int i = 0; i < bots.Count; i++)
            {
                WrapPanel wp = (WrapPanel)Board.Children[i + 1];
                wp.Children.RemoveRange(1, bots[i].Cards.Count);
                for (int j = 0; j < 2; j++)
                {
                    if (bots[i].Cards.Count > 0)
                    {
                        wp.Children.Add(LoadImage(bots[i].Cards[j].ImagePath, wp_responsive.ActualHeight, (wp_responsive.ActualWidth * ratio) / 3.5));
                    }
                }
            }
            isPlayerFolded= false;
        }

        public void ResetCards()
        {
            
            for (int i = 0; i < bots.Count; i++)
            {
                WrapPanel wp = (WrapPanel)Board.Children[i + 1];
                wp.Children.RemoveRange(1, bots[i].Cards.Count);
            }
            foreach (Bot bot in bots)
            {
                bot.Cards.Clear();
            }
            wp_player.Children.RemoveRange(1, player.Cards.Count);
            player.Cards.Clear();
            dealer.Cards.Clear();
            wp_dealer.Children.Clear();
            cards = File.ReadAllLines("cards.txt").Select(x => new Card(x)).ToList();
        }
        
        private void gridSizeChange(object sender, SizeChangedEventArgs e)
        {
            double gridWidth = Table.ActualWidth;
            double gridHeight = Table.ActualHeight;
            double cardGridHeight = wp_dealer.ActualHeight;
            double ratio = 0.688;
            Ellipse ellipse = Table.Children[0] as Ellipse;
            ellipse.Width = gridWidth;
            ellipse.Height = gridHeight;
            double height;

            if (wp_player.Children.Count>1)
            {
                for (int i = 0; i < 3; i++)
                {
                    string[] typeArray = wp_player.Children[i].GetType().ToString().Split('.');
                    string type = typeArray[typeArray.Length - 1];
                    if (type == "Image")
                    {
                        Image img = wp_player.Children[i] as Image;
                        img.Height = wp_responsive.ActualHeight;
                        img.Width = img.ActualWidth * (ratio+1);
                    }
                    else if (type == "Label")
                    {
                        Label lbl = wp_player.Children[i] as Label;
                        lbl.FontSize = (wp_responsive.ActualWidth * ratio) / 20;

                    }
                }
            }
            if (wp_bot0.Children.Count>1)
            {
                for (int i = 0; i < 3; i++)
                {
                    string[] typeArray = wp_bot0.Children[i].GetType().ToString().Split('.');
                    string type = typeArray[typeArray.Length - 1];
                    if (type == "Image")
                    {
                        Image img = wp_bot0.Children[i] as Image;
                        img.Height = wp_responsive.ActualHeight;
                        img.Width = img.ActualWidth * (ratio + 1);
                    }
                    else if (type == "Label")                                 
                    {
                        Label lbl = wp_bot0.Children[i] as Label;
                        lbl.FontSize = (wp_responsive.ActualWidth * ratio) / 20;
                    }
                }
            }

            if (wp_bot1.Children.Count>1)
            {
                for (int i = 0; i < 3; i++)
                {
                    string[] typeArray = wp_bot1.Children[i].GetType().ToString().Split('.');
                    string type = typeArray[typeArray.Length - 1];
                    if (type == "Image")
                    {
                        Image img = wp_bot1.Children[i] as Image;
                        img.Height = wp_responsive.ActualHeight;
                        img.Width = img.ActualWidth * (ratio + 1);
                    }
                    else if (type == "Label")
                    {
                        Label lbl = wp_bot1.Children[i] as Label;
                        lbl.FontSize = (wp_responsive.ActualWidth * ratio) / 20;
                    }
                }

            }
            if (wp_bot2.Children.Count>1)
            {
                for (int i = 0; i < 3; i++)
                {
                    string[] typeArray = wp_bot2.Children[i].GetType().ToString().Split('.');
                    string type = typeArray[typeArray.Length - 1];
                    if (type == "Image")
                    {
                        Image img = wp_bot2.Children[i] as Image;
                        img.Height = wp_responsive.ActualHeight;
                        img.Width = img.ActualWidth * (ratio + 1);
                    }
                    else if (type == "Label")
                    {
                        Label lbl = wp_bot2.Children[i] as Label;
                        lbl.FontSize = (wp_responsive.ActualWidth * ratio) / 20;
                    }
                }

            }
            foreach (Image img in wp_dealer.Children)
            {
                img.Height = wp_responsive.ActualHeight;
                img.Width =(wp_responsive.ActualWidth * ratio)/3.5;
            }
            int coinCounter = 0;
            foreach (Image img in grid_playerchips.Children)
            {
                img.Height = wp_responsive.ActualHeight*ratio;
                img.Width = (wp_responsive.ActualWidth * ratio) / 1.2;
                img.Margin = new Thickness(coinCounter* ((wp_responsive.ActualWidth * ratio) / 7), 0,0,0);
                coinCounter++;
            }
            lb_moneyInPlay.FontSize = (wp_responsive.ActualWidth * ratio) / 20;
            lb_moneyInPlay.VerticalAlignment = VerticalAlignment.Bottom;
            lb_moneyInPlay.HorizontalAlignment = HorizontalAlignment.Center;
            wp_player.HorizontalAlignment = HorizontalAlignment.Stretch;
            wp_player.Width = wp_responsive.ActualWidth * ratio;
            wp_player.VerticalAlignment = VerticalAlignment.Center;
            wp_bot0.HorizontalAlignment = HorizontalAlignment.Stretch;
            wp_bot0.VerticalAlignment = VerticalAlignment.Center;
            wp_bot0.Width = wp_responsive.ActualWidth * ratio;
            wp_bot1.HorizontalAlignment = HorizontalAlignment.Stretch;
            wp_bot1.VerticalAlignment = VerticalAlignment.Center;
            wp_bot1.Width = wp_responsive.ActualWidth * ratio;
            wp_bot2.HorizontalAlignment = HorizontalAlignment.Stretch;
            wp_bot2.VerticalAlignment = VerticalAlignment.Center;
            wp_bot2.Width = wp_responsive.ActualWidth * ratio;

        }
        public UIElement ColorChange(UIElement ui)
        {
            Random random = new Random();
            byte red = (byte)random.Next(255);
            byte green = (byte)random.Next(255);
            byte blue = (byte)random.Next(255);

            // Create a SolidColorBrush with the random color
            SolidColorBrush brush = new SolidColorBrush(Color.FromRgb(red, green, blue));

            // Set the background color of a UI element to the random color
            ui.SetValue(BackgroundProperty, brush);
            return ui;
        }
    }
}
