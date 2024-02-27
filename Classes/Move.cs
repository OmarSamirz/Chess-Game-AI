using Chess.Pages;
using Chess.Screens;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Media;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.ConstrainedExecution;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup.Localizer;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace Chess.Classes
{
    public static class Move
    {
        private static bool checkMate = false;
        private static bool staleMate = false;
        private static bool inCheck = false;
        private static int oldStartSquareX = -1;
        private static int oldStartSquareY = -1;
        private static int oldTargetSquaresX = -1;
        private static int oldTargetSquaresY = -1;
        private static int startSquareX = -1;
        private static int startSquareY = -1;
        private static int targetSquareX = -1;
        private static int targetSquareY = -1;
        private static (int, int) whiteKingLocation = (7, 4);
        private static (int, int) blackKingLocation = (0, 4);
        private static List<(int, int)> legalMoves = new List<(int, int)>();
        private static List<(int, int, int, int)> pins = new List<(int, int, int, int)>();
        private static List<(int, int, int, int)> checks = new List<(int, int, int, int)>();
        private static (List<(int, int)>, List<(int, int)>) validMoves = (new List<(int, int)>(), new List<(int, int)>());
        private static Grid chessGrid = new Grid();

        public static bool CheckMate { get { return checkMate; } set { checkMate = value; } }
        public static bool StaleMate { get { return staleMate; } set { StaleMate = value; } }
        public static bool InCheck { get { return inCheck; }  set { inCheck = value; } }
        public static int StartSquareX { get { return startSquareX; } set { startSquareX = value; } }
        public static int StartSquareY { get { return startSquareY; } set { startSquareY = value; } }
        public static int TargetSquareX { get { return targetSquareX; } set { targetSquareX = value; } }
        public static int TargetSquareY { get { return targetSquareY; } set { targetSquareY = value; } }
        public static (int, int) WhiteKingLocation { get { return whiteKingLocation; } set { whiteKingLocation = value; } }
        public static (int, int) BlackKingLocation { get { return blackKingLocation; } set { blackKingLocation = value; } }
        public static List<(int, int)> LegalMoves { get { return legalMoves; } set { legalMoves = value; } }
        public static List<(int, int, int, int)> Pins { get { return pins; } set { pins = value; } }
        public static List<(int, int, int, int)> Checks { get { return checks; } }
        public static (List<(int, int)>, List<(int, int)>) ValidMoves { get { return validMoves; } }
        public static Grid ChessGrid { get { return chessGrid; }  set { chessGrid = value; } }

        public static void ResetAll()
        {
            checkMate = false;
            staleMate = false;
            inCheck = false;
            startSquareX = -1;
            startSquareY = -1;
            targetSquareX = -1;
            targetSquareY = -1;
            whiteKingLocation = (7, 4);
            blackKingLocation = (0, 4);
            legalMoves.Clear();
            pins.Clear();
            checks.Clear();
            LegalMove.RookLocations.Clear();
            validMoves.Item1.Clear();
            validMoves.Item2.Clear();
        }


        public static bool isEmpty(int piece)
        {
            if (piece == Piece.None) 
                return true;

            return false;
        }

        public static bool IsInBoard(int x, int y)
        {
            if (x > 7 || y > 7)
                return false;

            else if (x < 0 || y < 0)
                return false;

            return true;
        }

        private static Ellipse CreateEllipse()
        {
            Ellipse ellipse = new Ellipse();
            ellipse.Height = 12;
            ellipse.Width = 12;
            Canvas.SetLeft(ellipse, 135);
            Canvas.SetTop(ellipse, 25);

            return ellipse;
        }

        private static Image CreateImage()
        {
            Image image = new Image();
            BitmapImage bi = new BitmapImage();
            string imagePath = (string)ChessBoard.GameButtons[TargetSquareX, TargetSquareY].Content;
            bi.BeginInit();
            bi.UriSource = new Uri(imagePath, UriKind.Relative);
            bi.EndInit();
            image.Source = bi;
            image.Height = 50;
            image.Margin = new Thickness(5, 0, 0, 0);

            return image;
        }

        private static void KingInCheckBackground(int AllyColor)
        {
            Button btn = new Button();
            if (AllyColor == Piece.White)
            {
                btn = ChessBoard.GameButtons[WhiteKingLocation.Item1, WhiteKingLocation.Item2];
                btn.Background = new SolidColorBrush(Color.FromRgb(209, 10, 37));
            }
            else
            {
                btn = ChessBoard.GameButtons[BlackKingLocation.Item1, BlackKingLocation.Item2];
                btn.Background = new SolidColorBrush(Color.FromRgb(209, 10, 37));
            }
        }

        public static void AIMovedBackgroud(Button btn1, Button btn2)
        {
            btn1.Background = new SolidColorBrush(Color.FromRgb(29, 138, 222));
            btn2.Background = new SolidColorBrush(Color.FromRgb(29, 138, 222));
        }

        private static void KingNotInCheckBackGround(int AllyColor)
        {
            GameBoard.remove_Background(
                                          (AllyColor == Piece.White)? ChessBoard.GameButtons[WhiteKingLocation.Item1, WhiteKingLocation.Item2] 
                                                                      : ChessBoard.GameButtons[BlackKingLocation.Item1, BlackKingLocation.Item2]
                                        );
        }

        private static void Ellipse_Click(object sender, MouseButtonEventArgs e)
        {
            Ellipse ellipse = (Ellipse)sender;
            int row = Grid.GetRow(ellipse);
            int col = Grid.GetColumn(ellipse);
            Button btn = ChessBoard.GameButtons[row, col];
            btn.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
        }

        private static void Ellipse_MouseEnter(object sender, MouseEventArgs e)
        {
            Ellipse ellipse = (Ellipse)sender;
            int row = Grid.GetRow(ellipse);
            int col = Grid.GetColumn(ellipse);
            Button btn = ChessBoard.GameButtons[row, col];
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(41, 182, 255));
            btn.BorderThickness = new Thickness(2.0);
            btn.MouseEnter += Button_MouseEnter;
            btn.MouseLeave += Button_MouseLeave;
        }

        private static void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            btn.BorderBrush = new SolidColorBrush(Color.FromRgb(41, 182, 255));
            btn.BorderThickness = new Thickness(2.0);
        }

        private static void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            Button btn = (Button)sender;
            btn.BorderBrush = Brushes.Transparent;
            btn.BorderThickness = new Thickness(0);
        }

        public static void AddBackgroundStep(Button btn, bool IsEmpty)
        {
            if (IsEmpty)
            {       
                int row = Grid.GetRow(btn);
                int col = Grid.GetColumn(btn);
                Ellipse ellipse = CreateEllipse();

                ellipse.Width = 8.0;
                ellipse.Height = 8.0;
                ellipse.Fill = new SolidColorBrush(Color.FromRgb(41, 182, 255));
                ellipse.MouseLeftButtonDown += Ellipse_Click;
                ellipse.MouseEnter += Ellipse_MouseEnter;
                ChessGrid.Children.Add(ellipse);
                Grid.SetRow(ellipse, row);
                Grid.SetColumn(ellipse, col);
            }


            else
            {
                int row = Grid.GetRow(btn);
                int col = Grid.GetColumn(btn);
                Ellipse ellipse = CreateEllipse();
                ellipse.Width = 8.0;
                ellipse.Height = 8.0;
                ellipse.Fill = new SolidColorBrush(Color.FromRgb(209, 10, 37));
                ellipse.MouseLeftButtonDown += Ellipse_Click;
                ellipse.MouseEnter += Ellipse_MouseEnter;
                ChessGrid.Children.Add(ellipse);
                Grid.SetRow(ellipse, row);
                Grid.SetColumn(ellipse, col);
            }
        }

        public static void RemoveBackgroundStep(bool flag)
        {
            if (!flag)
                return;

            foreach (var index in legalMoves)
            {
                ChessGrid.Children.Remove(ChessGrid.Children.Cast<UIElement>().Last(e => Grid.GetRow(e) == index.Item1 && Grid.GetColumn(e) == index.Item2));
            }
            legalMoves.Clear();
        }

        private static void SwapTurn(Player player1, Player player2, AIPlayer Computer, bool IsAI)
        {
            if (IsAI)
                return;

            bool temp = player1.Turn;
            player1.Turn = player2.Turn;
            player2.Turn = temp;
            Computer.Turn = temp;
        }

        public static (bool, List<(int, int, int, int)>) CheckForPinsAndChecks(int AllyColor, bool KingCheck)
        {
            int StartRow = 0, StartCol = 0, EnemyColor = 0;

            List<(int, int, int, int)> checking = new List<(int, int, int, int)>();
            bool isCheck = false;

            if ((AllyColor & Piece.White) != 0)
            {
                StartRow = whiteKingLocation.Item1;
                StartCol = whiteKingLocation.Item2;
                EnemyColor = Piece.Black;
            }
            else
            {
                StartRow = blackKingLocation.Item1;
                StartCol = blackKingLocation.Item2;
                EnemyColor = Piece.White;
            }

            (int, int)[] directions = { (-1, 0), (0, -1), (1, 0), (0, 1), (-1, -1), (-1, 1), (1, -1), (1, 1) };

            for (int j = 0; j < directions.Length; j++)
            {
                (int, int) d = directions[j];
                (int, int, int, int) PossiblePin = (-1, -1, -1, -1);
                for (int i = 1; i < 8; i++)
                {
                    int EndRow = StartRow + d.Item1 * i;
                    int EndCol = StartCol + d.Item2 * i;
                    if (IsInBoard(EndRow, EndCol))
                    {
                        int EndPiece = ChessBoard.Squares[EndRow, EndCol];
                        if ((EndPiece & AllyColor) != 0)
                        {
                            if (PossiblePin == (-1, -1, -1, -1))
                                PossiblePin = (EndRow, EndCol, d.Item1, d.Item2);
                            else
                                break;
                        }
                        else if ((EndPiece & EnemyColor) != 0)
                        {
                            int type = (EnemyColor > EndPiece) ? EnemyColor - EndPiece : EndPiece - EnemyColor;
                            type = ((type & Piece.Moved) != 0) ? type - Piece.Moved : type;
                            if ((j >= 0 && j <= 3 && type == Piece.Rook || type == (Piece.Rook | Piece.Moved))
                                 || (j >= 4 && j <= 7 && type == Piece.Bishop)
                                 || (i == 1 && (type == Piece.Pawn || type == (Piece.Moved | Piece.Pawn)) && ((EnemyColor == Piece.White && j >= 6 && j <= 7) || (EnemyColor == Piece.Black && j >= 4 && j <= 5)))
                                 || (type == Piece.Queen) || (i == 1 && (type == Piece.King || (type == (Piece.Moved | Piece.King)))))
                            {
                                if (PossiblePin == (-1, -1, -1, -1)) //No piece blocking so check
                                {
                                    if (!KingCheck) //We don't check for the king legal moves
                                    {
                                        inCheck = true;
                                        checks.Add((EndRow, EndCol, d.Item1, d.Item2));
                                        KingInCheckBackground(AllyColor);
                                    }
                                    else
                                    {
                                        isCheck = true;
                                        checking.Add((EndRow, EndCol, d.Item1, d.Item2));
                                    }
                                    continue; //Was break
                                }
                                else //Ally piece blocking so pin it
                                {
                                    if (!KingCheck) //We don't check for the king legal moves
                                        pins.Add(PossiblePin);
                                    break;
                                }
                            }
                            else //EnemyPiece not applying check
                                break;
                        }
                    }
                    else //Out the board
                        break;
                }
            }

            //Knight Part
            (int, int)[] KnightMoves = { (-2, -1), (-2, 1), (-1, -2), (-1, 2), (1, -2), (1, 2), (2, -1), (2, 1) };
            for (int m = 0; m < KnightMoves.Length; m++)
            {
                (int, int) M = KnightMoves[m];
                int EndRow = StartRow + M.Item1;
                int EndCol = StartCol + M.Item2;

                if (IsInBoard(EndRow, EndCol) && Piece.CheckColor(AllyColor | Piece.King, ChessBoard.Squares[EndRow, EndCol]))
                {
                    int EndPiece = ChessBoard.Squares[EndRow, EndCol];
                    int type = ((EndPiece & Piece.Moved) != 0) ? EndPiece - Piece.Moved : EndPiece;
                    type = (EnemyColor > type) ? EnemyColor - type : type - EnemyColor;
                    if ((EndPiece & EnemyColor) != 0 && (type == Piece.Knight))
                    {
                        if (!KingCheck)
                        {
                            inCheck = true;
                            checks.Add((EndRow, EndCol, M.Item1, M.Item2));
                        }
                        else
                        {
                            isCheck = true;
                            checking.Add((EndRow, EndCol, M.Item1, M.Item2));
                        }
                    }
                }
            }

            return (isCheck, checking);
        }

        private static (List<(int, int)>, List<(int, int)>) GetAllLegalMoves(int AllyColor)
        {
            int OldSQX = startSquareX;
            int OldSQY = startSquareY;
            var legal = legalMoves;
            List<(int, int)> Moves = new List<(int, int)>();
            List<(int, int)> KingMoves = new List<(int, int)>();
            List<(int, int)> HolderList = new List<(int, int)>();
            HolderList.AddRange(legalMoves);

            int rows = 8, cols = 8;

            for (int i = 0; i < rows * cols; i++)
            {
                int x = i / cols;
                int y = i % cols;

                int piece = ChessBoard.Squares[x, y];
                if ((piece & AllyColor) != 0)
                {
                    startSquareX = x;
                    startSquareY = y;
                    LegalMove.ChooselegalMoves(piece);
                    Moves.AddRange(legalMoves);
                    if ((piece == (Piece.Moved | AllyColor | Piece.King)) || (piece == (AllyColor | Piece.King)))
                        KingMoves.AddRange(legalMoves);
                    
                    RemoveBackgroundStep(true);
                }
            }

            if (HolderList.Count > 0)
            {
                legalMoves = HolderList;
                foreach (var Move in legalMoves)
                {
                    AddBackgroundStep(ChessBoard.GameButtons[Move.Item1, Move.Item2], isEmpty(ChessBoard.Squares[Move.Item1, Move.Item2]));
                }
            }

            startSquareX = OldSQX;
            startSquareY = OldSQY;

            return (Moves, KingMoves);
        }

        public static (List<(int, int)>, List<(int, int, int, int, double)>, bool) GetValidMoves(int AllyColor, bool IsAI)
        {
            int KingRow = 0, KingCol = 0;
            var legal = legalMoves;
            List<(int, int)> ValidSquares = new List<(int, int)>();
            List<(int, int, int, int, double)> ValidSquaresAI = new List<(int, int, int, int, double)>();
            var squares = ChessBoard.Squares;
            validMoves.Item1.Clear();
            validMoves.Item2.Clear();

            
            //May Remove
            var Checking = CheckForPinsAndChecks(AllyColor, true);

            if (AllyColor == Piece.White)
            {
                KingRow = whiteKingLocation.Item1;
                KingCol = whiteKingLocation.Item2;
            }
            else
            {
                KingRow = blackKingLocation.Item1;
                KingCol = blackKingLocation.Item2;
            }

            if (Checking.Item1)
            {
                if (Checking.Item2.Count == 1)
                {
                    (int, int, int, int) check;
                    int CheckRow, CheckCol;
                    if (checks.Count > 0)
                    {
                        check = checks.First();
                        CheckRow = check.Item1;
                        CheckCol = check.Item2;
                    }
                    else
                    {
                        check = Checking.Item2.First();
                        CheckRow = check.Item1;
                        CheckCol = check.Item2;
                    }
                    int PieceCheck = ChessBoard.Squares[CheckRow, CheckCol];
                    int EnemyColor = (AllyColor == Piece.White) ? Piece.Black : Piece.White;
                    PieceCheck = (EnemyColor > PieceCheck) ? EnemyColor - PieceCheck : PieceCheck - EnemyColor;

                    var moves = GetAllLegalMoves(AllyColor);
                    ValidSquares.AddRange(moves.Item2); //King's legal moves
                    validMoves.Item2.AddRange(moves.Item2); //King's legal moves

                    if (PieceCheck == Piece.Knight && (moves.Item1.Contains((CheckRow, CheckCol)) || moves.Item2.Contains((CheckRow, CheckCol))))
                    {
                        if (!IsAI)
                            KingInCheckBackground(AllyColor);
                        ValidSquares.Add((CheckRow, CheckCol));
                        validMoves.Item1.Add((CheckRow, CheckCol));
                        if (IsAI)
                        {
                            ValidSquaresAI.AddRange(AIPlayer.GetAllLegalMoves(AllyColor, ValidSquares));
                        }
                    }
                    else
                    {
                        for (int i = 1; i < 8; i++)
                        {
                            (int, int) ValidSquare = (KingRow + check.Item3 * i, KingCol + check.Item4 * i);
                            if (moves.Item1.Contains(ValidSquare))
                            {
                                ValidSquares.Add(ValidSquare);
                                validMoves.Item1.Add(ValidSquare);
                            }
                            if (ValidSquare.Item1 == CheckRow && ValidSquare.Item2 == CheckCol)
                                break;
                        }
                        if (IsAI)
                        {
                            ValidSquaresAI.AddRange(AIPlayer.GetAllLegalMoves(AllyColor, ValidSquares));
                        }
                    }
                }
                else
                {
                    LegalMove.ChooselegalMoves(AllyColor | Piece.King);
                    ValidSquares.AddRange(LegalMoves);
                    validMoves.Item1 = ValidSquares;
                    RemoveBackgroundStep(true);
                    if (IsAI)
                    {
                        ValidSquaresAI.AddRange(AIPlayer.GetAllLegalMoves(AllyColor, ValidSquares));
                    }
                }
            }
            else
            {
                ValidSquares.AddRange(GetAllLegalMoves(AllyColor).Item1);
                if (IsAI)
                    ValidSquaresAI.AddRange(AIPlayer.GetAllLegalMoves(AllyColor, ValidSquares));
            }


            return (ValidSquares, ValidSquaresAI, Checking.Item1);
        }

        private static bool IsCastling(int x, int y)
        {
            foreach (var RookLocation in LegalMove.RookLocations)
            {
                if (RookLocation.Item3 == x && RookLocation.Item4 == y)
                    return true;
            }

            return false;
        }

        private static void Castling(bool IsAI)
        {
            (int, int, int, int) CurrentRookLocation = LegalMove.RookLocations.First();

            //Moving the king
            if ((ChessBoard.Squares[startSquareX, startSquareY] & Piece.White) != 0)
                whiteKingLocation = (targetSquareX, targetSquareY);
            else
                blackKingLocation = (targetSquareX, targetSquareY);

            ChessBoard.Squares[targetSquareX, targetSquareY] = (Piece.Moved | ChessBoard.Squares[startSquareX, startSquareY]);
            ChessBoard.Squares[startSquareX, startSquareY] = Piece.None;

            if (!IsAI)
            {
                ChessBoard.GameButtons[targetSquareX, targetSquareY].Content = ChessBoard.GameButtons[startSquareX, startSquareY].Content;
                ChessBoard.GameButtons[startSquareX, startSquareY].Content = "";
                GameBoard.remove_Background(ChessBoard.GameButtons[startSquareX, startSquareY]);
            }


            //Moving the rook
            if (CurrentRookLocation.Item2 != -1)
            {
                if (CurrentRookLocation.Item2 > 4)
                {
                    ChessBoard.Squares[targetSquareX, targetSquareY - 1] = (Piece.Moved | ChessBoard.Squares[CurrentRookLocation.Item1, CurrentRookLocation.Item2]);
                    ChessBoard.Squares[CurrentRookLocation.Item1, CurrentRookLocation.Item2] = Piece.None;
                    if (!IsAI)
                    {
                        ChessBoard.GameButtons[targetSquareX, targetSquareY - 1].Content = ChessBoard.GameButtons[CurrentRookLocation.Item1, CurrentRookLocation.Item2].Content;
                        ChessBoard.GameButtons[CurrentRookLocation.Item1, CurrentRookLocation.Item2].Content = "";
                    }
                }
                else
                {
                    ChessBoard.Squares[targetSquareX, targetSquareY + 1] = (Piece.Moved | ChessBoard.Squares[CurrentRookLocation.Item1, CurrentRookLocation.Item2]);
                    ChessBoard.Squares[CurrentRookLocation.Item1, CurrentRookLocation.Item2] = Piece.None;
                    if (!IsAI)
                    {
                        ChessBoard.GameButtons[targetSquareX, targetSquareY + 1].Content = ChessBoard.GameButtons[CurrentRookLocation.Item1, CurrentRookLocation.Item2].Content;
                        ChessBoard.GameButtons[CurrentRookLocation.Item1, CurrentRookLocation.Item2].Content = "";
                    }
                }
            }

            checks.Clear();
            pins.Clear();
            inCheck = false;
            startSquareX = -1;
            startSquareY = -1;
            targetSquareX = -1;
            targetSquareY = -1;
        }

        public static bool MovePiece(Player player1, Player player2, AIPlayer Computer, bool IsAI)
        {
            int piece = ChessBoard.Squares[startSquareX, startSquareY];
            int PieceColor = ((piece & Piece.White) != 0) ? Piece.White : Piece.Black;
            piece = ((piece & Piece.White) != 0) ? piece - Piece.White : piece - Piece.Black;

            if (inCheck && checks.Count == 2 && piece != Piece.King)
                return false;

            //if there is piece in the target square then put it's image inside the WrapPanel
            if (ChessBoard.Squares[TargetSquareX, TargetSquareY] != Piece.None && !IsAI)
            {
                if ((ChessBoard.Squares[TargetSquareX, TargetSquareY] & Piece.White) != 0)
                {
                    GameBoard.playerTwoWrapPanel.Children.Add(CreateImage());
                }
                else
                {
                    GameBoard.playerOneWrapPanel.Children.Add(CreateImage());
                }
            }

            //Castling
            if (piece == Piece.King && IsCastling(targetSquareX, targetSquareY))
            {
                if (player1.Turn)
                {
                    oldStartSquareX = startSquareX;
                    oldStartSquareY = startSquareY;
                    oldTargetSquaresX = targetSquareX;
                    oldTargetSquaresY = targetSquareY;
                }

                Castling(IsAI);

                SwapTurn(player1, player2, Computer, IsAI);
                return true;
            }

            //Check if the king is moved
            if ((piece - Piece.King) == 0 || (piece - Piece.Moved - Piece.King) == 0)
            {
                //Update the location of the white king
                if (((piece | PieceColor) & Piece.White) != 0)
                {
                    whiteKingLocation.Item1 = targetSquareX;
                    whiteKingLocation.Item2 = targetSquareY;
                    if ((ChessBoard.Squares[startSquareX, startSquareY] & Piece.Moved) == 0)
                        ChessBoard.Squares[startSquareX, startSquareY] += Piece.Moved;
                }
                //Update the location of the black king
                else
                {
                    blackKingLocation.Item1 = targetSquareX;
                    blackKingLocation.Item2 = targetSquareY;
                    if ((ChessBoard.Squares[startSquareX, startSquareY] & Piece.Moved) == 0)
                        ChessBoard.Squares[startSquareX, startSquareY] += Piece.Moved;
                }
            }

            //Mark the pawn or the rook piece as moved
            if ((piece - Piece.Pawn) == 0 || (piece - Piece.Rook) == 0)
                ChessBoard.Squares[startSquareX, startSquareY] += Piece.Moved;



            var squares = ChessBoard.Squares;
            //Pawn's Promotion to another piece
            if (((piece - Piece.Pawn) == 0 || (piece - Piece.Moved - Piece.Pawn) == 0) && (TargetSquareX == 0 || TargetSquareX == 7))
            {

                if (player1.Turn)
                {
                    oldStartSquareX = startSquareX;
                    oldStartSquareY = startSquareY;
                    oldTargetSquaresX = targetSquareX;
                    oldTargetSquaresY = targetSquareY;
                }

                if (!IsAI)
                {
                    PawnPromotionWindow pawnPromotionWindow = new PawnPromotionWindow(PieceColor, player1, player2, Move.StartSquareX, Move.StartSquareY, Move.TargetSquareX, Move.TargetSquareY);
                    pawnPromotionWindow.ShowDialog();

                    ChessBoard.Squares[startSquareX, startSquareY] = Piece.None;
                    ChessBoard.GameButtons[startSquareX, startSquareY].Content = "";
                    GameBoard.remove_Background(ChessBoard.GameButtons[startSquareX, startSquareY]);
                    
                }
                else
                {
                    ChessBoard.Squares[startSquareX, startSquareY] = Piece.None;
                    ChessBoard.Squares[targetSquareX, targetSquareY] = PieceColor | Piece.Queen;
                }

                checks.Clear();
                pins.Clear();
                SwapTurn(player1, player2, Computer, IsAI);
                inCheck = false;
                startSquareX = -1;
                startSquareY = -1;
                targetSquareX = -1;
                targetSquareY = -1;
                return true;
            }

            ChessBoard.Squares[targetSquareX, targetSquareY] = ChessBoard.Squares[startSquareX, startSquareY];
            ChessBoard.Squares[startSquareX, startSquareY] = Piece.None;

            if (!IsAI)
            {
                ChessBoard.GameButtons[targetSquareX, targetSquareY].Content = ChessBoard.GameButtons[startSquareX, startSquareY].Content;
                ChessBoard.GameButtons[startSquareX, startSquareY].Content = "";
                GameBoard.remove_Background(ChessBoard.GameButtons[startSquareX, startSquareY]);
            }

            
            checks.Clear();
            pins.Clear();
            if (inCheck)
                KingNotInCheckBackGround(PieceColor);
            SwapTurn(player1, player2, Computer, IsAI);
            inCheck = false;

            if (player1.Turn)
            {
                oldStartSquareX = startSquareX;
                oldStartSquareY = startSquareY;
                oldTargetSquaresX = targetSquareX;
                oldTargetSquaresY = targetSquareY;
            }

            startSquareX = -1;
            startSquareY = -1;
            targetSquareX = -1; 
            targetSquareY = -1;
            return true;
        }

        public static void CreateMove(Button btn, int color, Player player1, Player player2, AIPlayer Computer, bool IsAI)
        {
            int X = -1, Y = -1, StartPiece = -1;
            if (btn != null)
            {
                X = Grid.GetRow(btn);
                Y = Grid.GetColumn(btn);
                StartPiece = ChessBoard.Squares[X, Y];
            }

            if (startSquareX == X && startSquareY == Y)
            {
                RemoveBackgroundStep(true);
                GameBoard.remove_Background(ChessBoard.GameButtons[startSquareX, startSquareY]);
                startSquareX = -1;
                startSquareY = -1;
            }

            else if ((StartPiece & color) == color && (startSquareX == -1 || startSquareY == -1))
            {
                if (StartPiece == Piece.None)
                    return;

                else if ((startSquareX == -1 || startSquareY == -1) && StartPiece != Piece.None)
                {
                    startSquareX = X;
                    startSquareY = Y;
                    if (btn != null)
                        GameBoard.add_Background(btn);
                    LegalMove.ChooselegalMoves(StartPiece);
                }
            }

            else if (startSquareX != -1 && startSquareY != -1)
            {
                if (!IsAI)
                {
                    targetSquareX = X;
                    targetSquareY = Y;
                }
                if (Piece.CheckColor(ChessBoard.Squares[startSquareX, startSquareY], ChessBoard.Squares[targetSquareX, targetSquareY]))
                {
                    if (!legalMoves.Contains((targetSquareX, targetSquareY)) && !IsAI)
                        return;

                    var btn1 = ChessBoard.GameButtons[Move.StartSquareX, Move.StartSquareY];
                    var btn2 = ChessBoard.GameButtons[Move.TargetSquareX, Move.TargetSquareY];

                    bool IsMoved = MovePiece(player1, player2, Computer, false);
                    if (IsAI)
                    {
                        btn1.Background = new SolidColorBrush(Color.FromRgb(98, 176, 245));
                        btn2.Background = new SolidColorBrush(Color.FromRgb(29, 138, 222));
                    }
                    else
                    {
                        if (oldStartSquareX != -1)
                        {
                            GameBoard.remove_Background(ChessBoard.GameButtons[oldStartSquareX, oldStartSquareY]);
                            GameBoard.remove_Background(ChessBoard.GameButtons[oldTargetSquaresX, oldTargetSquaresY]);
                        }
                    }


                    if (IsMoved)
                    {
                        CheckForPinsAndChecks((player1.Turn == true) ? player1.Color : player2.Color, false);
                        Options.MovePiecePlay(IsAI);
                    }

                    RemoveBackgroundStep(IsMoved);
                    var ValidMoves = GetValidMoves((player1.Turn == true) ? player1.Color : player2.Color, false).Item1;

                    if (ValidMoves.Count == 0)
                    {
                        if (InCheck)
                            checkMate = true;
                        else
                            staleMate = true;
                    }
                }
                else
                {
                    GameBoard.remove_Background(ChessBoard.GameButtons[startSquareX, startSquareY]);
                    startSquareX = X;
                    startSquareY = Y;
                    if (btn != null)
                        GameBoard.add_Background(btn);
                    RemoveBackgroundStep(true);
                    LegalMove.ChooselegalMoves(ChessBoard.Squares[X, Y]);
                    targetSquareX = -1;
                    targetSquareY = -1;
                }
            }
        }
    }
}