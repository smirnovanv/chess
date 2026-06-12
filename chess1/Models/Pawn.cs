using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess1.Models
{
    public class Pawn: Figure
    {
        public Pawn(FigureColor color)
        {
            Type = FigureType.Pawn;
            Color = color;
        }

        public override List<Position> GetPossibleMoves(Position currPosition, Board board)
        {
            List<Position> possibleMoves = new List<Position>();

            int direction = Color == FigureColor.White ? -1 : 1;
            int startRow = Color == FigureColor.White ? 6 : 1;

            // 1. Проверка хода вперед
            AddForwardMove(currPosition, direction, board, possibleMoves);

            // 2. Проверка двойного хода
            AddDoubleMove(currPosition, direction, startRow, board, possibleMoves);

            // 3. Проверка взятия по диагоналям
            AddDiagonalCaptures(currPosition, direction, board, possibleMoves);

            return possibleMoves;
        }

        private void AddForwardMove(Position current, int direction, Board board, List<Position> moves)
        {
            Position forward = new Position(current.Row + direction, current.Col);
            if (IsValidPosition(forward) && board.IsEmptyCell(forward))
            {
                moves.Add(forward);
            }
        }

        private void AddDoubleMove(Position current, int direction, int startRow, Board board, List<Position> moves)
        {
            if (current.Row == startRow)
            {
                Position doubleForward = new Position(current.Row + 2 * direction, current.Col);
                Position middle = new Position(current.Row + direction, current.Col);

                if (IsValidPosition(doubleForward) && board.IsEmptyCell(doubleForward) && board.IsEmptyCell(middle))
                {
                    moves.Add(doubleForward);
                }
            }
        }

        private void AddDiagonalCaptures(Position current, int direction, Board board, List<Position> moves)
        {
            // Левая диагональ
            Position leftDiagonal = new Position(current.Row + direction, current.Col - 1);
            CheckAndAddCapture(leftDiagonal, board, moves);

            // Правая диагональ
            Position rightDiagonal = new Position(current.Row + direction, current.Col + 1);
            CheckAndAddCapture(rightDiagonal, board, moves);
        }

        private void CheckAndAddCapture(Position diagonal, Board board, List<Position> moves)
        {
            if (IsValidPosition(diagonal))
            {
                Figure target = board.GetFigureAt(diagonal);
                if (target != null && target.Color != Color)
                {
                    moves.Add(diagonal);
                }
            }
        }

        private bool IsValidPosition(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Col >= 0 && pos.Col < 8;
        }
    }
}
