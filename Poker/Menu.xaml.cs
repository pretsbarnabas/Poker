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
            public static Dictionary<string, int> settings = new Dictionary<string, int>();
        public Menu()
        {
            if (settings.Count == 0)
            {
            settings.Add("Zsetonok", 2000);     //default settings go here
            }


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
            
            Grid settingsGrid = new Grid();

            ColumnDefinition c1 = new ColumnDefinition();
            c1.Width = new GridLength(1, GridUnitType.Star);
            ColumnDefinition c2 = new ColumnDefinition();
            c2.Width = new GridLength(1, GridUnitType.Star);
            settingsGrid.ColumnDefinitions.Add(c1);
            settingsGrid.ColumnDefinitions.Add(c2);

            RowDefinition r1 = new RowDefinition();
            r1.Height = new GridLength(1, GridUnitType.Star);
            RowDefinition r2 = new RowDefinition();
            r2.Height = new GridLength(1, GridUnitType.Star);
            settingsGrid.RowDefinitions.Add(r1);
            settingsGrid.RowDefinitions.Add(r2);

            SettingsPanel.Children.Add(settingsGrid);

            Label Zsetonok = new Label();
            Zsetonok.Content = "Zsetonok";
            settingsGrid.Children.Add(Zsetonok);
            Grid.SetColumn(Zsetonok, 0);
            Grid.SetRow(Zsetonok, 0);
            Zsetonok.Margin = new Thickness(3);
            TextBox ZsetonSzam = new TextBox();
            ZsetonSzam.HorizontalAlignment= HorizontalAlignment.Right;
            ZsetonSzam.VerticalAlignment = VerticalAlignment.Center;
            ZsetonSzam.Text = settings["Zsetonok"].ToString();
            settingsGrid.Children.Add(ZsetonSzam);
            Grid.SetColumn(ZsetonSzam, 1);
            Grid.SetRow(ZsetonSzam, 0);
            ZsetonSzam.Margin = new Thickness(3);

            Button CloseButton = new Button();
            CloseButton.Content = "Apply and Close";
            //CloseButton.Margin = new Thickness(0, 50, 0, 0);
            settingsGrid.Children.Add(CloseButton);
            Grid.SetColumn(CloseButton, 1);
            Grid.SetRow(CloseButton, 1);
            CloseButton.Margin= new Thickness(3);
            CloseButton.Width = double.NaN;
            CloseButton.Height = 20;
            CloseButton.VerticalAlignment = VerticalAlignment.Bottom;
            CloseButton.HorizontalAlignment = HorizontalAlignment.Left;
            CloseButton.Click += (s, e) => CloseButtonClick(s, e, ZsetonSzam);

            SettingsPanel.Visibility = Visibility.Visible;
        }

        void CloseButtonClick(object sender, RoutedEventArgs e, TextBox ZsetonSzam)
        {
            try
            {
                if (Convert.ToInt32(ZsetonSzam.Text) <= 10000)
                {
                    settings["Zsetonok"] = Convert.ToInt32(ZsetonSzam.Text);
                    SettingsPanel.Visibility= Visibility.Collapsed;
                }
                else
                {
                    MessageBox.Show("Invalid input (<=10000)", ">:(", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid input", ">:(", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
