using Chess.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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

namespace Chess.Screens
{
    /// <summary>
    /// Interaction logic for StartScreen.xaml
    /// </summary>
    public partial class StartScreen : Window
    {
        HomeScreen homeScreen;
        public StartScreen(HomeScreen homeScreen)
        {
            InitializeComponent();
            this.homeScreen = homeScreen;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Options.ButtonClickPlay();
            homeScreen.Show();
            this.Close();
        }
        private void PlayerButton_Click(object sender, RoutedEventArgs e)
        {
            Options.ButtonClickPlay();    
            GameBoard gameBoard = new GameBoard(homeScreen, false);
            gameBoard.Show();
            this.Close();
        }

        private void ComputerButton_Click(object sender, RoutedEventArgs e)
        {
            Options.ButtonClickPlay();
            GameBoard gameBoard = new GameBoard(homeScreen, true);
            gameBoard.Show();
            this.Close();
        }
    }
}
