using System.Diagnostics.CodeAnalysis;
using System.Drawing;

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
    private List<Position> _directions = new List<Position>{
        new Position(-1, -1), 
        new Position(-1, 0),
        new Position(-1, 1),
        new Position(0, -1),
        new Position(0, 1),
        new Position(1, -1),
        new Position(1, 0),
        new Position(1, 1)
    };
    private bool isGameOver;
    public GameController(List<IPlayer> players, IBoard board)
    {
        Players = players;
        Board = board;
    }
    public void StartGame()
    {
        int black = 0, white = 0; // Count of black and white pieces
        Board.Initialize();
        _currentPlayerIndex = 0;
        currentPlayer = Players[_currentPlayerIndex];
        isGameOver = false;
        while (!isGameOver)
        {
            CountPieces(out black, out white);
            Display(black, white);
            string inputRow;
            string inputCol;
            int row;
            int col;
            Position pos = new Position();
            bool isValidMove = false;
            while (!isValidMove)
            {    
                bool isSuccessInputRow = false, isSuccessInputCol = false;
                while (!isSuccessInputRow || !isSuccessInputCol)
                {
                    Console.Write($"{currentPlayer.Name} ({PieceColorMap(currentPlayer.Color)}), Input row: ");
                    inputRow = Console.ReadLine();
                    Console.Write($"{currentPlayer.Name} ({PieceColorMap(currentPlayer.Color)}), Input column: ");
                    inputCol = Console.ReadLine();
                    isSuccessInputRow = int.TryParse(inputRow.Trim(), out row);
                    isSuccessInputCol = int.TryParse(inputCol.Trim(), out col);
                    pos = new Position(row, col);
                    isValidMove = IsValidMove(pos, currentPlayer.Color);
                    if (!isValidMove) Console.WriteLine("Position invalid! Please reenter position.");
                }
            }
            MakeMove(pos);
            // CountPieces(out black, out white);
            // Display(black, white);
            // isGameOver = true;
        }
    }
    public bool GameOver()
    {
        // TODO
        return true;
    }
    public void MakeMove(Position pos)
    {
        PlacePiece(pos, currentPlayer.Color);
        // FlipPiece(position, color) Use loop to flip all the pieces?
        SwitchPlayer();
    }
    public bool IsValidMove(Position pos, PieceColor color)
    {
        if (Board.Grid[pos.Row, pos.Col].Color != PieceColor.None) return false;
        List<Position> validMoves = GetValidMove(color);
        if (validMoves.Contains(pos)) return true;
        else return false;
    }
    public List<Position> GetValidMove(PieceColor color)
    {
        List<Position> validMoves = new List<Position>();
        PieceColor opponentColor = (currentPlayer.Color == PieceColor.Black) ? PieceColor.White : PieceColor.Black;

        for (int row = 0; row < Board.Size; row++)
        {
            for (int col = 0; col < Board.Size; col++)
            {
                if (Board.Grid[row, col].Color != PieceColor.None)
                {
                    continue; // Skip squares that are not empty
                }

                bool foundValidLine = false;
                foreach (Position dir in _directions)
                {
                    int r = row + dir.Row;
                    int c = col + dir.Col;

                    // Check for an adjacent opponent piece
                    if (r < 0 || r >= Board.Size || c < 0 || c >= Board.Size || Board.Grid[r, c].Color != opponentColor)
                    {
                        continue;
                    }

                    // Trace along the line to find a friendly piece
                    r += dir.Row;
                    c += dir.Col;
                    while (r >= 0 && r < Board.Size && c >= 0 && c <= Board.Size)
                    {
                        if (Board.Grid[r, c].Color == currentPlayer.Color)
                        {
                            // Found a valid line, add the move.
                            validMoves.Add(new Position(row, col));
                            foundValidLine = true;
                            break; // Stop checking other directions for this square
                        }
                        if (Board.Grid[r, c].Color == PieceColor.None)
                        {
                            break; // Line is broken by an empty space
                        }
                        r += dir.Row;
                        c += dir.Col;
                    }
                    if (foundValidLine) break; // Exit the direction loop and move to the next board square
                }
            }
        }
        
        return validMoves;
    }
    public void PlacePiece(Position pos, PieceColor color)
    {
        Board.Grid[pos.Row, pos.Col] = new Piece(color);
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
        black = 0;
        white = 0;
        for (int row = 0; row < Board.Size; row++)
        {
            for (int col = 0; col < Board.Size; col++)
            {
                if (Board.Grid[row, col].Color == PieceColor.Black) black++;
                else if (Board.Grid[row, col].Color == PieceColor.White) white++;
            }
        }
    }
    public void Display(int blackCount, int whiteCount)
    {
        Console.Clear();
        Console.WriteLine("=========================");
        Console.WriteLine("=========OTHELLO=========");
        Console.WriteLine("=========================\n");
        Console.WriteLine($"Count (x): {blackCount}, (o): {whiteCount}");
        Console.WriteLine("-------------------------");
        PrintBoard();
        Console.WriteLine("-------------------------");
    }
    private void PrintBoard()
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