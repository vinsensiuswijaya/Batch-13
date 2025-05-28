public struct Position
{
    public int Row { get; }
    public int Col { get; }
    public Position(int row, int col)
    {
        Row = row;
        Col = col;
    }
}
public enum PieceColor
{
    Black = 1,
    None = 0,
    White = -1
}
public interface IPiece
{
    public PieceColor Color { get; }
}

public class Piece : IPiece
{
    public PieceColor Color { get; }
    public Piece(PieceColor color)
    {
        Color = color;
    }
}

public interface IPlayer
{
    public string Name { get; }
    public PieceColor Color{ get; }
}

public class Player : IPlayer
{
    public string Name { get; }
    public PieceColor Color{ get; }
    public Player(string name, PieceColor color)
    {
        Name = name;
        Color = color;
    }
}

public interface IBoard
{
    public IPiece[,] Grid { get; }
    public int Size { get; }
    public void Initialize();
    public bool IsInBounds(Position pos);
}

public class Board : IBoard
{
    public IPiece[,] Grid { get; }
    public int Size { get; }
    public Board(int size)
    {
        Size = size;
        IPiece[,] grid = new IPiece[Size, Size];
        for (int i = 0; i < Size; i++)
        {
            for (int j = 0; j < Size; j++)
            {
                grid[i, j] = new Piece(PieceColor.None);
            }
        }
        Grid = grid;
    }
    public void Initialize()
    {
        int midGrid = (int)Size / 2;
        Grid[midGrid - 1, midGrid - 1] = new Piece(PieceColor.White);
        Grid[midGrid - 1, midGrid] = new Piece(PieceColor.Black);
        Grid[midGrid, midGrid - 1] = new Piece(PieceColor.Black);
        Grid[midGrid, midGrid] = new Piece(PieceColor.White);
    }
    public bool IsInBounds(Position pos)
    {
        return pos.Col > 0 && pos.Col < Size && pos.Row > 0 && pos.Row < Size;
    }
}

public class GameController
{
    public IBoard Board;
    public List<IPlayer> Players;
    public IPlayer currentPlayer;
    private int _currentPlayerIndex;
    private List<Position> _directions;
    public GameController(List<IPlayer> players, IBoard board)
    {
        Players = players;
        Board = board;
    }
    public void StartGame()
    {
        // TODO
    }
    public bool GameOver()
    {
        // TODO
        return true;
    }
    public void MakeMove(Position pos)
    {
        // TODO
    }
    public bool IsValidMove(Position pos, PieceColor color)
    {
        // TODO
        return true;
    }
    public List<Position> GetValidMove(PieceColor color)
    {
        // TODO
        return null;
    }
    public void PlacePiece(Position pos, PieceColor color)
    {
        // TODO
    }
    public void FlipPiece(Position pos, PieceColor color)
    {
        // TODO
    }
    public void SwitchPlayer() { }
    public void CountPieces(out int black, out int white)
    {
        // TODO
        black = 0;
        white = 0;
    }
    public void Display()
    {
        for (int row = 0; row <= Board.Size; row++)
        {
            for (int col = 0; col <= Board.Size; col++)
            {
                if (row < Board.Size && col < Board.Size) Console.Write($"{PieceColorMap(Board.Grid[row, col].Color)} ");
                if (row == Board.Size && row != col) Console.Write($"{col + 1} ");
            }
            if (row < Board.Size) Console.WriteLine($"{row + 1} ");
        }
    }
    private char PieceColorMap(PieceColor color) // Helper method for Display to map the piece's color to a character 
    {
        return color switch
        {
            PieceColor.Black => 'x',
            PieceColor.None => '.',
            PieceColor.White => 'o'
        };
    }
}

class Program
{
    static void Main()
    {
        // TODO
    }
}