using Othello.Interfaces;
using Othello.Models;
using Othello.Views;

namespace Othello.Controllers
{
    public class GameController
    {
        private IBoard _board;
        private List<IPlayer> _players;
        private IPlayer _currentPlayer;
        private int _currentPlayerIndex;
        private Display _display;

        private List<Position> _directions = [
            new Position(-1, -1), new Position(-1, 0), new Position(-1, 1),
            new Position(0, -1),                       new Position(0, 1),
            new Position(1, -1),  new Position(1, 0),  new Position(1, 1)
        ];

        public Action<Position, IPlayer> onMoveMade;
        //TODO: nggak usah pakai parameter string, panggil method yang di make move (?)

        public GameController(List<IPlayer> players, IBoard board)
        {
            _players = players;
            _board = board;
            _display = new Display(board);
        }

        public void InitializeBoard()
        {
            int midGrid = _board.Size / 2;
            _board.Grid[midGrid - 1, midGrid - 1] = new Piece(PieceColor.White);
            _board.Grid[midGrid - 1, midGrid] = new Piece(PieceColor.Black);
            _board.Grid[midGrid, midGrid - 1] = new Piece(PieceColor.Black);
            _board.Grid[midGrid, midGrid] = new Piece(PieceColor.White);

            // Set up a simple board where Player 1 (Black) has no valid moves.
            // _board.Grid[0, 1] = new Piece(PieceColor.Black);
            // _board.Grid[0, 2] = new Piece(PieceColor.White);
            // _board.Grid[0, 3] = new Piece(PieceColor.White);
            // _board.Grid[0, 4] = new Piece(PieceColor.Black);
        }

        public void StartGame()
        {
            // Count of black and white pieces
            int black;
            int white;

            _currentPlayerIndex = 0;
            _currentPlayer = _players[_currentPlayerIndex];
            bool isGameOver = false;

            InitializeBoard();

            while (!isGameOver)
            {
                CountPieces(out black, out white);
                _display.PrintBoard(black, white, _currentPlayer.Color, GetValidMoves(_currentPlayer.Color));

                if (!HasValidMove(_currentPlayer.Color))
                {
                    HandleNoValidMove();
                    continue;
                }

                Position position = PromptForMove();
                MakeMove(position);

                isGameOver = IsGameOver();
                if (!isGameOver) SwitchPlayer();
            }

            CountPieces(out black, out white);
            _display.PrintBoard(black, white, _currentPlayer.Color, GetValidMoves(_currentPlayer.Color));
        }

        private bool HasValidMove(PieceColor color)
        {
            return GetValidMoves(color).Count > 0;
        }

        private void HandleNoValidMove()
        {
            _display.Print($"{_currentPlayer.Name} has no valid move. {_currentPlayer.Name}'s turn is skipped");
            _display.ReadInput();
            SwitchPlayer();
        }

        private Position PromptForMove()
        {
            // Note: Displayed coordinates are 1-based for user-friendliness.
            // Internally, all logic uses 0-based indices.
            int row, col;
            while (true)
            {
                string inputPos = _display.ReadInput($"{_currentPlayer.Name} ({_display.PieceColorMap(_currentPlayer.Color)}), input position (row, col): ");
                string[] inputRowCol = inputPos.Split(",");

                if (inputRowCol.Count() != 2)
                {
                    _display.Print("Invalid input! Please enter numbers for row and column.");
                    continue;
                }

                string inputRow = inputRowCol[0];
                string inputCol = inputRowCol[1];

                bool isSuccessInputRow = int.TryParse(inputRow.Trim(), out row);
                bool isSuccessInputCol = int.TryParse(inputCol.Trim(), out col);

                if (!isSuccessInputRow || !isSuccessInputCol)
                {
                    _display.Print("Invalid input! Please enter numbers for row and column.");
                    continue;
                }

                // Convert to 0-based index
                row -= 1;
                col -= 1;

                Position position = new Position(row, col);
                if (IsValidMove(position, _currentPlayer.Color))
                {
                    return position;
                }
                _display.Print("Invalid position! Please reenter position.");
            }
        }

        public void AnnounceWinner()
        {
            int black;
            int white;
            CountPieces(out black, out white);

            string winner;
            if (black > white)
                winner = _players.First(p => p.Color == PieceColor.Black).Name;
            else if (white > black)
                winner = _players.First(p => p.Color == PieceColor.White).Name;
            else
                winner = "It's a tie!";
            _display.Print($"Game Over! {(winner == "It's a tie!" ? winner : winner + " Wins!")}");
        }

        public bool IsGameOver()
        {
            if (GetValidMoves(PieceColor.Black).Count == 0 && GetValidMoves(PieceColor.White).Count == 0) return true;
            else return false;
        }

        public void MakeMove(Position position)
        {
            PlacePiece(position, _currentPlayer.Color);
            FlipPiece(position, _currentPlayer.Color);
            onMoveMade?.Invoke(position, _currentPlayer);
        }

        public bool IsValidMove(Position position, PieceColor color)
        {
            if (!_board.IsInBounds(position)) return false;
            if (_board.Grid[position.Row, position.Col].Color != PieceColor.None) return false;
            foreach (Position dir in _directions)
            {
                List<Position> flipsInThisDirection = GetFlipsInSingleDirection(position, dir, color);
                if (flipsInThisDirection.Count > 0) return true;
            }
            return false;
        }

        public List<Position> GetValidMoves(PieceColor color)
        {
            List<Position> validMoves = new List<Position>();

            for (int row = 0; row < _board.Size; row++)
            {
                for (int col = 0; col < _board.Size; col++)
                {
                    if (IsValidMove(new Position(row, col), color)) validMoves.Add(new Position(row, col));
                }
            }
            return validMoves;
        }

        public List<Position> GetFlipsInSingleDirection(Position position, Position direction, PieceColor color)
        {
            List<Position> flipsInThisLine = new List<Position>();
            PieceColor opponentColor = color == PieceColor.Black ? PieceColor.White : PieceColor.Black;

            int row = position.Row + direction.Row;
            int col = position.Col + direction.Col;

            while (_board.IsInBounds(new Position(row, col)))
            {
                if (_board.Grid[row, col].Color == opponentColor)
                {
                    flipsInThisLine.Add(new Position(row, col));
                }
                else if (_board.Grid[row, col].Color == color)
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

        public List<Position> GetFlippedPieces(Position position, PieceColor color)
        {
            List<Position> allFlankedPieces = new List<Position>();
            foreach (Position dir in _directions)
            {
                List<Position> lineFlips = GetFlipsInSingleDirection(position, dir, color);
                allFlankedPieces.AddRange(lineFlips);
            }
            return allFlankedPieces;
        }

        public void PlacePiece(Position position, PieceColor color)
        {
            _board.Grid[position.Row, position.Col] = new Piece(color);
        }

        public void FlipPiece(Position position, PieceColor color)
        {
            List<Position> flankedPieces = GetFlippedPieces(position, color);
            foreach (Position piece in flankedPieces)
            {
                PlacePiece(piece, color);
            }
        }

        public void SwitchPlayer()
        {
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
            _currentPlayer = _players[_currentPlayerIndex];
        }

        public void CountPieces(out int black, out int white)
        {
            black = 0;
            white = 0;
            for (int row = 0; row < _board.Size; row++)
            {
                for (int col = 0; col < _board.Size; col++)
                {
                    if (_board.Grid[row, col].Color == PieceColor.Black) black++;
                    else if (_board.Grid[row, col].Color == PieceColor.White) white++;
                }
            }
        }
    }
}