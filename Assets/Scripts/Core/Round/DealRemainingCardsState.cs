using Core.Game;

namespace Core.Round
{
    public class DealRemainingCardsState : IRoundState
    {
        public void OnEnter(Round round)
        {
            round.Deck.Deal(round.Players, Deck.SecondDealCount);
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