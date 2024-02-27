using Chess.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Chess.Classes
{
    public class Player
    {
        public string Name;
        public int Color;
        public int num_pieces;
        private bool turn;

        public Player(string name, bool turn, int color)
        {
            Name = name;
            Color = color;
            this.turn = turn;
        }

        public bool Turn { get { return turn; } set { turn = value; } }
        

        public static void Check_Turn(Button btn, Ellipse Circle, Player player1, Player player2, AIPlayer Computer, bool IsAI)
        {
            if (player1.Turn == true)
                Move.CreateMove(btn, player1.Color, player1, player2, Computer, false);

            else
            {
                if (IsAI)
                    Computer.MovePiece();
                else
                    Move.CreateMove(btn, player2.Color, player1, player2, Computer, false);
            }
        }
    }
}
