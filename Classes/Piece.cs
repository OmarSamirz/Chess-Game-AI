using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chess.Classes
{
    public static class Piece
    {
        public const int None = 0;
        public const int King = 1;
        public const int Queen = 2;
        public const int Bishop = 3;
        public const int Knight = 4;
        public const int Rook = 5;
        public const int Pawn = 6;

        public const int White = 8;
        public const int Black = 16;
        public const int Moved = 32;
        

        public static bool CheckColor(int StartPieceColor, int TargetPieceColor)
        {
            if (((StartPieceColor & White) != 0) && ((TargetPieceColor & White) != 0))
                return false;

            if (((StartPieceColor & Black) != 0) && ((TargetPieceColor & Black) != 0))
                return false;

            return true;
        }
    }
}
