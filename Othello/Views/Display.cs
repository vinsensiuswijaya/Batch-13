using Othello.Interfaces;

namespace Othello.Views
{
    public class Display
    {
        private IBoard _board;
        public Display(IBoard board)
        {
            _board = board;
        }
        public char PieceColorMap(PieceColor color)
        {
            return color switch
            {
                PieceColor.Black => 'X',
                PieceColor.White => 'O',
                PieceColor.None => ' '
            };
        }
        public void Print(string message)
        {
            Console.WriteLine(message);
        }
        public void Clear()
        {
            Console.Clear();
        }
        public string ReadInput(string prompt)
        {
            Console.Write(prompt);
            return Console.ReadLine();
        }
        public void ReadInput()
        {
            Console.ReadKey();
        }
        public void PrintBoard(int blackCount, int whiteCount, PieceColor currentPlayerColor, List<Position> availableMoves)
        {
            // Console.Clear();
            Console.WriteLine("\n=========================================");
            Console.WriteLine("================ OTHELLO ================");
            Console.WriteLine("=========================================");
            Console.WriteLine($"  Count ({PieceColorMap(PieceColor.Black)}): {blackCount}, ({PieceColorMap(PieceColor.White)}): {whiteCount}");

            // Note: Board display uses 1-based coordinates for user-friendliness.
            // Internally, all logic and data structures use 0-based indices.
            // When displaying, add 1 to row and column numbers.

            PrintSeparatorLine(_board.Size);
            PrintHeaderRow(_board.Size);
            PrintSeparatorLine(_board.Size);

            for (int row = 0; row < _board.Size; row++)
            {
                Console.Write($"  | {row + 1} |"); // 1-based row display
                for (int col = 0; col < _board.Size; col++)
                {
                    if (availableMoves.Contains(new Position(row, col)))
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Console.Write(" * ");
                        Console.ResetColor();
                        Console.Write("|");
                    }
                    else
                    {
                        Console.Write($" {PieceColorMap(_board.Grid[row, col].Color)} |");
                    }
                }
                Console.WriteLine();
                PrintSeparatorLine(_board.Size);
            }
            Console.ResetColor();
        }
        private void PrintSeparatorLine(int boardSize)
        {
            Console.Write("  +---+");
            for (int col = 0; col < boardSize; col++) Console.Write("---+");
            Console.WriteLine();
        }
        private void PrintHeaderRow(int boardSize)
        {
            Console.Write("  |   |");
            for (int col = 0; col < boardSize; col++) Console.Write($" {col + 1} |"); // 1-based row display
            Console.WriteLine();
        }     
    }
}