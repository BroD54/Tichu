namespace Core.Game
{
    using Player;
    
    public class Team
    {
        public int Id { get; }
        public Player Player1 { get; }
        public Player Player2 { get; }
        public int Score { get; set; }

        public Team(int id, Player player1, Player player2)
        {
            Id = id;
            Player1 = player1;
            Player2 = player2;
            Score = 0;
        }

        public void UpdateScore(int change)
        {
            Score += change;
        }
    }
}