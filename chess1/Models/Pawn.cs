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
    }
}
