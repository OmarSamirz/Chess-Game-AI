using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace Chess.Classes
{
    public class AIPlayer : Player
    {
        private readonly Player player1;
        private readonly Player player2;
        private static int openingCounter = 0;
        private int RandomOpening = 0;
        private (int, int, int, int, double) BestMove = (-1, -1, -1, -1, float.MaxValue);

        private static int depth = 2;
        public static int OpeningCounter { get { return openingCounter; } }
        public static int DEPTH { get { return depth; } set { depth = value; } }

        public AIPlayer(string name, bool turn, int color, Player player1, Player player2) : base(name, turn, color)
        {
            this.Name = name;
            this.Turn = turn;
            this.Color = color;
            this.player1 = player1;
            this.player2 = player2;

            Random rnd = new Random();
            this.RandomOpening = rnd.Next(0, 2);
        }

        private (int, int, int, int)[,] openings =
        {
            { (1, 3, 3, 3), (1, 2, 3, 2), (0, 2, 1, 3) },    //Catalan Opening
            { (1, 3, 3, 3), (1, 2, 3, 2), (0, 1, 2, 2) },    //Scandinavian Defense
            { (1, 3, 3, 3), (0, 2, 5, 3), (1, 4, 2, 4) },    //London System
        };

        private static double[,] KingMap =
{
            { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
            { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
            { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
            { -3.0, -4.0, -4.0, -5.0, -5.0, -4.0, -4.0, -3.0 },
            { -2.0, -3.0, -3.0, -4.0, -4.0, -3.0, -3.0, -2.0 },
            { -1.0, -2.0, -2.0, -2.0, -2.0, -2.0, -2.0, -1.0 },
            { 2.0, 2.0, 0.0, 0.0, 0.0, 0.0, 2.0, 2.0 },
            { 2.0, 3.0, 1.0, 0.0, 0.0, 1.0, 3.0, 2.0 }
        };

        private static double[,] QueenMap =
        {
            { -2.0, -1.0, -1.0, -0.5, -0.5, -1.0, -1.0, -2.0 },
            { -1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -1.0 },
            { -1.0, 0.0, 0.5, 0.5, 0.5, 0.5, 0.0, -1.0 },
            { -0.5, 0.0, 0.5, 0.5, 0.5, 0.5, 0.0, -0.5 },
            { 0.0, 0.0, 0.5, 0.5, 0.5, 0.5, 0.0, -0.5 },
            { -1.0, 0.0, 0.5, 0.5, 0.5, 0.5, 0.0, -1.0 },
            { -1.0, 0.0, 0.5, 0.5, 0.5, 0.5, 0.0, -1.0 },
            { -2.0, -1.0, -1.0, -0.5, -0.5, -1.0, -1.0, -2.0 }
        };

        private static double[,] BishopMap =
        {
            { -2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0 },
            { -1.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -1.0 },
            { -1.0, 0.0, 0.5, 1.0, 1.0, 0.5, 0.0, -1.0 },
            { -1.0, 0.5, 0.5, 1.0, 1.0, 0.5, 0.5, -1.0 },
            { -1.0, 0.0, 1.0, 1.0, 1.0, 1.0, 0.0, -1.0 },
            { -1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, -1.0 },
            { -1.0, 0.5, 0.0, 0.0, 0.0, 0.0, 0.5, -1.0 },
            { -2.0, -1.0, -1.0, -1.0, -1.0, -1.0, -1.0, -2.0 }
        };

        private static double[,] KnightMap =
        {
            { -5.0, -4.0, -3.0, -3.0, -3.0, -3.0, -4.0, -5.0 },
            { -4.0, -2.0, 0.0, 0.0, 0.0, 0.0, -2.0, -4.0 },
            { -3.0, 0.0, 1.0, 1.5, 1.5, 1.0, 0.0, -3.0 },
            { -3.0, 0.5, 1.5, 2.0, 2.0, 1.5, 0.5, -3.0 },
            { -3.0, 0.0, 1.5, 2.0, 2.0, 1.5, 0.0, -3.0 },
            { -3.0, 0.5, 1.0, 1.5, 1.5, 1.0, 0.5, -3.0 },
            { -4.0, -2.0, 0.0, 0.5, 0.5, 0.0, -2.0, -4.0 },
            { -5.0, -4.0, -3.0, -3.0, -3.0, -3.0, -4.0, -5.0 }
        };

        private static double[,] RookMap =
        {
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 0.5, 1.0, 1.0, 1.0, 1.0, 1.0, 1.0, 0.5 },
            { -0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5 },
            { -0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5 },
            { -0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5 },
            { -0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5 },
            { -0.5, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, -0.5 },
            { 0.0, 0.0, 0.0, 0.5, 0.5, 0.0, 0.0, 0.0 }
        };

        private static double[,] PawnMap =
        {
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 },
            { 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0, 5.0 },
            { 1.0, 1.0, 2.0, 3.0, 3.0, 2.0, 1.0, 1.0 },
            { 0.5, 0.5, 1.0, 2.5, 2.5, 1.0, 0.5, 0.5 },
            { 0.0, 0.0, 0.0, 2.0, 2.0, 0.0, 0.0, 0.0 },
            { 0.5 , -0.5, -1.0, 0.0, 0.0, -1.0, -0.5, 0.5 },
            { 0.5, 1.0, 1.0, -2.0, -2.0, 1.0, 1.0, 0.5 },
            { 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0, 0.0 }
        };


        private static double PieceLocationScore(int piece, int x, int y)
        {
            //int AllyColor = ((piece & Piece.White) != 0) ? 1 : -1;
            piece = ((piece & Piece.White) != 0) ? piece - Piece.White : piece - Piece.Black;

            switch (piece)
            {
                case Piece.King:
                case (Piece.King | Piece.Moved):
                    {
                        return KingMap[x, y];
                    }

                case Piece.Queen:
                    return QueenMap[x, y];

                case Piece.Bishop:
                    return BishopMap[x, y];

                case Piece.Knight:
                    return KnightMap[x, y];

                case Piece.Rook:
                case (Piece.Rook | Piece.Moved):
                    {
                        return RookMap[x, y];
                    }

                case Piece.Pawn:
                case (Piece.Pawn | Piece.Moved):
                    {
                        return PawnMap[x, y];
                    }

                default:
                    return 0;
            }
        }

        private static int PieceScore(int piece)
        {
            int AllyColor = ((piece & Piece.White) != 0) ? Piece.White : Piece.Black;
            piece = ((piece & Piece.White) != 0) ? piece - Piece.White : piece - Piece.Black;

            switch (piece)
            {
                case Piece.King:
                case (Piece.King | Piece.Moved):
                    {
                        if (AllyColor == Piece.White) return 900;
                        else return -900;
                    }

                case Piece.Queen:
                    if (AllyColor == Piece.White) return 90;
                    else return -90;

                case Piece.Bishop:
                    if (AllyColor == Piece.White) return 30;
                    else return -30;

                case Piece.Knight:
                    if (AllyColor == Piece.White) return 30;
                    else return -30;

                case Piece.Rook:
                case (Piece.Rook | Piece.Moved):
                    {
                        if (AllyColor == Piece.White) return 50;
                        else return -50;
                    }

                case Piece.Pawn:
                case (Piece.Pawn | Piece.Moved):
                    {
                        if (AllyColor == Piece.White) return 10;
                        else return -10;
                    }

                default:
                    return 0;
            }
        }

        private static List<(int, int, int, int, double)> ConvertList(List<(int, int)> ValidMoves, int AllyColor, int x, int y)
        {
            var ConvertedList = new List<(int, int, int, int, double)>();
            foreach (var legalMove in Move.LegalMoves)
            {
                if (ValidMoves.Contains(legalMove) && ChessBoard.Squares[x, y] != Piece.None)
                {
                    (int, int, int, int, double) NewLegalMove = (Move.StartSquareX, Move.StartSquareY, legalMove.Item1, legalMove.Item2, -1.0);
                    NewLegalMove = ComputeMoveScore(NewLegalMove, AllyColor);
                    ConvertedList.Add(NewLegalMove);
                }
            }

            return ConvertedList;
        }

        public static List<(int, int, int, int, double)> GetAllLegalMoves(int AllyColor, List<(int, int)> ValidMoves)
        {
            var AllLegalMoves = new List<(int, int, int, int, double)>();
            int rows = 8, cols = 8;


            for (int i = 0; i < rows * cols; i++)
            {
                int x = i / cols;
                int y = i % cols;
                int piece = ChessBoard.Squares[x, y];
                if ((piece & AllyColor) != 0)
                {
                    Move.StartSquareX = x;
                    Move.StartSquareY = y;
                    LegalMove.ChooselegalMoves(piece);
                    AllLegalMoves.AddRange(ConvertList(ValidMoves, AllyColor, x, y));
                    Move.RemoveBackgroundStep(true);
                }
            }

            Move.StartSquareX = -1;
            Move.StartSquareY = -1;
            return AllLegalMoves;
        }

        private int[,] CopyArray(int[,] SourceArray, int[,] DestinationArray)
        {
            int cols = 8;
            for (int i = 0; i < 64; i++)
            {
                int x = i / cols;
                int y = i % cols;
                DestinationArray[x, y] = SourceArray[x, y];
            }

            return DestinationArray;
        }

        private double MiniMax((List<(int, int)>, List<(int, int, int, int, double)>,bool) ValidMoves, (int, int, int, int, double) Position, int Depth, double Alpha, double Beta, int PlayerColor)
        {
            var WhiteKingOldLocation = Move.WhiteKingLocation;
            var BlackKingOldLocation = Move.BlackKingLocation;
            //var CurrentBoard = (int[,])ChessBoard.Squares.Clone();
            var CurrentBoard = new int[8, 8];
            CurrentBoard = CopyArray(ChessBoard.Squares, CurrentBoard);
            if (Depth == 0 || ValidMoves.Item2.Count == 0)
            {

                return Position.Item5;
            }

            //Max (Black turn)
            if (PlayerColor == Piece.Black)
            {
                var MaxList = ValidMoves.Item2.OrderByDescending(x => x.Item5).ToList();
                for (int i = 0; i < MaxList.Count; i++)
                {
                    var legalMove = MaxList[i];
                    MakeMove(legalMove);    //Making virtual move
                    var NewValidMoves = Move.GetValidMoves(Piece.White, true);
                    var MaxEval = MiniMax(NewValidMoves, legalMove, Depth - 1, Alpha, Beta, Piece.White);
                    Move.BlackKingLocation = BlackKingOldLocation;
                    CopyArray(CurrentBoard, ChessBoard.Squares);    //Undo virtual move
                    //Array.Copy(CurrentBoard, ChessBoard.Squares, ChessBoard.Squares.Length);  //Undo virtual move

                    if (MaxEval > Alpha)
                    {
                        Alpha = MaxEval;
                        if (Depth == DEPTH)
                            BestMove = legalMove;
                    }
                    if (Beta <= Alpha)
                        return Alpha;
                }

                return Alpha;
            }


            //Min (White turn)
            //foreach (var legalMove in ValidMoves)
            var MinList = ValidMoves.Item2.OrderBy(x => x.Item5).ToList();
            for (int i = 0; i < MinList.Count; i++)
            {
                var legalMove = MinList[i];
                MakeMove(legalMove);    //Making virtual move
                var NewValidMoves = Move.GetValidMoves(Piece.Black, true);
                var MinEval = MiniMax(NewValidMoves, legalMove, Depth - 1, Alpha, Beta, Piece.Black);
                Move.WhiteKingLocation = WhiteKingOldLocation;
                CopyArray(CurrentBoard, ChessBoard.Squares);    //Undo virtual move
                //Array.Copy(CurrentBoard, ChessBoard.Squares, ChessBoard.Squares.Length);    //Undo virtual move


                if (MinEval <= Beta)
                    Beta = MinEval;

                if (Beta <= Alpha)
                    return Beta;
            }

            return Beta;
        }

        private void MakeMove((int, int, int, int, double) legalMove)
        {
            Move.StartSquareX = legalMove.Item1;
            Move.StartSquareY = legalMove.Item2;
            Move.TargetSquareX = legalMove.Item3;
            Move.TargetSquareY = legalMove.Item4;

            Move.MovePiece(player1, player2, this, true);
        }

        private static (int, int, int, int, double) ComputeMoveScore((int, int, int, int, double) legalMove, int AllyColor)
        {
            int ColorDirection = (AllyColor == Piece.Black) ? 7 - legalMove.Item3 : legalMove.Item3;

            int Score = PieceScore(ChessBoard.Squares[legalMove.Item3, legalMove.Item4]);
            double LocationScore = PieceLocationScore(ChessBoard.Squares[legalMove.Item1, legalMove.Item2], ColorDirection, legalMove.Item4);

            legalMove.Item5 = (double)(Score + LocationScore);
            return legalMove;
        }

        public void MovePiece()
        {
            if (openingCounter > 2)
            {
                var ValidMoves = Move.GetValidMoves(Piece.Black, true);
                MiniMax(ValidMoves, (-1, -1, -1, -1, float.MaxValue), DEPTH, Int32.MinValue, Int32.MaxValue, Piece.Black);
                if (BestMove.Item1 == -1 && BestMove.Item2 == -1)
                {
                    Move.StartSquareX = Move.BlackKingLocation.Item1;
                    Move.StartSquareY = Move.BlackKingLocation.Item2;
                    LegalMove.ChooselegalMoves(ChessBoard.Squares[Move.StartSquareX, Move.StartSquareY]);

                    Random rnd = new Random();
                    int RandomINdex = rnd.Next(Move.LegalMoves.Count);

                    var KingMove = Move.LegalMoves[RandomINdex];
                    Move.TargetSquareX = KingMove.Item1;
                    Move.TargetSquareY = KingMove.Item2;
                    Move.RemoveBackgroundStep(true);
                }
                else
                {
                    Move.StartSquareX = BestMove.Item1;
                    Move.StartSquareY = BestMove.Item2;
                    Move.TargetSquareX = BestMove.Item3;
                    Move.TargetSquareY = BestMove.Item4;
                }
            }
            else
            {
                Move.StartSquareX = openings[RandomOpening, openingCounter].Item1;
                Move.StartSquareY = openings[RandomOpening, openingCounter].Item2;
                Move.TargetSquareX = openings[RandomOpening, openingCounter].Item3;
                Move.TargetSquareY = openings[RandomOpening, openingCounter].Item4;

                openingCounter++;
            }

            
            Move.CreateMove(null, Piece.Black, player1, player2, this, true);
        }
    }
}