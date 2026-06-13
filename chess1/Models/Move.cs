using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace chess1.Models
{
    public class Move
    {
        public Position From { get; set; }
        public Position To { get; set; }
        public Figure Piece { get; set; }
        public Figure CapturedPiece { get; set; }

        public Move(Position from, Position to, Figure piece, Figure capturedPiece = null)
        {
            From = from;
            To = to;
            Piece = piece;
            CapturedPiece = capturedPiece;
        }

        public override string ToString()
        {
            string capture = CapturedPiece != null ? "x" : "";
            return $"{Piece.Type} {From} {capture} {To}";
        }
    }
}
