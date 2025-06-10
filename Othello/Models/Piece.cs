using Othello.Interfaces;

namespace Othello.Models
{
    public class Piece : IPiece
    {
        public PieceColor Color { get; }
        public Piece(PieceColor color)
        {
            Color = color;
        }
    }
}