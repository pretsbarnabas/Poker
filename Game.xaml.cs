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
            List<Card> cards = File.ReadAllLines("cards.txt").Select(x => new Card(x)).ToList();
            List<Bot> bots = GenerateBots(cards);
            for (int i = 0; i < bots.Count; i++)
            {
                FillWrapPanel((WrapPanel)grid_main.Children[i+1], bots[i]);
            }
            wp_player.Children.Add(GenerateImage("pic/1.gif"));
            wp_player.Children.Add(GenerateImage("pic/2.gif"));
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
        public Image GenerateImage(string path)
        {
            Image image = new Image();
            string packUri = $"{path}";
            image.Source = new ImageSourceConverter().ConvertFromString(packUri) as ImageSource;
            image.Height = 100;
            image.Width = 100;
            return image;
        }
        public List<Bot> GenerateBots(List<Card> cards)
        {
            List<Bot> bots = new List<Bot>();
            for (int i = 0; i < 3; i++)
            {
                Bot bot = new Bot(PopRandomCard(cards),PopRandomCard(cards));
                bots.Add(bot);
            }
            return bots;
        }
        public void FillWrapPanel(WrapPanel wp, Bot bot)
        {
            wp.Children.Add(GenerateImage("pic/Hátlap.gif"));
            wp.Children.Add(GenerateImage("pic/Hátlap.gif"));
        }
    }
}
