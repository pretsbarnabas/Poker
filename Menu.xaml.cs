using System;
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
    /// Interaction logic for Menu.xaml
    /// </summary>
    public partial class Menu : Page
    {
            public Dictionary<string, int> settings = new Dictionary<string, int>();
        public Menu()
        {
            settings.Add("Zsetonok", 2000);     //default settings go here


            InitializeComponent();
        }

        private void Play(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("Game.xaml", UriKind.Relative));
        }

        private void Close(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

         public void Settings(object sender, RoutedEventArgs e)
        {
            SettingsPanel.Children.Clear();
            
            Label Zsetonok = new Label();
            Zsetonok.Content = "Zsetonok";
            Zsetonok.HorizontalAlignment = HorizontalAlignment.Left;
            SettingsPanel.Children.Add(Zsetonok);
            TextBox ZsetonSzam = new TextBox();
            ZsetonSzam.HorizontalAlignment= HorizontalAlignment.Right;
            ZsetonSzam.Text = settings["Zsetonok"].ToString();
            int ZsetonDB = int.Parse(ZsetonSzam.Text);
            ZsetonSzam.TextChanged += (sender, e) => ZsetonChanged(sender, e, ZsetonDB);
            SettingsPanel.Children.Add(ZsetonSzam);

            Button CloseButton = new Button();
            CloseButton.Content = "Close";
            CloseButton.Width = 80;
            CloseButton.Height = 20;
            CloseButton.VerticalAlignment = VerticalAlignment.Bottom;
            CloseButton.HorizontalAlignment = HorizontalAlignment.Left;
            CloseButton.AddHandler(Button.ClickEvent, new RoutedEventHandler(CloseButtonClick)); 
            SettingsPanel.Children.Add(CloseButton);

            SettingsPanel.Visibility = Visibility.Visible;
        }

        private void ZsetonChanged(object sender, RoutedEventArgs e, int zsetonDB)
        {
            settings["Zsetonok"] = zsetonDB;
        }

        void CloseButtonClick(object sender, RoutedEventArgs e)
        {
            SettingsPanel.Visibility= Visibility.Collapsed;
        }
    }
}
