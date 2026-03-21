namespace Core.Round
{
    public class DealRemainingCardsState : IRoundState
    {
        public void OnEnter(Round round)
        {
            throw new System.NotImplementedException();
        }

        public void OnExit(Round round)
        {
            throw new System.NotImplementedException();
        }

        public IRoundState NextState()
        {  
            return RoundStateFactory.Create(RoundPhase.CardExchange);
        }
    }
}