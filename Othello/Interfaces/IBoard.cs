namespace Othello.Interfaces
{
    public interface IBoard
    {
        public IPiece[,] Grid { get; }
        public int Size { get; }
        public bool IsInBounds(Position pos);
    }
}