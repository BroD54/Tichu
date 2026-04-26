using System.Linq;
using Core.Game;

namespace Core.Round
{
    public class DealRemainingCardsState : IRoundState
    {
        private RoundStateFactory _roundStateFactory = new();

        public void OnEnter(Round round)
        {
            round.Deck.Deal(round.Players, Deck.SecondDealCount);
            round.Events.RaiseAllCardsDealt(round.Players.Select(player => player.Name).ToList());
            round.TransitionToNext();
        }

        public void OnExit(Round round)
        {
        }

        public IRoundState NextState()
        {  
            return _roundStateFactory.Create(RoundPhase.CardExchange);
        }
    }
}