namespace Core.Round
{
    using Player;
    using Game;
    
    public class GrandTichuCallsState : IRoundState
    {
        private int _pendingResponses;
        public void OnEnter(Round round)
        {
            _pendingResponses = round.Players.Count;
            foreach (var player in round.Players)
                round.FireGrandTichuDecisionNeeded(player);
        }

        public void OnExit(Round round)
        {
        }

        public IRoundState NextState()
        {
            return RoundStateFactory.Create(RoundPhase.DealingRemainingCards);
        }
        
        public bool SubmitDecision(Round round, Player player, bool calledGrandTichu)
        {
            if (round.TichuCalls.ContainsKey(player)) return false;

            round.TichuCalls[player] = calledGrandTichu ? TichuCall.GrandTichu : TichuCall.None;
            if (calledGrandTichu) player.DeclareGrandTichu();

            _pendingResponses--;
            if (_pendingResponses == 0)
                round.TransitionToNext();

            return true;
        }
    }
}