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
using System.Windows.Shapes;
using Chess.Classes;

namespace Chess.Screens
{
    /// <summary>
    /// Interaction logic for OptionsScreen.xaml
    /// </summary>
    public partial class OptionsScreen : Window
    {
        readonly HomeScreen homeScreen;
        public OptionsScreen(HomeScreen homeScreen)
        {
            InitializeComponent();
            RadioButtons_Checking();
            this.homeScreen = homeScreen;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Options.ButtonClickPlay();
            homeScreen.Show();
            this.Close();
        }
        private void RadioButtons_Checking()
        {
            if (Options.Music) 
            { 
                MusicOnButton.Background = new SolidColorBrush(Color.FromRgb(41, 182, 255));
                MusicOnButton.BorderThickness = new Thickness(0.0);
            }
            else
            {
                MusicOffButton.Background = new SolidColorBrush(Color.FromRgb(41, 182, 255));
                MusicOffButton.BorderThickness = new Thickness(0.0);
            }
            if (Options.Sounds)
            {
                SoundsOnButton.Background = new SolidColorBrush(Color.FromRgb(41, 182, 255));
                SoundsOnButton.BorderThickness = new Thickness(0.0);
            }
            else
            {
                SoundsOffButton.Background = new SolidColorBrush(Color.FromRgb(41, 182, 255));
                SoundsOffButton.BorderThickness = new Thickness(0.0);
            }
            if (Options.Difficulity == "Easy")
            {
                EasyButton.Background = new SolidColorBrush(Color.FromRgb(41, 182, 255));
                EasyButton.BorderThickness = new Thickness(0.0);
            }
            else
            {
                MediumButton.Background = new SolidColorBrush(Color.FromRgb(41, 182, 255));
                MediumButton.BorderThickness = new Thickness(0.0);
            }
        }

        private void MusicOnButton_Click(object sender, RoutedEventArgs e)
        {
            Options.Music = true;
            MusicOnButton.Background = new SolidColorBrush(Color.FromRgb(41, 182, 255));
            MusicOnButton.BorderThickness = new Thickness(0.0);

            MusicOffButton.Background = Brushes.Transparent;
            MusicOffButton.BorderThickness = new Thickness(4.0);
        }

        private void MusicOffButton_Click(object sender, RoutedEventArgs e)
        {
            Options.Music = false;
            MusicOffButton.Background = new SolidColorBrush(Color.FromRgb(41, 182, 255));
            MusicOffButton.BorderThickness = new Thickness(0.0);

            MusicOnButton.Background = Brushes.Transparent;
            MusicOnButton.BorderThickness = new Thickness(4.0);
        }

        private void SoundsOffButton_Click(object sender, RoutedEventArgs e)
        {
            Options.Sounds = false;
            SoundsOffButton.Background = new SolidColorBrush(Color.FromRgb(41, 182, 255));
            SoundsOffButton.BorderThickness = new Thickness(0.0);

            SoundsOnButton.Background = Brushes.Transparent;
            SoundsOnButton.BorderThickness = new Thickness(4.0);
        }

        private void SoundsOnButton_Click(object sender, RoutedEventArgs e)
        {
            Options.Sounds = true;
            SoundsOnButton.Background = new SolidColorBrush(Color.FromRgb(41, 182, 255));
            SoundsOnButton.BorderThickness = new Thickness(0.0);

            SoundsOffButton.Background = Brushes.Transparent;
            SoundsOffButton.BorderThickness = new Thickness(4.0);
        }

        private void MediumButton_Click(object sender, RoutedEventArgs e)
        {
            AIPlayer.DEPTH = 2;
            Options.Difficulity = "Medium";
            MediumButton.Background = new SolidColorBrush(Color.FromRgb(41, 182, 255));
            MediumButton.BorderThickness = new Thickness(0.0);

            EasyButton.Background = Brushes.Transparent;
            EasyButton.BorderThickness = new Thickness(4.0);
        }

        private void EasyButton_Click(object sender, RoutedEventArgs e)
        {
            AIPlayer.DEPTH = 1;
            Options.Difficulity = "Easy";
            EasyButton.Background = new SolidColorBrush(Color.FromRgb(41, 182, 255));
            EasyButton.BorderThickness = new Thickness(0.0);

            MediumButton.Background = Brushes.Transparent;
            MediumButton.BorderThickness = new Thickness(4.0);
        }
    }
}
