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
        return pos.Col >= 0 && pos.Col < Size && pos.Row >= 0 && pos.Row < Size;
    }
}

public class GameController
{
    public IBoard Board;
    public List<IPlayer> Players;
    public IPlayer currentPlayer;
    private int _currentPlayerIndex;
    private List<Position> _directions = new List<Position>{
        new Position(-1, -1), new Position(-1, 0), new Position(-1, 1),
        new Position(0, -1),                       new Position(0, 1),
        new Position(1, -1),  new Position(1, 0),  new Position(1, 1)
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

            if (GetValidMoves(currentPlayer.Color).Count == 0)
            {
                Console.WriteLine($"{currentPlayer.Name} has no valid move. {currentPlayer.Name}'s turn is skipped");
                Console.ReadKey();
                SwitchPlayer();
                continue;
            }

            int row, col;
            Position pos = new Position();
            bool isValidMove = false;
            while (!isValidMove)
            {
                Console.Write($"{currentPlayer.Name} ({PieceColorMap(currentPlayer.Color)}), Input row: ");
                string inputRow = Console.ReadLine();
                Console.Write($"{currentPlayer.Name} ({PieceColorMap(currentPlayer.Color)}), Input column: ");
                string inputCol = Console.ReadLine();
                bool isSuccessInputRow = int.TryParse(inputRow.Trim(), out row);
                bool isSuccessInputCol = int.TryParse(inputCol.Trim(), out col);

                if (!isSuccessInputRow || !isSuccessInputCol)
                {
                    Console.WriteLine("Invalid input! Please enter numbers for row and column.");
                    continue;
                }

                pos = new Position(row, col);
                isValidMove = IsValidMove(pos, currentPlayer.Color);
                if (!isValidMove) Console.WriteLine("Invalid position! Please reenter position.");
            }
            MakeMove(pos);
            isGameOver = IsGameOver();
            if (!isGameOver) SwitchPlayer();
        }
        string winner;
        if (black > white) winner = Players.First(p => p.Color == PieceColor.Black).Name;
        else if (white > black) winner = Players.First(p => p.Color == PieceColor.White).Name;
        else winner = "It's a tie!";
        Console.WriteLine($"Game Over! {(winner == "It's a tie!" ? winner : winner + " Wins!")}");
    }
    public bool IsGameOver()
    {
        if (GetValidMoves(PieceColor.Black).Count == 0 && GetValidMoves(PieceColor.White).Count == 0) return true;
        else return false;
    }
    public void MakeMove(Position pos)
    {
        PlacePiece(pos, currentPlayer.Color);
        FlipPiece(pos, currentPlayer.Color);
    }
    public bool IsValidMove(Position pos, PieceColor color)
    {
        if (!Board.IsInBounds(pos)) return false;
        if (Board.Grid[pos.Row, pos.Col].Color != PieceColor.None) return false;
        foreach (Position dir in _directions)
        {
            List<Position> flipsInThisDirection = GetFlipsInSingleDirection(pos, dir, color);
            if (flipsInThisDirection.Count > 0) return true;
        }
        return false;
    }
    public List<Position> GetValidMoves(PieceColor color)
    {
        List<Position> validMoves = new List<Position>();
        
        for (int row = 0; row < Board.Size; row++)
        {
            for (int col = 0; col < Board.Size; col++)
            {
                if (IsValidMove(new Position(row, col), color)) validMoves.Add(new Position(row, col));
            }
        }
        return validMoves;
    }

    public List<Position> GetFlipsInSingleDirection(Position pos, Position direction, PieceColor color)
    {
        List<Position> flipsInThisLine = new List<Position>();
        PieceColor opponentColor = (color == PieceColor.Black) ? PieceColor.White : PieceColor.Black;

        int r = pos.Row + direction.Row;
        int c = pos.Col + direction.Col;

        while (r >= 0 && r < Board.Size && c >= 0 && c < Board.Size)
        {
            if (Board.Grid[r, c].Color == opponentColor)
            {
                flipsInThisLine.Add(new Position(r, c));
            }
            else if (Board.Grid[r, c].Color == color)
            {
                // Valid line found if there were opponent pieces in between
                return flipsInThisLine.Count > 0 ? flipsInThisLine : new List<Position>();
            }
            else // Empty square
            {
                return new List<Position>(); // Invalid line, no flips in this direction
            }
            r += direction.Row;
            c += direction.Col;
        }
        // Reached the edge of the board without finding a capping player's piece
        return new List<Position>();
    }

    public List<Position> GetFlankedPieces(Position pos, PieceColor color)
    {
        List<Position> allFlankedPieces = new List<Position>();
        foreach (Position dir in _directions)
        {
            List<Position> lineFlips = GetFlipsInSingleDirection(pos, dir, color);
            allFlankedPieces.AddRange(lineFlips);
        }
        return allFlankedPieces;
    }

    public void PlacePiece(Position pos, PieceColor color)
    {
        Board.Grid[pos.Row, pos.Col] = new Piece(color);
    }
    public void FlipPiece(Position pos, PieceColor color)
    {
        List<Position> flankedPieces = GetFlankedPieces(pos, color);
        foreach (Position piece in flankedPieces)
        {
            PlacePiece(piece, color);
        }
    }
    public void SwitchPlayer()
    {
        _currentPlayerIndex = (_currentPlayerIndex + 1) % Players.Count;
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
        Console.Write("  ");
        for (int col = 0; col < Board.Size; col++)
        {
            Console.Write($"{col} ");
        }
        Console.WriteLine();

        for (int row = 0; row < Board.Size; row++)
        {
            Console.Write($"{row} ");
            for (int col = 0; col < Board.Size; col++)
            {
                Console.Write($"{PieceColorMap(Board.Grid[row, col].Color)} ");
            }
            Console.WriteLine();
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
        bool playAgain = true;
        while (playAgain)
        {
            Board board = new Board(8);
            List<IPlayer> players = new List<IPlayer>();
            Player player1 = new Player("Player 1", PieceColor.Black);
            Player player2 = new Player("Player 2", PieceColor.White);
            players.Add(player1);
            players.Add(player2);
            GameController game = new GameController(players, board);
            game.StartGame();

            Console.Write("Play again? (y/n): ");
            string input = Console.ReadLine();
            playAgain = input.Trim().ToLower() == "y";
        }
    }
}