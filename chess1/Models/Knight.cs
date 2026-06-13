using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess1.Models
{
    public class Knight: Figure
    {
        public Knight(FigureColor color)
        {
            Type = FigureType.Knight;
            Color = color;
        }

        public override List<Position> GetPossibleMoves(Position currPosition, Board board)
        {
            List<Position> possibleMoves = new List<Position>();

            // Все 8 возможных L-образных ходов коня
            int[] rowOffsets = { -2, -2, -1, -1, 1, 1, 2, 2 };
            int[] colOffsets = { -1, 1, -2, 2, -2, 2, -1, 1 };

            for (int i = 0; i < rowOffsets.Length; i++)
            {
                int newRow = currPosition.Row + rowOffsets[i];
                int newCol = currPosition.Col + colOffsets[i];
                Position newPosition = new Position(newRow, newCol);

                // Проверяем, что позиция в пределах доски
                if (!board.IsValidPosition(newPosition))
                    continue;

                Figure target = board.GetFigureAt(newPosition);

                // Конь может ходить на пустую клетку или бить фигуру противника
                if (target == null || target.Color != Color)
                {
                    possibleMoves.Add(newPosition);
                }
            }

            return possibleMoves;
        }
    }
}
