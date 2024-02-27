using Chess.Classes;
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
    /// Interaction logic for PawnPromotionPage.xaml
    /// </summary>
    public partial class PawnPromotionPage : Page
    {
        int color, sqX, sqY, tsX, tsY;
        Frame frame;
        Player player1;
        Player player2;
        public bool IsActive = true;

        public PawnPromotionPage(int color, Frame frame, Player palyer1, Player player2, int sqX, int sqY, int tsX, int tsY)
        {
            InitializeComponent();
            this.color = color;
            this.frame = frame;
            this.player1 = palyer1;
            this.player2 = player2; 
            this.sqX = sqX;
            this.sqY = sqY;
            this.tsX = tsX;
            this.tsY = tsY;

            SetupColor();
        }

        private void SetupColor()
        {
            if ((color & Piece.White) != 0)
            {
                QueenImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/queen - white.png", UriKind.Relative));
                BishopImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/bishop - white.png", UriKind.Relative));
                KnightImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/knight - white.png", UriKind.Relative));
                RookImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/rook - white.png", UriKind.Relative));
            }
            else
            {
                QueenImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/queen - black.png", UriKind.Relative));
                BishopImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/bishop - black.png", UriKind.Relative));
                KnightImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/knight - black.png", UriKind.Relative));
                RookImage.Source = new BitmapImage(new Uri(@"/Images/ChessPieces/rook - black.png", UriKind.Relative));
            }
        }
        
        private void ResetXY()
        {
            Move.StartSquareX = -1;
            Move.StartSquareY = -1;
            Move.TargetSquareX = -1;
            Move.TargetSquareY = -1;
        }

        private void RookImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((color & Piece.White) != 0)
            {
                ChessBoard.Squares[tsX, tsY] = Piece.White | Piece.Rook;
                ChessBoard.GameButtons[tsX, tsY].Content = "/Images/ChessPieces/rook - white.png";
            }
            else
            {
                ChessBoard.Squares[tsX, tsY] = Piece.Black | Piece.Rook;
                ChessBoard.GameButtons[tsX, tsY].Content = "/Images/ChessPieces/rook - black.png";
            }
            frame.Visibility = Visibility.Hidden;
            ResetXY();
        }

        private void KnightImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((color & Piece.White) != 0)
            {
                ChessBoard.Squares[tsX, tsY] = Piece.White | Piece.Knight;
                ChessBoard.GameButtons[tsX, tsY].Content = "/Images/ChessPieces/knight - white.png";
            }
            else
            {
                ChessBoard.Squares[tsX, tsY] = Piece.Black | Piece.Knight;
                ChessBoard.GameButtons[tsX, tsY].Content = "/Images/ChessPieces/knight - black.png";
            }
            frame.Visibility = Visibility.Hidden;
            ResetXY();
        }

        private void BishopImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((color & Piece.White) != 0)
            {
                ChessBoard.Squares[tsX, tsY] = Piece.White | Piece.Bishop;
                ChessBoard.GameButtons[tsX, tsY].Content = "/Images/ChessPieces/bishop - white.png";
            }
            else
            {
                ChessBoard.Squares[tsX, tsY] = Piece.Black | Piece.Bishop;
                ChessBoard.GameButtons[tsX, tsY].Content = "/Images/ChessPieces/bishop - black.png";
            }
            frame.Visibility = Visibility.Hidden;
            ResetXY();
        }

        private void QueenImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if ((color & Piece.White) != 0)
            {
                ChessBoard.Squares[tsX, tsY] = Piece.White | Piece.Queen;
                ChessBoard.GameButtons[tsX, tsY].Content = "/Images/ChessPieces/queen - white.png";
            }
            else
            {
                ChessBoard.Squares[tsX, tsY] = Piece.Black | Piece.Queen;
                ChessBoard.GameButtons[tsX, tsY].Content = "/Images/ChessPieces/queen - black.png";
            }
            frame.Visibility = Visibility.Hidden;
            ResetXY();
        }
    }
}
