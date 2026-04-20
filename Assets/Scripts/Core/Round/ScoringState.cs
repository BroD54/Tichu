using Core.Commands;
using Core.Scoring;

namespace Core.Round
{
    public class ScoringState : IRoundState
    {
        
        private readonly ScoringService _scoringService = new ScoringService();

        public void OnEnter(Round round)
        {
            var command = new ScoreRoundCommand(round, _scoringService);
            command.Execute();

            round.TransitionToNext();
        }

        public void OnExit(Round round)
        {
            throw new System.NotImplementedException();
        }

        public IRoundState NextState()
        {
            return RoundStateFactory.Create(RoundPhase.Finished);
        }
    }
}
