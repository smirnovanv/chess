using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess1.Models
{
    public class Board
    {
        private Cell[,] cells;
        public const int Size = 8;
        public Cell LastSelectedCell;
        private Stack<Move> moveHistory; // Стек для истории ходов
        public FigureColor CurrentPlayerColor;
        public List<Position> possibleMoves;

        // Событие для уведомления о новом ходе
        public event EventHandler<Move> MoveMade;
        public Board()
        {
            cells = new Cell[Size, Size];
            moveHistory = new Stack<Move>();
            CurrentPlayerColor = FigureColor.White;
            InitializeBoard();
        }

        private void InitializeBoard()
        {
            // Пустые клетки
            for (int i = 0; i < Size; i++) 
            {
                for (int j = 0; j < Size; j++) 
                {
                    CellType cellType = (i + j) % 2 == 0 ? CellType.Light : CellType.Dark;
                   
                    cells[i, j] = new Cell(cellType, i, j);
                }
            }


            // заполняем пешками (пока отдельно)

            for (int i = 0; i < 8; i++)
            {
                cells[1, i].UpdateFigure(new Pawn(FigureColor.Black));
                cells[6, i].UpdateFigure(new Pawn(FigureColor.White));
            }

            cells[0, 4].UpdateFigure(new King(FigureColor.Black));
            cells[7, 4].UpdateFigure(new King(FigureColor.White));
        }

        public Figure GetFigureAt(Position pos)
        {
            if (IsValidPosition(pos)) 
            {
                return cells[pos.Row, pos.Col].Figure;
            }
                
            return null;
        }

        public Cell GetCellAt(int col, int row)
        {
            return cells[row, col];
        }

        public List<Cell> MoveFigure(Cell startCell, Cell endCell)
        {
            Figure movingFigure = startCell.Figure;
            Figure capturedFigure = endCell.Figure;

            List<Cell> cellsToUpdate = new List<Cell> { startCell, endCell};

            // для взятия на проходе
            if (movingFigure is Pawn && startCell.Position.Col != endCell.Position.Col && endCell.Figure == null)
            {
                Position enPassantCapturedPosition = new Position(startCell.Position.Row, endCell.Position.Col);

                capturedFigure = GetFigureAt(enPassantCapturedPosition); // для истории

                Cell capturedCell = GetCellAt(enPassantCapturedPosition.Col, enPassantCapturedPosition.Row);
                capturedCell.Figure = null;
                cellsToUpdate.Add(capturedCell);
            }

            startCell.Figure = null;
            movingFigure.OnMove();
            endCell.Figure = movingFigure;

            // Сохраняем ход в историю
            Move move = new Move(startCell.Position, endCell.Position, movingFigure, capturedFigure);
            moveHistory.Push(move);

            MoveMade?.Invoke(this, move); // для истории

            return cellsToUpdate;
        }

        public List<Move> GetAllMoves()
        {
            return moveHistory.Reverse().ToList(); // От более старых к новым
        }

        public Move GetLastMove()
        {
            if (moveHistory.Count == 0)
            {
                return null;
            }

            return moveHistory.Peek();
        }

        public bool IsEmptyCell(Position pos) {
            return GetFigureAt(pos) == null;
        }

        public bool IsValidPosition(Position pos)
        {
            return pos.Row >= 0 && pos.Row < Size && pos.Col >= 0 && pos.Col < Size;
        }
    
        public bool IsFigureSelection(Cell prevSelectedCell, Cell currSelectedCell)
        {
            if (prevSelectedCell == null && IsPlayerFigure(currSelectedCell))
            {
                return true;
            }

            if (IsPlayerFigure(currSelectedCell) && !IsPossibleMove(currSelectedCell.Position))
            {
                return true;
            }

            return false;
        }

        private bool IsPlayerFigure(Cell cell)
        {
            Figure figure = cell.Figure;

            return figure != null && (figure.Color == CurrentPlayerColor);
        }

        public bool IsPossibleMove(Position position)
        {
            if (possibleMoves == null) return false;

            return possibleMoves.Any(move =>
                move.Row == position.Row && move.Col == position.Col);
        }

    }
}
