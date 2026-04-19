using Core.Events;
using Core.Game;

namespace Core.Round
{
    public class DealingFirstCardsState : IRoundState
    {

        public void OnEnter(Round round)
        {
            round.Deck.Deal(round.Players, Deck.FirstDealCount);
            TichuEventBus.RaiseFirstCardsDealt(round.Players);  // ← add this
            round.TransitionToNext();
        }

        public void OnExit(Round round)
        {
            
        }

        public IRoundState NextState()
        {
            return RoundStateFactory.Create(RoundPhase.GrandTichuCalls);
        }
    }
}