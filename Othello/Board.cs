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
            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    Grid[i, j] = new Piece(PieceColor.None);
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