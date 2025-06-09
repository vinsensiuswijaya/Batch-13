namespace Othello
{
    public class Board : IBoard
    {
        public IPiece[,] Grid { get; }
        public int Size { get; }
        public Board(int size)
        {
            Size = size;
            Grid = new IPiece[Size, Size];
            for (int row = 0; row < Size; row++)
            {
                for (int col = 0; col < Size; col++)
                {
                    Grid[row, col] = new Piece(PieceColor.None);
                }
            }
        }
        public void Initialize()
        {
            int midGrid = Size / 2;
            Grid[midGrid - 1, midGrid - 1] = new Piece(PieceColor.White);
            Grid[midGrid - 1, midGrid] = new Piece(PieceColor.Black);
            Grid[midGrid, midGrid - 1] = new Piece(PieceColor.Black);
            Grid[midGrid, midGrid] = new Piece(PieceColor.White);
        }
        public bool IsInBounds(Position pos)
        {
            return pos.Col >= 0 && pos.Col < Size && pos.Row >= 0 && pos.Row < Size;
        }
    }
}