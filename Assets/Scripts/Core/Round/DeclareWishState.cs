using Core.Card;

namespace Core.Round
{
    public class DeclareWishState : IRoundState
    {
        private int _playerIndex;
        private RoundStateFactory _roundStateFactory = new();
        
        public DeclareWishState(int playerIndex)
        {
            _playerIndex = playerIndex;
        }

        public void OnEnter(Round round)
        {
            round.Events.RaiseWishNeeded(_playerIndex);
        }

        public void OnExit(Round round)
        {
        }

        public IRoundState NextState()
        {
            return _roundStateFactory.Create(RoundPhase.Playing);
        }

        public void DeclareWish(Round round, Rank rank)
        {
            round.SetWish(rank);
            round.Events.RaiseWishMade(_playerIndex, (int)rank);
            round.TransitionToNext();
        }
    }
}