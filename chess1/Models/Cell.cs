using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess1.Models
{
    public class Cell
    {
        public CellType Type;
        public Figure Figure;
        public Position Position { get; set; }


        public Cell(CellType cellType, int row, int col)
        {
            Type = cellType;
            Figure = null;
            Position = new Position(row, col);
        }

        public void UpdateFigure(Figure cellFigure) {
            Figure = cellFigure;
        }
    }
}
