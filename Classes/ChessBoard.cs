using Chess.Screens;
using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Security.Policy;
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

namespace Chess.Classes
{
    public static class ChessBoard
    {
        private const string startFen = "rnbkqbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBKQBNR w KQkq - 0 1";
        public static int[,] Squares = new int[8, 8];
        public static Button[,] GameButtons = new Button[8, 8];

        private static GameBoard gameWindow = Application.Current.Windows.OfType<GameBoard>().FirstOrDefault(w => w.Title == "GameBoard");

        public static void ResetAll()
        {
            Array.Clear(Squares, Piece.None, Squares.Length);
            Array.Clear(GameButtons, 0, GameButtons.Length);
        }

        public static void LoadPositionsFen()
        {
            var PieceTypeFromSymbol = new Dictionary<char, int>()
            {
                ['k'] = Piece.King, ['p'] = Piece.Pawn, ['n'] = Piece.Knight,
                ['b'] = Piece.Bishop, ['r'] = Piece.Rook, ['q'] = Piece.Queen
            };

            string[] fileName = { "/Images/ChessPieces/king - white.png", "/Images/ChessPieces/queen - white.png", "/Images/ChessPieces/bishop - white.png", "/Images/ChessPieces/knight - white.png",
                                  "/Images/ChessPieces/rook - white.png", "/Images/ChessPieces/pawn - white.png", "/Images/ChessPieces/king - black.png", "/Images/ChessPieces/queen - black.png",
                                  "/Images/ChessPieces/bishop - black.png", "/Images/ChessPieces/knight - black.png", "/Images/ChessPieces/rook - black.png", "/Images/ChessPieces/pawn - black.png"};

            string fenBoard = startFen.Split(' ')[0];
            int rank = 0, file = 7;

            foreach(char symbol in fenBoard)
            {
                if (symbol == '/')
                {
                    file = 7;
                    rank++;
                }
                else
                {
                    if (char.IsDigit(symbol))
                        file += (int)char.GetNumericValue(symbol);

                    else
                    {
                        int pieceColor = (char.IsUpper(symbol)) ? Piece.White : Piece.Black;
                        int minus = (pieceColor == Piece.White) ? 9 : 11;
                        int pieceType = PieceTypeFromSymbol[char.ToLower(symbol)];
                        int pieceSize = Squares[rank, file] = pieceType | pieceColor;
                        GameButtons[rank, file].Content = fileName[pieceSize-minus];
                        file--;
                    }
                }
            }
        }
    }
}
