using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess1.Models
{
    public class Cell
    {
        public CellType type;
        public Figure figure;

        public Cell(CellType cellType)
        {
            type = cellType;
            figure = null;
        }

        public void UpdateFigure(Figure cellFigure) {
            figure = cellFigure;
        }

        //public void SetType(CellType cellType)
        //{
        //    type = cellType;
        //}
    }
}
