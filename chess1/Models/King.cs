using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess1.Models
{
    public class King: Figure
    {
        public King(FigureColor color)
        {
            Type = FigureType.King;
            Color = color;
        }

        public override List<Position> GetPossibleMoves(Position currPosition, Board board) 
        {
            List<Position> possibleMoves = new List<Position>();

            // Все 8 направлений для короля
            int[] rowDirections = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] colDirections = { -1, 0, 1, -1, 1, -1, 0, 1 };

            // Проверка каждого направления
            for (int i = 0; i < rowDirections.Length; i++)
            {
                AddMoveInDirection(currPosition, rowDirections[i], colDirections[i], board, possibleMoves);
            }

            return possibleMoves;
        }

        private void AddMoveInDirection(Position current, int rowOffset, int colOffset, Board board, List<Position> moves)
        {
            Position newPosition = new Position(current.Row + rowOffset, current.Col + colOffset);

            if (IsValidPosition(newPosition))
            {
                Figure target = board.GetFigureAt(newPosition);

                // Клетка пуста или содержит фигуру противника
                if (target == null)
                {
                    moves.Add(newPosition);
                }
                else if (target.Color != Color)
                {
                    moves.Add(newPosition); // Взятие
                }
            }
        }


    }
}
