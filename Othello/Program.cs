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
        // TODO Board
    }
    public void Initialize()
    {
        // TODO
        Console.WriteLine("TODO Initialize : Board");
    }
    public bool IsInBounds(Position pos)
    {
        // TODO
        Console.WriteLine("TODO IsInBound : Board");
        return true;
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
}

class Program
{
    static void Main()
    {
        // TODO
    }
}