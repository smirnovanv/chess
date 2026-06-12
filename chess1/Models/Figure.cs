using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess1.Models
{
    public abstract class Figure
    {
        public FigureType Type { get; protected set; }
        public FigureColor Color { get; set; }

        public abstract List<Position> GetPossibleMoves(Position currPosition, Board board);
    }
}
