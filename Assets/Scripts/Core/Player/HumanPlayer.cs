namespace Core.Player
{
    using Game;
    
    public class HumanPlayer : Player
    {
        private Move _pendingMove;

        public HumanPlayer(string name) : base(name)
        {
            
        }

        public void SetMove(Move move)
        {
            _pendingMove = move;
        }

        public override Move MakeMove(Round currentRound)
        {
            return _pendingMove;
        }
    }
}