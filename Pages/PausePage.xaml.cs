using Chess.Classes;
using Chess.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Channels;
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

namespace Chess.Pages
{
    /// <summary>
    /// Interaction logic for PausePage.xaml
    /// </summary>
    public partial class PausePage : Page
    {
        HomeScreen homeScreen;
        GameBoard gameBoard;
        Frame frame;
        public PausePage(HomeScreen homeScreen, GameBoard gameBoard, Frame frame)
        {
            InitializeComponent();
            this.homeScreen = homeScreen;
            this.gameBoard = gameBoard;
            this.frame = frame;
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            frame.Visibility = Visibility.Hidden;
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            GameBoard newGame = new GameBoard(homeScreen, gameBoard.IsAI);
            newGame.Show();
            gameBoard.Hide();
            gameBoard.Close();
        }

        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            homeScreen.Show();
            gameBoard.Hide();
            gameBoard.Close();
        }
    }
}
