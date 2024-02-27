using Chess.Classes;
using Chess.Pages;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Cryptography;
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
using System.Windows.Threading;
using System.Xml;
using System.Xml.Linq;

namespace Chess.Screens
{
    /// <summary>
    /// Interaction logic for GameBoard.xaml
    /// </summary>
    public partial class GameBoard : Window
    {
        Player player1;
        Player player2;
        AIPlayer Computer;
        HomeScreen homeScreen;
        public bool IsAI;
        public static Frame frame = new Frame();
        public static WrapPanel playerOneWrapPanel = new WrapPanel();
        public static WrapPanel playerTwoWrapPanel = new WrapPanel();
        public static GameBoard? CurrentGameBoard;

        public GameBoard(HomeScreen homeScreen, bool IsAI)
        {
            InitializeComponent();
            Move.ResetAll();
            ChessBoard.ResetAll();
            this.homeScreen = homeScreen;
            this.IsAI = IsAI;
            Move.ChessGrid = ChessGrid;
            CurrentGameBoard = this;

            player1 = (IsAI)? new Player("Player", true, Piece.White) : new Player("Player 1", true, Piece.White);
            player2 = new Player("Player 2", false, Piece.Black);
            Computer = new AIPlayer("AI", false, Piece.Black, player1, player2);

            CreatePlayers(IsAI);
            runLoop();
            ChessBoard.LoadPositionsFen();
            frame = Frame;
            playerOneWrapPanel = this.PlayerOneWrapPanel;
            playerTwoWrapPanel = this.PlayertwoWrapPanel;
            if (IsAI)
                PlayerTwoImage.Source = new BitmapImage(new Uri(@"/Images/Icons/AIHead.png", UriKind.Relative));
            else
                PlayerTwoImage.Source = new BitmapImage(new Uri(@"/Images/Icons/head1.png", UriKind.Relative));
        }
        
        private void CreatePlayers(bool IsAI)
        {
            if (!IsAI)
            {
                PlayerOneName.Text = player1.Name;
                PlayerTwoName.Text = player2.Name;
            }
            else
            {
                PlayerOneName.Text = player1.Name;
                PlayerTwoName.Text = Computer.Name;
            }
        }

        public void runLoop()
        {
            int i = 0;
            int j = 0;
            foreach (Button btn in ChessGrid.Children.OfType<Button>())
            {
                if (j <= 7)
                {
                    ChessBoard.GameButtons[i, j] = btn;
                    j++;
                }
                else
                {
                    i++;
                    j = 0;
                    ChessBoard.GameButtons[i, j] = btn;
                    j++;
                }
            }
        }

        public static void remove_Background(Button btn)
        {
            if ((Grid.GetRow(btn) % 2 == 0 && Grid.GetColumn(btn) % 2 == 0) || (Grid.GetRow(btn) % 2 != 0 && Grid.GetColumn(btn) % 2 != 0))
                btn.Background = Brushes.White;

            else
                btn.Background = Brushes.Transparent;
        }

        public static void add_Background(Button btn)
        {
            btn.Background = new SolidColorBrush(Color.FromArgb(100, 15, 182, 255));
        }

        private void MakePausePage()
        {
            PausePage pausePage = new PausePage(homeScreen, this, Frame);
            Frame.Content = pausePage;
            Frame.Visibility = Visibility.Hidden;
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Options.MusicPlayer != null)
                Options.MusicPlay();

            Button btn = (Button)sender;
            Player.Check_Turn(btn, Circle, player1, player2, Computer, IsAI);
            if (Move.CheckMate || Move.StaleMate)
            {
                CheckType.Text = (Move.CheckMate) ? "Checkmate" : "Stalemate";
                NewGameButton.Visibility = Visibility.Visible;
                HomeButton.Visibility = Visibility.Visible;
                /*                EndingPage Ending = new EndingPage(homeScreen, this, (player1.Turn)? true : false, (Move.CheckMate)? true : false);
                                Frame.Content = Ending;
                                Panel.SetZIndex(Frame, 3);
                                Frame.Visibility = Visibility.Visible;*/
                return;
            }

            int Column = (player1.Turn == true) ? 0 : 2;
            int MarginLeft = (player1.Turn == true) ? 0 : 7;
            Grid.SetColumn(Circle, Column);
            Circle.Margin = new Thickness(MarginLeft, 30, 5, 0);

            if (IsAI && player1.Turn == false)
            {
                await Task.Delay((AIPlayer.DEPTH <= 2 || AIPlayer.OpeningCounter < 2) ? 500 : 100);
                Player.Check_Turn(btn, Circle, player1, player2, Computer, IsAI);
                Column = (player1.Turn == true) ? 0 : 2;
                MarginLeft = (player1.Turn == true) ? 0 : 7;
                Grid.SetColumn(Circle, Column);
                Circle.Margin = new Thickness(MarginLeft, 30, 5, 0);
                if (Move.CheckMate || Move.StaleMate)
                {
                    CheckType.Text = (Move.CheckMate) ? "Checkmate" : "Stalemate";
                    NewGameButton.Visibility = Visibility.Visible;
                    HomeButton.Visibility = Visibility.Visible;
                }
            }
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MakePausePage();
            Options.ButtonClickPlay();
            Panel.SetZIndex(Frame, 3);
            Frame.Visibility = Visibility.Visible;
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            GameBoard newGame = new GameBoard(homeScreen, IsAI);
            newGame.Show();
            this.Hide();
            this.Close();
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            homeScreen.Show();
            this.Hide();
            this.Close();
        }
    }
}
