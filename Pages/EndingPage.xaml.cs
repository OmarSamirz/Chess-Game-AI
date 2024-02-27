using Chess.Classes;
using Chess.Screens;
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

namespace Chess.Pages
{
    /// <summary>
    /// Interaction logic for EndingPage.xaml
    /// </summary>
    public partial class EndingPage : Page
    {
        private HomeScreen homeScreen;
        private GameBoard gameBoard;
        private readonly bool PlayerOneTurn;
        private readonly bool IsCheckMate;

        public EndingPage(HomeScreen homeScreen, GameBoard gameBoard, bool PlayerOneTurn, bool IsCheckMate)
        {
            InitializeComponent();
            this.homeScreen = homeScreen;
            this.gameBoard = gameBoard;
            this.PlayerOneTurn = PlayerOneTurn;
            this.IsCheckMate = IsCheckMate;

            SettingThingsUp();
        }

        private void SettingThingsUp()
        {
            PlayerOneImage.Source = gameBoard.PlayerTwoImage.Source;
            PlayerTwoImage.Source = gameBoard.PlayerOneImage.Source;
            PlayerOneName.Text = gameBoard.PlayerOneName.Text;
            PlayerTwoName.Text = gameBoard.PlayerTwoName.Text;
            if (IsCheckMate)
            {
                CheckType.Text = "CheckMate";
                if (PlayerOneTurn)
                {
                    WinnerOne.Text = "Lost";
                    WinnerOne.Foreground = Brushes.Red;
                    WinnerTwo.Text = "Won";
                    WinnerTwo.Foreground = Brushes.LightGreen;
                }
                else
                {
                    WinnerOne.Text = "Won";
                    WinnerOne.Foreground = Brushes.Green;
                    WinnerTwo.Text = "Lost";
                    WinnerTwo.Foreground = Brushes.Red;
                }
            }
            else
            {
                CheckType.Text = "StaleMate";
                WinnerOne.Text = "Draw";
                WinnerTwo.Text = "Draw";
            }
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            homeScreen.Show();
            gameBoard.Hide();
            gameBoard.Close();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            GameBoard newGame = new GameBoard(homeScreen, false);
            newGame.Show();
            gameBoard.Hide();
            gameBoard.Close();
        }
    }
}
