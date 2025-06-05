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
                List<IPlayer> players = new List<IPlayer>();
                Player player1 = new Player("Player 1", PieceColor.Black);
                Player player2 = new Player("Player 2", PieceColor.White);
                players.Add(player1);
                players.Add(player2);
                GameController game = new GameController(players, board);
                game.OnMoveMade = msg => Console.WriteLine(msg);
                game.StartGame();

                Console.Write("Play again? (y/n): ");
                string input = Console.ReadLine();
                playAgain = input.Trim().ToLower() == "y";
            }
        }
    }
}