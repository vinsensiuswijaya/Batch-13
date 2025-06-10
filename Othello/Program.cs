using Othello.Interfaces;
using Othello.Models;
using Othello.Controllers;

namespace Othello
{
    class Program
    {
        static void Main()
        {
            bool playAgain = true;
            while (playAgain)
            {
                Board board = new Board(8);

                List<IPlayer> players =
                [
                    new Player("Player 1", PieceColor.Black),
                    new Player("Player 2", PieceColor.White),
                ];

                GameController game = new GameController(players, board);

                game.onMoveMade += (position, player) => Console.WriteLine($"{player.Name} made a move on ({position.Row + 1}, {position.Col + 1})"); // 1-based row position

                game.StartGame();
                game.AnnounceWinner();

                Console.Write("Play again? (y/n): ");
                string input = Console.ReadLine();
                playAgain = input.Trim().ToLower() == "y";
            }
        }
    }
}