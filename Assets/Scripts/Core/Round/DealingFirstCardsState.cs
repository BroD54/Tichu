using System.Linq;
using Core.Events;
using Core.Game;

namespace Core.Round
{
    public class DealingFirstCardsState : IRoundState
    {

        public void OnEnter(Round round)
        {
            round.Deck.Deal(round.Players, Deck.FirstDealCount);
            round.Events.RaiseFirstCardsDealt(round.Players.Select(player => player.Name).ToList());
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