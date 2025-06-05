namespace Othello
{
    public interface IBoard
    {
        public IPiece[,] Grid { get; }
        public int Size { get; }
        public void Initialize();
        public bool IsInBounds(Position pos);
    }
}