using System;
using System.Media;
using System.Windows;
using System.Windows.Media.Imaging;
using Chess.Classes;

namespace Chess.Screens
{
    /// <summary>
    /// Interaction logic for HomeScreen.xaml
    /// </summary>
    public partial class HomeScreen : Window
    {
        public HomeScreen()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            Options.ButtonClickPlay();
            StartScreen startScreen = new StartScreen(this);
            startScreen.Show();
            this.Hide();
        }

        private void OptionsButton_Click(object sender, RoutedEventArgs e)
        {
            Options.ButtonClickPlay();
            OptionsScreen optionsScreen = new OptionsScreen(this);
            optionsScreen.Show();
            this.Hide();
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Options.ButtonClickPlay();
            System.Windows.Application.Current.Shutdown();
        }

        private void WhiteButton_Checked(object sender, RoutedEventArgs e)
        {
            if (WhiteButton.IsChecked == true)
                Circle1.Margin = new Thickness(35, 74.5, 0, 0);

            if (KingButton.IsChecked == true)
                PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/king - white - text.png", UriKind.Relative));

            if (QueenButton.IsChecked == true)
                PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/queen - white - text.png", UriKind.Relative));

            if (BishopButton.IsChecked == true)
                PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/bishop - white- text.png", UriKind.Relative));

            if (KnightButton.IsChecked == true)
                PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/knight - white - text.png", UriKind.Relative));

            if (RookButton.IsChecked == true)
                PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/rook - white - text.png", UriKind.Relative));

            if (PawnButton.IsChecked == true)
                PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/pawn - white - text.png", UriKind.Relative));

            KingButton.Content = new BitmapImage(new Uri(@"/Images/ChessPieces/king - white.png", UriKind.Relative));
            QueenButton.Content = new BitmapImage(new Uri(@"/Images/ChessPieces/queen - white.png", UriKind.Relative));
            BishopButton.Content = new BitmapImage(new Uri(@"/Images/ChessPieces/bishop - white.png", UriKind.Relative));
            KnightButton.Content = new BitmapImage(new Uri(@"/Images/ChessPieces/knight - white.png", UriKind.Relative));
            RookButton.Content = new BitmapImage(new Uri(@"/Images/ChessPieces/rook - white.png", UriKind.Relative));
            PawnButton.Content = new BitmapImage(new Uri(@"/Images/ChessPieces/pawn - white.png", UriKind.Relative));
        }

        private void BlackButton_Checked(object sender, RoutedEventArgs e)
        {
            if (BlackButton.IsChecked == true)
                Circle1.Margin = new Thickness(80, 74.5, 0, 0);

            if (KingButton.IsChecked == true)
                PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/king - black - text.png", UriKind.Relative));

            if (QueenButton.IsChecked == true)
                PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/queen - black - text.png", UriKind.Relative));

            if (BishopButton.IsChecked == true)
                PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/bishop - black - text.png", UriKind.Relative));

            if (KnightButton.IsChecked == true)
                PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/knight - black - text.png", UriKind.Relative));

            if (RookButton.IsChecked == true)
                PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/rook - black - text.png", UriKind.Relative));

            if (PawnButton.IsChecked == true)
                PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/pawn - black - text.png", UriKind.Relative));

            KingButton.Content = new BitmapImage(new Uri(@"/Images/ChessPieces/king - black.png", UriKind.Relative));
            QueenButton.Content = new BitmapImage(new Uri(@"/Images/ChessPieces/queen - black.png", UriKind.Relative));
            BishopButton.Content = new BitmapImage(new Uri(@"/Images/ChessPieces/bishop - black.png", UriKind.Relative));
            KnightButton.Content = new BitmapImage(new Uri(@"/Images/ChessPieces/knight - black.png", UriKind.Relative));
            RookButton.Content = new BitmapImage(new Uri(@"/Images/ChessPieces/rook - black.png", UriKind.Relative));
            PawnButton.Content = new BitmapImage(new Uri(@"/Images/ChessPieces/pawn - black.png", UriKind.Relative));
        }

        private void KingButton_Checked(object sender, RoutedEventArgs e)
        {
            if (KingButton.IsChecked == true)
            {
                if (WhiteButton.IsChecked == false && BlackButton.IsChecked == false)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/king - white - text.png", UriKind.Relative));

                Circle2.Width = 70;
                Circle2.Height = 70;
                Circle2.Margin = new Thickness(21, 100, 0, 0);
                if (WhiteButton.IsChecked == true)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/king - white - text.png", UriKind.Relative));
                else if (BlackButton.IsChecked == true)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/king - black - text.png", UriKind.Relative));

                HowDescription.Text = "How to move the king:";
                MoveDescription.Text = "A king can move one square horizontally, vertically, or diagonally unless the square is already occupied by a friendly piece or the move would place the king in check.";
            }
        }

        private void QueenButton_Checked(object sender, RoutedEventArgs e)
        {
            if (QueenButton.IsChecked == true)
            {
                if (WhiteButton.IsChecked == false && BlackButton.IsChecked == false)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/queen - white - text.png", UriKind.Relative));

                Circle2.Width = 70;
                Circle2.Height = 70;
                Circle2.Margin = new Thickness(100, 100, 0, 0);
                if (WhiteButton.IsChecked == true)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/queen - white - text.png", UriKind.Relative));
                else if (BlackButton.IsChecked == true)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/queen - black - text.png", UriKind.Relative));

                HowDescription.Text = "How to move the queen:";
                MoveDescription.Text = "A queen may move any number of unoccupied squares in any direction -- horizontally, vertically, or diagonally, giving her the greatest range of legal moves in the game.";
            }
        }

        private void BishopButton_Checked(object sender, RoutedEventArgs e)
        {
            if (BishopButton.IsChecked == true)
            {
                if (WhiteButton.IsChecked == false && BlackButton.IsChecked == false)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/bishop - white - text.png", UriKind.Relative));

                Circle2.Width = 60;
                Circle2.Height = 60;
                Circle2.Margin = new Thickness(185, 110, 0, 0);
                if (WhiteButton.IsChecked == true)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/bishop - white - text.png", UriKind.Relative));
                else if (BlackButton.IsChecked == true)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/bishop - black - text.png", UriKind.Relative));

                HowDescription.Text = "How to move the bishop:";
                MoveDescription.Text = "A bishop chess piece moves in any direction diagonally. Chess rules state that there is no limit to the number of squares a bishop can travel on the chessboard, as long as there is not another piece obstructing its path.";
            }
        }

        private void KnightButton_Checked(object sender, RoutedEventArgs e)
        {
            if (KnightButton.IsChecked == true)
            {
                if (WhiteButton.IsChecked == false && BlackButton.IsChecked == false)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/knight - white - text.png", UriKind.Relative));

                Circle2.Width = 60;
                Circle2.Height = 60;
                Circle2.Margin = new Thickness(264, 110, 0, 0);
                if (WhiteButton.IsChecked == true)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/knight - white - text.png", UriKind.Relative));
                else if (BlackButton.IsChecked == true)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/knight - black - text.png", UriKind.Relative));

                HowDescription.Text = "How to move the knight:";
                MoveDescription.Text = "A knight can move two squares in any direction vertically followed by one square horizontally, or two squares in any direction horizontally followed by one square vertically.";
            }
        }

        private void RookButton_Checked(object sender, RoutedEventArgs e)
        {
            if (RookButton.IsChecked == true)
            {
                if (WhiteButton.IsChecked == false && BlackButton.IsChecked == false)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/rook - white - text.png", UriKind.Relative));

                Circle2.Width = 60;
                Circle2.Height = 60;
                Circle2.Margin = new Thickness(345, 110, 0, 0);
                if (WhiteButton.IsChecked == true)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/rook - white - text.png", UriKind.Relative));
                else if (BlackButton.IsChecked == true)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/rook - black - text.png", UriKind.Relative));

                HowDescription.Text = "How to move the rook:";
                MoveDescription.Text = "A rook moves horizontally or vertically, through any number of unoccupied squares. The rook cannot jump over pieces. The rook may capture an enemy piece by moving to the square on which the enemy piece stands, removing it from play.";
            }

        }

        private void PawnButton_Checked(object sender, RoutedEventArgs e)
        {
            if (PawnButton.IsChecked == true)
            {
                if (WhiteButton.IsChecked == false && BlackButton.IsChecked == false)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/pawn - white - text.png", UriKind.Relative));

                Circle2.Width = 60;
                Circle2.Height = 60;
                Circle2.Margin = new Thickness(425, 110, 0, 0);
                if (WhiteButton.IsChecked == true)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/pawn - white - text.png", UriKind.Relative));
                else if (BlackButton.IsChecked == true)
                    PieceTextImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/pawn - black - text.png", UriKind.Relative));

                HowDescription.Text = "How to move the pawn:";
                MoveDescription.Text = "A Pawn moves directly forward, never backward or to the side. Pawns move exactly one square forward; though each Pawn may advance two squares forward the first time it is moved. Pawns capture a piece that is one square diagonally forward.";
            }
        }
    }
}