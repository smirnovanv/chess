using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess1.Models
{
    public class Rook: Figure
    {
        public Rook(FigureColor color)
        {
            Type = FigureType.Rook;
            Color = color;
        }

        public override List<Position> GetPossibleMoves(Position currPosition, Board board)
        {
            List<Position> possibleMoves = new List<Position>();

            // Ладья ходит по вертикали и горизонтали (4 направления)
            int[] rowDirections = { -1, 1, 0, 0 };  // вверх, вниз, по горизонтали
            int[] colDirections = { 0, 0, -1, 1 };  // по вертикали, влево, вправо

            // Проверяем каждое направление
            for (int i = 0; i < rowDirections.Length; i++)
            {
                AddMovesInDirection(currPosition, rowDirections[i], colDirections[i], board, possibleMoves);
            }

            return possibleMoves;
        }

        private void AddMovesInDirection(Position current, int rowOffset, int colOffset, Board board, List<Position> moves)
        {
            // Двигаемся в направлении, пока не упремся в край доски или фигуру
            for (int step = 1; step <= 7; step++)
            {
                int newRow = current.Row + rowOffset * step;
                int newCol = current.Col + colOffset * step;

                Position newPosition = new Position(newRow, newCol);

                // Если вышли за пределы доски - останавливаемся
                if (!board.IsValidPosition(newPosition))
                    break;

                Figure target = board.GetFigureAt(newPosition);

                // Если клетка пуста - добавляем ход и продолжаем
                if (target == null)
                {
                    moves.Add(newPosition);
                }
                // Если стоит фигура противника - добавляем ход (взятие) и останавливаемся
                else if (target.Color != Color)
                {
                    moves.Add(newPosition);
                    break;
                }
                // Если стоит своя фигура - останавливаемся, не добавляя ход
                else
                {
                    break;
                }
            }
        }
    }
}
