namespace Othello
{
    public class Player : IPlayer
    {
        public string Name { get; }
        public PieceColor Color { get; }
        public Player(string name, PieceColor color)
        {
            Name = name;
            Color = color;
        }
    }
}