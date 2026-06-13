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
        public bool HasMoved { get; set; } = false;

        public abstract List<Position> GetPossibleMoves(Position currPosition, Board board);

        public bool IsValidPosition(Position pos)
        {
            return pos.Row >= 0 && pos.Row < 8 && pos.Col >= 0 && pos.Col < 8;
        }

        public virtual void OnMove()
        {
            HasMoved = true;
        }
    }
}
