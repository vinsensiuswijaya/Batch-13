namespace Othello
{
    public class GameController
    {
        public IBoard Board { get; set; }
        public List<IPlayer> Players { get; set; }
        public IPlayer CurrentPlayer { get; set; }
        private int _currentPlayerIndex;

        private List<Position> _directions = [
            new Position(-1, -1), new Position(-1, 0), new Position(-1, 1),
            new Position(0, -1),                       new Position(0, 1),
            new Position(1, -1),  new Position(1, 0),  new Position(1, 1)
        ];

        public Action<string> OnMoveMade { get; set; }

        public GameController(List<IPlayer> players, IBoard board)
        {
            Players = players;
            Board = board;
        }

        public void StartGame()
        {
            int black, white; // Count of black and white pieces
            _currentPlayerIndex = 0;
            CurrentPlayer = Players[_currentPlayerIndex];
            bool isGameOver = false;
            
            Board.Initialize();

            while (!isGameOver)
            {
                CountPieces(out black, out white);
                Display(black, white);

                if (!HasValidMove(CurrentPlayer.Color))
                {
                    HandleNoValidMove();
                    continue;
                }

                Position pos = PromptForMove();
                MakeMove(pos);

                isGameOver = IsGameOver();
                if (!isGameOver) SwitchPlayer();
            }

            CountPieces(out black, out white);
            Display(black, white);
            AnnounceWinner(black, white);
        }

        private bool HasValidMove(PieceColor color)
        {
            return GetValidMoves(color).Count > 0;
        }

        private void HandleNoValidMove()
        {
            Console.WriteLine($"{CurrentPlayer.Name} has no valid move. {CurrentPlayer.Name}'s turn is skipped");
            Console.ReadKey();
            SwitchPlayer();
        }

        private Position PromptForMove()
        {
            // Note: Displayed coordinates are 1-based for user-friendliness.
            // Internally, all logic uses 0-based indices.
            int row, col;
            while (true)
            {
                Console.Write($"{CurrentPlayer.Name} ({PieceColorMap(CurrentPlayer.Color)}), input position (row, col): ");
                string inputPos = Console.ReadLine();
                string[] inputRowCol = inputPos.Split(",");

                if (inputRowCol.Count() != 2)
                {
                    Console.WriteLine("Invalid input! Please enter numbers for row and column.");
                    continue;
                }

                string inputRow = inputRowCol[0];
                string inputCol = inputRowCol[1];

                bool isSuccessInputRow = int.TryParse(inputRow.Trim(), out row);
                bool isSuccessInputCol = int.TryParse(inputCol.Trim(), out col);

                if (!isSuccessInputRow || !isSuccessInputCol)
                {
                    Console.WriteLine("Invalid input! Please enter numbers for row and column.");
                    continue;
                }

                // Convert to 0-based index
                row -= 1;
                col -= 1;

                Position pos = new Position(row, col);
                if (IsValidMove(pos, CurrentPlayer.Color))
                {
                    return pos;
                }
                Console.WriteLine("Invalid position! Please reenter position.");
            }
        }

        private void AnnounceWinner(int black, int white)
        {
            string winner;
            if (black > white)
                winner = Players.First(p => p.Color == PieceColor.Black).Name;
            else if (white > black)
                winner = Players.First(p => p.Color == PieceColor.White).Name;
            else
                winner = "It's a tie!";
            Console.WriteLine($"Game Over! {(winner == "It's a tie!" ? winner : winner + " Wins!")}");
        }

        public bool IsGameOver()
        {
            if (GetValidMoves(PieceColor.Black).Count == 0 && GetValidMoves(PieceColor.White).Count == 0) return true;
            else return false;
        }

        public void MakeMove(Position pos)
        {
            PlacePiece(pos, CurrentPlayer.Color);
            FlipPiece(pos, CurrentPlayer.Color);
            OnMoveMade?.Invoke($"{CurrentPlayer.Name} made a move on ({pos.Row + 1}, {pos.Col + 1})"); // 1-based row display
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
            PieceColor opponentColor = color == PieceColor.Black ? PieceColor.White : PieceColor.Black;

            int row = pos.Row + direction.Row;
            int col = pos.Col + direction.Col;

            while (Board.IsInBounds(new Position(row, col)))
            {
                if (Board.Grid[row, col].Color == opponentColor)
                {
                    flipsInThisLine.Add(new Position(row, col));
                }
                else if (Board.Grid[row, col].Color == color)
                {
                    // Valid line found if there were opponent pieces in between
                    return flipsInThisLine.Count > 0 ? flipsInThisLine : new List<Position>();
                }
                else // Empty square
                {
                    return new List<Position>(); // Invalid line, no flips in this direction
                }
                row += direction.Row;
                col += direction.Col;
            }
            // Reached the edge of the board without finding a capping player's piece
            return new List<Position>();
        }

        public List<Position> GetFlippedPieces(Position pos, PieceColor color)
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
            List<Position> flankedPieces = GetFlippedPieces(pos, color);
            foreach (Position piece in flankedPieces)
            {
                PlacePiece(piece, color);
            }
        }

        public void SwitchPlayer()
        {
            _currentPlayerIndex = (_currentPlayerIndex + 1) % Players.Count;
            CurrentPlayer = Players[_currentPlayerIndex];
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
            // Console.Clear();
            Console.WriteLine("\n=========================================");
            Console.WriteLine("================ OTHELLO ================");
            Console.WriteLine("=========================================");
            Console.WriteLine($"  Count ({PieceColorMap(PieceColor.Black)}): {blackCount}, ({PieceColorMap(PieceColor.White)}): {whiteCount}");
            PrintBoard();
        }

        private void PrintBoard()
        {
            // Note: Board display uses 1-based coordinates for user-friendliness.
            // Internally, all logic and data structures use 0-based indices.
            // When displaying, add 1 to row and column numbers.
            List<Position> availableMoves = GetValidMoves(CurrentPlayer.Color);
            
            PrintSeparatorLine(Board.Size);
            PrintHeaderRow(Board.Size);
            PrintSeparatorLine(Board.Size);

            for (int row = 0; row < Board.Size; row++)
            {
                Console.Write($"  | {row + 1} |"); // 1-based row display
                for (int col = 0; col < Board.Size; col++)
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
                        Console.Write($" {PieceColorMap(Board.Grid[row, col].Color)} |");
                    }
                }
                Console.WriteLine();
                PrintSeparatorLine(Board.Size);
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

        private char PieceColorMap(PieceColor color)
        {
            return color switch
            {
                PieceColor.Black => 'X',
                PieceColor.None => ' ',
                PieceColor.White => 'O'
            };
        }
    }
}