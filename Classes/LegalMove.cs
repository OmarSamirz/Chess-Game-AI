using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Classes
{
    public static class LegalMove
    {

        public static List<(int, int, int, int)> RookLocations = new List<(int, int, int, int)>();
        
        //Choose legal move by piece
        public static void ChooselegalMoves(int piece)
        {
            int AllyColor = ((piece & Piece.White) != 0) ? Piece.White : Piece.Black;
            piece = ((piece & Piece.White) != 0) ? piece - Piece.White : piece - Piece.Black;

            switch (piece)
            {
                case Piece.King:
                case (Piece.King | Piece.Moved):
                    {
                        KinglegalMoves(AllyColor);
                        break;
                    }

                case Piece.Queen:
                    QueenlegalMoves();
                    break;

                case Piece.Bishop:
                    BishoplegalMoves();
                    break;

                case Piece.Knight:
                    KnightlegalMoves();
                    break;

                case Piece.Rook:
                case (Piece.Rook | Piece.Moved):
                    {
                        RooklegalMoves();
                        break;
                    }

                case Piece.Pawn:
                case (Piece.Pawn | Piece.Moved):
                    {
                        PawnlegalMoves();
                        break;
                    }

                default:
                    break;
            }
        }

        //Linear movment
        private static void LinearMove(int x, int y)
        {
            int sqX = Move.StartSquareX;
            int sqY = Move.StartSquareY;
            int mul = ((ChessBoard.Squares[sqX, sqY] & Piece.White) != 0) ? -1 : 1;

            sqX += (mul * x);
            sqY += (mul * y);
            while ((Move.IsInBoard(sqX, sqY) && ChessBoard.Squares[sqX, sqY] == Piece.None) || (Move.IsInBoard(sqX, sqY) && ChessBoard.Squares[sqX, sqY] != Piece.None))
            {
                if (!Move.IsInBoard(sqX, sqY) || !Piece.CheckColor(ChessBoard.Squares[Move.StartSquareX, Move.StartSquareY], ChessBoard.Squares[sqX, sqY]))
                    break;

                if (Move.IsInBoard(sqX, sqY) && !Move.isEmpty(ChessBoard.Squares[sqX, sqY]) && (!Move.InCheck || (!Move.InCheck && Move.Checks.Count != 2) || (Move.InCheck && Move.ValidMoves.Item1.Count == 0 && Move.ValidMoves.Item2.Count == 0) || Move.ValidMoves.Item1.Contains((sqX, sqY)))
                    && Piece.CheckColor(ChessBoard.Squares[Move.StartSquareX, Move.StartSquareY], ChessBoard.Squares[sqX, sqY]))
                {
                    Move.LegalMoves.Add((sqX, sqY));
                    Move.AddBackgroundStep(ChessBoard.GameButtons[sqX, sqY], Move.isEmpty(ChessBoard.Squares[sqX, sqY]));
                    break;
                }

                if (!Move.InCheck || (!Move.InCheck && Move.Checks.Count != 2) || (Move.InCheck && Move.ValidMoves.Item1.Count == 0 && Move.ValidMoves.Item2.Count == 0) || Move.ValidMoves.Item1.Contains((sqX, sqY)))
                {
                    Move.LegalMoves.Add((sqX, sqY));
                    Move.AddBackgroundStep(ChessBoard.GameButtons[sqX, sqY], Move.isEmpty(ChessBoard.Squares[sqX, sqY]));
                }
                sqX += (mul * x);
                sqY += (mul * y);
            }
        }

        private static void KingCastling(int sqX, int sqY, int Dir, int AllyColor)
        {
            (int, int) KingOldLocation = ((AllyColor & Piece.White) != 0) ? Move.WhiteKingLocation : Move.BlackKingLocation;
            int OldsqY = sqY;


            sqY += (Dir * 2);
            if (Move.IsInBoard(sqX, sqY) && ChessBoard.Squares[sqX, sqY] == Piece.None)
            {
                if ((AllyColor & Piece.White) != 0)
                {
                    ChessBoard.Squares[Move.WhiteKingLocation.Item1, Move.WhiteKingLocation.Item2] = Piece.None;
                    Move.WhiteKingLocation = (sqX, sqY);
                }
                else
                {
                    ChessBoard.Squares[Move.BlackKingLocation.Item1, Move.BlackKingLocation.Item2] = Piece.None;
                    Move.BlackKingLocation = (sqX, sqY);
                }

                var Checking = Move.CheckForPinsAndChecks(AllyColor, true);
                if ((AllyColor & Piece.White) != 0)
                {
                    Move.WhiteKingLocation = KingOldLocation;
                    ChessBoard.Squares[KingOldLocation.Item1, KingOldLocation.Item2] = (Piece.King | Piece.White);
                }
                else
                {
                    Move.BlackKingLocation = KingOldLocation;
                    ChessBoard.Squares[KingOldLocation.Item1, KingOldLocation.Item2] = (Piece.King | Piece.Black);
                }
                if (!Checking.Item1)
                {
                    sqY += (Dir * 1);
                    while ((Move.IsInBoard(sqX, sqY) && ChessBoard.Squares[sqX, sqY] == Piece.None) || (Move.IsInBoard(sqX, sqY) && ChessBoard.Squares[sqX, sqY] == (AllyColor | Piece.Rook)))
                    {
                        if (ChessBoard.Squares[sqX, sqY] == (AllyColor | Piece.Rook))
                        {
                            if ((AllyColor & Piece.White) != 0)
                            {
                                ChessBoard.Squares[Move.WhiteKingLocation.Item1, Move.WhiteKingLocation.Item2] = Piece.None;
                                Move.WhiteKingLocation = (sqX, sqY);
                            }
                            else
                            {
                                ChessBoard.Squares[Move.BlackKingLocation.Item1, Move.BlackKingLocation.Item2] = Piece.None;
                                Move.BlackKingLocation = (sqX, sqY);
                            }
                            Checking = Move.CheckForPinsAndChecks(AllyColor, true);
                            
                            if (!Checking.Item1)
                            {
                                RookLocations.Add((sqX, sqY, sqX, OldsqY + (Dir * 2)));
                                Move.LegalMoves.Add((sqX, OldsqY + (Dir * 2)));
                                Move.AddBackgroundStep(ChessBoard.GameButtons[sqX, OldsqY + (Dir * 2)], Move.isEmpty(ChessBoard.Squares[sqX , OldsqY + (+(Dir * 2))]));
                            }

                            if ((AllyColor & Piece.White) != 0)
                            {
                                Move.WhiteKingLocation = KingOldLocation;
                                ChessBoard.Squares[KingOldLocation.Item1, KingOldLocation.Item2] = (Piece.King | Piece.White);
                            }
                            else
                            {
                                Move.BlackKingLocation = KingOldLocation;
                                ChessBoard.Squares[KingOldLocation.Item1, KingOldLocation.Item2] = (Piece.King | Piece.Black);
                            }
                        }
                        sqY += (Dir * 1);
                    }

                }
            }
        }

        //King's legal moves
        private static void KinglegalMoves(int AllyColor)
        {
            int[] x_axis = { 1, 1, 0, -1, -1, -1, 0, 1 };
            int[] y_axis = { 0, 1, 1, 1, 0, -1, -1, -1 };
            int sqX = ((AllyColor & Piece.White) != 0) ? Move.WhiteKingLocation.Item1 : Move.BlackKingLocation.Item1;
            int sqY = ((AllyColor & Piece.White) != 0) ? Move.WhiteKingLocation.Item2 : Move.BlackKingLocation.Item2;
            int mul = ((ChessBoard.Squares[sqX, sqY] & Piece.White) != 0) ? -1 : 1;
            (int, int) KingOldLocation = (mul == -1) ? Move.WhiteKingLocation : Move.BlackKingLocation;
            bool Moved = ((ChessBoard.Squares[sqX, sqY] & Piece.Moved) != 0) ? true : false;

            for (int x = 0, y = 0; x < 8; x++, y++)
            {
                if (Move.IsInBoard(sqX + (mul * x_axis[x]), sqY + (mul * y_axis[y])) && Piece.CheckColor(ChessBoard.Squares[sqX, sqY], ChessBoard.Squares[sqX + (mul * x_axis[x]), sqY + (mul * y_axis[y])]))
                {
                    if (mul == -1)
                    {
                        ChessBoard.Squares[Move.WhiteKingLocation.Item1, Move.WhiteKingLocation.Item2] = Piece.None;
                        Move.WhiteKingLocation = (sqX + (mul * x_axis[x]), sqY + (mul * y_axis[y]));
                    }
                    if (mul == 1)
                    {
                        ChessBoard.Squares[Move.BlackKingLocation.Item1, Move.BlackKingLocation.Item2] = Piece.None;
                        Move.BlackKingLocation = (sqX + (mul * x_axis[x]), sqY + (mul * y_axis[y]));
                    }

                    var Checking = Move.CheckForPinsAndChecks((mul == -1) ? Piece.White : Piece.Black, true);
                    if (!Checking.Item1)
                    {
                        Move.LegalMoves.Add((sqX + (mul * x_axis[x]), sqY + (mul * y_axis[y])));
                        Move.AddBackgroundStep(ChessBoard.GameButtons[sqX + (mul * x_axis[x]), sqY + (mul * y_axis[y])], Move.isEmpty(ChessBoard.Squares[sqX + (mul * x_axis[x]), sqY + (mul * y_axis[y])]));
                    }

                    Checking.Item1 = false;
                    if (mul == -1)
                    {
                        Move.WhiteKingLocation = KingOldLocation;
                        ChessBoard.Squares[KingOldLocation.Item1, KingOldLocation.Item2] = (Moved) ? (Piece.Moved | Piece.White | Piece.King) : (Piece.King | Piece.White);
                    }
                    if (mul == 1)
                    {
                        Move.BlackKingLocation = KingOldLocation;
                        ChessBoard.Squares[KingOldLocation.Item1, KingOldLocation.Item2] = (Moved) ? (Piece.Moved | Piece.Black | Piece.King) : (Piece.King | Piece.Black);
                    }
                }
            }

            if (!Move.InCheck && !Moved)
            {
                //Right castling
                if (Move.LegalMoves.Contains((sqX + (mul * x_axis[2]), sqY + (mul * y_axis[2]))))
                    KingCastling(sqX, sqY, mul * 1, AllyColor);
                //Left castling
                if (Move.LegalMoves.Contains((sqX + (mul * x_axis[6]), sqY + (mul * y_axis[6]))))
                    KingCastling(sqX, sqY, mul * -1, AllyColor);
            }
        }

        //Queen's legal moves
        private static void QueenlegalMoves()
        {
            BishoplegalMoves();
            RooklegalMoves();
        }

        //Bishop's legal moves
        private static void BishoplegalMoves()
        {
            int mul = ((ChessBoard.Squares[Move.StartSquareX, Move.StartSquareY] & Piece.White) != 0) ? -1 : 1;
            bool PiecePinned = false;
            (int, int) PinnedDistination = (-1, -1);
            foreach (var pin in Move.Pins)
            {
                if (pin.Item1 == Move.StartSquareX && pin.Item2 == Move.StartSquareY)
                {
                    PiecePinned = true;
                    PinnedDistination = (pin.Item3, pin.Item4);
                    break;
                }
            }

            if (!PiecePinned || PinnedDistination == (1 * mul, 1 * mul))
                LinearMove(1, 1);
            if (!PiecePinned || PinnedDistination == (1 * mul, -1 * mul))
                LinearMove(1, -1);
            if (!PiecePinned || PinnedDistination == (-1 * mul, -1 * mul))
                LinearMove(-1, -1);
            if (!PiecePinned || PinnedDistination == (-1 * mul, 1 * mul))
                LinearMove(-1, 1);
        }


        //Knight's legal moves
        private static void KnightlegalMoves()
        {
            int[] x_axis = { -2, -1, 1, 2, -2, -1, 1, 2 };
            int[] y_axis = { -1, -2, -2, -1, 1, 2, 2, 1 };
            int sqX = Move.StartSquareX;
            int sqY = Move.StartSquareY;
            int mul = ((ChessBoard.Squares[sqX, sqY] & Piece.White) != 0) ? -1 : 1;

            bool PiecePinned = false;
            foreach (var pin in Move.Pins)
            {
                if (pin.Item1 == Move.StartSquareX && pin.Item2 == Move.StartSquareY)
                {
                    PiecePinned = true;
                    break;
                }
            }

            for (int x = 0, y = 0; x < 8; x++, y++)
            {
                if (Move.IsInBoard(sqX + (mul * x_axis[x]), sqY + (mul * y_axis[y])) && !PiecePinned 
                    && Piece.CheckColor(ChessBoard.Squares[sqX, sqY], ChessBoard.Squares[sqX + (mul * x_axis[x]), sqY + (mul * y_axis[y])])
                    && (!Move.InCheck || (!Move.InCheck && Move.Checks.Count != 2) || (Move.InCheck && Move.ValidMoves.Item1.Count == 0 && Move.ValidMoves.Item2.Count == 0) || Move.ValidMoves.Item1.Contains((sqX + (mul * x_axis[x]), sqY + (mul * y_axis[y])))))
                {
                    Move.LegalMoves.Add((sqX + (mul * x_axis[x]), sqY + (mul * y_axis[y])));
                    Move.AddBackgroundStep(ChessBoard.GameButtons[sqX + (mul * x_axis[x]), sqY + (mul * y_axis[y])], Move.isEmpty(ChessBoard.Squares[sqX + (mul * x_axis[x]), sqY + (mul * y_axis[y])]));
                }
            }
        }


        //Rook's legal moves
        private static void RooklegalMoves()
        {
            int mul = ((ChessBoard.Squares[Move.StartSquareX, Move.StartSquareY] & Piece.White) != 0) ? -1 : 1;
            bool PiecePinned = false;
            (int, int) PinnedDistination = (-1, -1);
            foreach (var pin in Move.Pins)
            {
                if (pin.Item1 == Move.StartSquareX && pin.Item2 == Move.StartSquareY)
                {
                    PiecePinned = true;
                    PinnedDistination = (pin.Item3, pin.Item4);
                    break;
                }
            }

            if (!PiecePinned || PinnedDistination == (1 * mul, 0))
                LinearMove(1, 0);

            if (!PiecePinned || PinnedDistination == (-1 * mul, 0))
                LinearMove(-1, 0);

            if (!PiecePinned || PinnedDistination == (0, 1 * mul))
                LinearMove(0, 1);

            if (!PiecePinned || PinnedDistination == (0, -1 * mul))
                LinearMove(0, -1);
        }

        //Pawn's legal moves
        private static void PawnlegalMoves()
        {
            int sqX = Move.StartSquareX;
            int sqY = Move.StartSquareY;
            int mul = ((ChessBoard.Squares[sqX, sqY] & Piece.White) != 0) ? -1 : 1;


            bool PiecePinned = false;
            (int, int) PinnedDistination = (-1, -1);
            foreach (var pin in Move.Pins)
            {
                if (pin.Item1 == Move.StartSquareX && pin.Item2 == Move.StartSquareY)
                {
                    PiecePinned = true;
                    PinnedDistination = (pin.Item3, pin.Item4);
                    //pins.Remove(pin);
                    break;
                }
            }

            //One Step Forward
            if (Move.IsInBoard(sqX + (mul * 1), sqY) && Move.isEmpty(ChessBoard.Squares[sqX + (mul * 1), sqY]) 
                && (!PiecePinned || PinnedDistination == (1 * mul, 0)) 
                && (!Move.InCheck || (!Move.InCheck && Move.Checks.Count != 2) || (Move.InCheck && Move.ValidMoves.Item1.Count == 0 && Move.ValidMoves.Item2.Count == 0) || Move.ValidMoves.Item1.Contains((sqX + (mul * 1), sqY)))
                )
            {
                Move.LegalMoves.Add((sqX + (mul * 1), sqY));
                Move.AddBackgroundStep(ChessBoard.GameButtons[sqX + (mul * 1), sqY], Move.isEmpty(ChessBoard.Squares[sqX + (mul * 1), sqY]));
            }

            //Two Step Forward
            if (((ChessBoard.Squares[sqX, sqY] & Piece.Moved) == 0) && !PiecePinned
                && Move.isEmpty(ChessBoard.Squares[sqX + (mul * 2), sqY])
                && Move.isEmpty(ChessBoard.Squares[sqX + (mul * 1), sqY])
                && (!Move.InCheck || (!Move.InCheck && Move.Checks.Count != 2) || (Move.InCheck && Move.ValidMoves.Item1.Count == 0 && Move.ValidMoves.Item2.Count == 0) || Move.ValidMoves.Item1.Contains((sqX + (mul * 2), sqY)))
                )
            {
                Move.LegalMoves.Add((sqX + (mul * 2), sqY));
                Move.AddBackgroundStep(ChessBoard.GameButtons[sqX + (mul * 2), sqY], Move.isEmpty(ChessBoard.Squares[sqX + (mul * 2), sqY]));
            }

            //One step right diagonal
            if (Move.IsInBoard(sqX + (mul * 1), sqY - (mul * 1)) && !Move.isEmpty(ChessBoard.Squares[sqX + (mul * 1), sqY - (mul * 1)]) && (!PiecePinned || PinnedDistination == (1 * mul, -1 * mul))
                && Piece.CheckColor(ChessBoard.Squares[sqX, sqY], ChessBoard.Squares[sqX + (mul * 1), sqY - (mul * 1)])
                && (!Move.InCheck || (!Move.InCheck && Move.Checks.Count != 2) || (Move.InCheck && Move.ValidMoves.Item1.Count == 0 && Move.ValidMoves.Item2.Count == 0) || Move.ValidMoves.Item1.Contains((sqX + (mul * 1), sqY - (mul * 1))))
                )
            {
                Move.LegalMoves.Add((sqX + (mul * 1), sqY - (mul * 1)));
                Move.AddBackgroundStep(ChessBoard.GameButtons[sqX + (mul * 1), sqY - (mul * 1)], Move.isEmpty(ChessBoard.Squares[sqX + (mul * 1), sqY - (mul * 1)]));
            }

            //One step left diagonal
            if (Move.IsInBoard(sqX + (mul * 1), sqY + (mul * 1)) && !Move.isEmpty(ChessBoard.Squares[sqX + (mul * 1), sqY + (mul * 1)]) && (!PiecePinned || PinnedDistination == (1 * mul, 1 * mul))
                && Piece.CheckColor(ChessBoard.Squares[sqX, sqY], ChessBoard.Squares[sqX + (mul * 1), sqY + (mul * 1)])
                && (!Move.InCheck || (!Move.InCheck && Move.Checks.Count != 2) || (Move.InCheck && Move.ValidMoves.Item1.Count == 0 && Move.ValidMoves.Item2.Count == 0) || Move.ValidMoves.Item1.Contains((sqX + (mul * 1), sqY + (mul * 1))))
                )
            {
                Move.LegalMoves.Add((sqX + (mul * 1), sqY + (mul * 1)));
                Move.AddBackgroundStep(ChessBoard.GameButtons[sqX + (mul * 1), sqY + (mul * 1)], Move.isEmpty(ChessBoard.Squares[sqX + (mul * 1), sqY + (mul * 1)]));
            }
        }
    }
}