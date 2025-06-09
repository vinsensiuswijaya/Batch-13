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
            // Default initialization
            int midGrid = Size / 2;
            Grid[midGrid - 1, midGrid - 1] = new Piece(PieceColor.White);
            Grid[midGrid - 1, midGrid] = new Piece(PieceColor.Black);
            Grid[midGrid, midGrid - 1] = new Piece(PieceColor.Black);
            Grid[midGrid, midGrid] = new Piece(PieceColor.White);

            // Set up a simple board where Player 1 (Black) has no valid moves.
            // Grid[0, 1] = new Piece(PieceColor.Black);
            // Grid[0, 2] = new Piece(PieceColor.White);
            // Grid[0, 3] = new Piece(PieceColor.White);
            // Grid[0, 4] = new Piece(PieceColor.Black);

        }
        public bool IsInBounds(Position pos)
        {
            return pos.Col >= 0 && pos.Col < Size && pos.Row >= 0 && pos.Row < Size;
        }
    }
}