using System.Linq;
using Core.Game;

namespace Core.Round
{
    public class DealRemainingCardsState : IRoundState
    {
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
            return RoundStateFactory.Create(RoundPhase.CardExchange);
        }
    }
}