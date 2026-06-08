using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess1.Models
{
    public class Board
    {
        private Figure[,] figures;
        public const int Size = 8;

        public Board()
        {
            figures = new Figure[Size, Size];
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Пустые клетки
            for (int i = 0; i < Size; i++)
                for (int j = 0; j < Size; j++)
                    figures[i, j] = null;


            for (int i = 0; i < 8; i++)
            {
                figures[1, i] = new Pawn(FigureColor.Black);
                figures[6, i] = new Pawn(FigureColor.White);
            }
        }

        public Figure GetFigureAt(Position pos)
        {
            //if (IsValidPosition(pos))
            //    return pieces[pos.Row, pos.Col];
            //return null;

            return figures[pos.Row, pos.Col];
        }

    }
}
