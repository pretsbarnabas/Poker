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
            Bot bot = new Bot(PopRandomCard(cards), PopRandomCard(cards));
            Debug.WriteLine(bot.Cards[0].Suite + bot.Cards[0].DefaultValue);

            ZsetonSlider.Value = Menu.settings["Zsetonok"];
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

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Menu.settings["Zsetonok"] = Convert.ToInt32(ZsetonSlider.Value);
        }
    }
}
