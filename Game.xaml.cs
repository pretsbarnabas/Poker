using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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
        Random random = new Random();
        public Game()
        {
            InitializeComponent();
            int numberofbots = 3;
            List<Card> cards = File.ReadAllLines("cards.txt").Select(x => new Card(x)).ToList();
            List<Bot> bots = GenerateBots(cards,numberofbots);
            GeneratePlayerCards(cards);
        }

        private void GeneratePlayerCards(List<Card>cards)
        {
            Bot player = new Bot(PopRandomCard(cards), PopRandomCard(cards));
            wp_player.Children.Add(LoadImage(player.Cards[0].ImageNumber));
            wp_player.Children.Add(LoadImage(player.Cards[1].ImageNumber));
        }

        private void Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Menu.xaml", UriKind.Relative));
        }
        public Card PopRandomCard(List<Card> list)
        {
            int randomnum = random.Next(0, list.Count);
            Card randomcard = list[randomnum];
            list.RemoveAt(randomnum);
            return randomcard;

        }
        public Image LoadImage(string path)
        {
            Image image = new Image();
            string packUri = $"pic/{path}.gif";
            image.Source = new ImageSourceConverter().ConvertFromString(packUri) as ImageSource;
            image.Height = 100;
            image.Width = 100;
            return image;
        }
        public List<Bot> GenerateBots(List<Card> cards, int NumberOfBots)
        {
            List<Bot> bots = new List<Bot>();
            for (int i = 0; i < NumberOfBots; i++)
            {
                Bot bot = new Bot(PopRandomCard(cards),PopRandomCard(cards));
                bots.Add(bot);
                FillWrapPanel((WrapPanel)grid_main.Children[i + 1], bots[i]);
            }
            return bots;
        }
        public void FillWrapPanel(WrapPanel wp, Bot bot)
        {
            wp.Children.Add(LoadImage("Hátlap"));
            wp.Children.Add(LoadImage("Hátlap"));
        }
    }
}
