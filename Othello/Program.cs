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

                game.StartGame();

                Console.Write("Play again? (y/n): ");
                string input = Console.ReadLine();
                playAgain = input.Trim().ToLower() == "y";
            }
        }
    }
}