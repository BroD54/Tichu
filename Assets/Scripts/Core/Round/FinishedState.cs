namespace Core.Round
{
    public class FinishedState : IRoundState
    {
        public void OnEnter(Round round)
        {
        }

        public void OnExit(Round round)
        {
        }

        public IRoundState NextState()
        {
            throw new System.InvalidOperationException(
                "Round is finished, no next step exists");        
        }
    }
}