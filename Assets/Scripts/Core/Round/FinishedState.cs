using System;

namespace Core.Round
{
    public class FinishedState : IRoundState
    {
        public void OnEnter(Round round)
        {
        }

        public void OnExit(Round round) { }

        public IRoundState NextState() =>
            throw new InvalidOperationException("Round is finished");
        }
    
}