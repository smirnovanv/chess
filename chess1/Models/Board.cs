using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess1.Models
{
    public class Board
    {
        //private Figure[,] figures;
        private Cell[,] cells;
        public const int Size = 8;
        public Cell selectedStartCell;

        public Board()
        {
            cells = new Cell[Size, Size];
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
        }

        public Figure GetFigureAt(Position pos)
        {
            //if (IsValidPosition(pos))
            //    return pieces[pos.Row, pos.Col];
            //return null;

            return cells[pos.Row, pos.Col].Figure;
        }

        public Cell GetCellAt(int col, int row)
        {
            return cells[row, col];
        }

        public void MoveFigure(Cell startCell, Cell endCell)
        {
            Figure movingFigure = startCell.Figure;
            
            if (startCell.Figure != null && endCell.Figure == null)
            {
                startCell.Figure = null;
                endCell.Figure = movingFigure;

            }
        }

    }
}
