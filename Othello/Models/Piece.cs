namespace Othello
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