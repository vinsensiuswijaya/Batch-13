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
    private bool isGameOver;
    public GameController(List<IPlayer> players, IBoard board)
    {
        Players = players;
        Board = board;
    }
    public void StartGame()
    {
        Board.Initialize();
        _currentPlayerIndex = 0;
        currentPlayer = Players[_currentPlayerIndex];
        isGameOver = false;
        while (!isGameOver)
        {
            DisplayBoard();
            int row;
            int col;
            string inputRow;
            // string[] splitInput;
            string inputCol;
            bool isSuccessInputRow = false, isSuccessInputCol = false;
            while (!isSuccessInputRow || !isSuccessInputCol)
            {
                Console.Write($"{currentPlayer.Name} ({PieceColorMap(currentPlayer.Color)}), Input row: ");
                inputRow = Console.ReadLine();
                Console.Write($"{currentPlayer.Name} ({PieceColorMap(currentPlayer.Color)}), Input column: ");
                inputCol = Console.ReadLine();
                isSuccessInputRow = int.TryParse(inputRow.Trim(), out row);
                isSuccessInputCol = int.TryParse(inputCol.Trim(), out col);
                Position pos = new Position(row, col);
                Console.WriteLine($"row: {pos.Row}, col: {pos.Col}");
            }
            SwitchPlayer();
            // int inputCol = int.Parse(split[1].Trim());
            isGameOver = true;
        }
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
    public void SwitchPlayer()
    {
        if (_currentPlayerIndex == Players.Count - 1) _currentPlayerIndex = 0;
        else _currentPlayerIndex++;
        currentPlayer = Players[_currentPlayerIndex];
    }
    
    public void CountPieces(out int black, out int white)
    {
        // TODO
        black = 0;
        white = 0;
    }
    public void DisplayBoard()
    {
        for (int row = 0; row <= Board.Size; row++)
        {
            for (int col = 0; col <= Board.Size; col++)
            {
                if (row < Board.Size && col < Board.Size) Console.Write($"{PieceColorMap(Board.Grid[row, col].Color)} ");
                if (row == Board.Size && row != col) Console.Write($"{col} ");
            }
            if (row < Board.Size) Console.WriteLine($"{row} ");
        }
        Console.WriteLine();
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
        Board board = new Board(8);
        List<IPlayer> players = new List<IPlayer>();
        Player player1 = new Player("Player 1", PieceColor.Black);
        Player player2 = new Player("Player 2", PieceColor.White);
        players.Add(player1);
        players.Add(player2);
        GameController game = new GameController(players, board);
        game.StartGame();
    }
}