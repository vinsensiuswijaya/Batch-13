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
            int black = 0, white = 0; // Count of black and white pieces
            Board.Initialize();
            _currentPlayerIndex = 0;
            CurrentPlayer = Players[_currentPlayerIndex];
            bool isGameOver = false;

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
            int row, col;
            while (true)
            {
                Console.Write($"{CurrentPlayer.Name} ({PieceColorMap(CurrentPlayer.Color)}), Input row: ");
                string inputRow = Console.ReadLine();
                Console.Write($"{CurrentPlayer.Name} ({PieceColorMap(CurrentPlayer.Color)}), Input column: ");
                string inputCol = Console.ReadLine();

                bool isSuccessInputRow = int.TryParse(inputRow.Trim(), out row);
                bool isSuccessInputCol = int.TryParse(inputCol.Trim(), out col);

                if (!isSuccessInputRow || !isSuccessInputCol)
                {
                    Console.WriteLine("Invalid input! Please enter numbers for row and column.");
                    continue;
                }

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
            OnMoveMade?.Invoke($"{CurrentPlayer.Name} made a move on ({pos.Row}, {pos.Col})");
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
            Console.WriteLine("=========================");
            Console.WriteLine("=========OTHELLO=========");
            Console.WriteLine("=========================\n");
            Console.WriteLine($"Count (X): {blackCount}, (O): {whiteCount}");
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
                PieceColor.Black => 'X',
                PieceColor.None => '.',
                PieceColor.White => 'O'
            };
        }
    }
}