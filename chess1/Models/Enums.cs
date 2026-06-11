using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess1.Models
{
    public enum FigureColor
    {
        White,
        Black
    }

    public enum FigureType
    {
        Pawn,
        Knight,
        Bishop,
        Rook,
        Queen,
        King
    }

    public enum CellType
    {
        Light,
        Dark
    }
}
