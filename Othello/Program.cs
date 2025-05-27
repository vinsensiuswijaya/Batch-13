public struct Position
{
    public int Row;
    public int Col;
}
public enum PieceColor
{
    Black = 1,
    White = -1
}
public interface IPlayer
{
    string name();
    PieceColor color();
}
public interface IPiece
{
    PieceColor color();
}
public interface IBoard
{
    IPiece[,] Grid();
    int size();
    void initialize();
    bool isInBounds(Position pos);
}

public class Player
{
    string Name;
    PieceColor Color;
    Player(string name, PieceColor color)
    {
        Name = name;
        Color = color;
    }
}

public class Board
{
    IPiece[,] Grid;
    int Size;
    Board(IPiece[,] grid, int size)
    {
        Grid = grid;
        Size = size;
    }
}

public class Piece
{
    PieceColor Color;
    Piece(PieceColor color)
    {
        Color = color;
    }
}
public class GameController
{
    IBoard Board;
    List<IPlayer> Players;
    int _currentPlayerIndex;
    List<Position> _directions;
    GameController(List<IPlayer> players, IBoard board)
    {
        Players = players;
        Board = board;
    }
    public void StartGame() { }
    public bool GameOver()
    {
        return true;
    }
    public void MakeMove(Position pos) { }
    public bool IsValidMove(Position pos, PieceColor color)
    {
        return true;
    }
    public List<Position> GetValidMove(PieceColor color)
    {
        return null;
    }
    public void PlacePiece(Position pos, PieceColor color) { }
    public void FlipPiece(Position pos, PieceColor color) { }
    public void SwitchPlayer() { }
    // public void CountPieces(out int black, out int white){}
}