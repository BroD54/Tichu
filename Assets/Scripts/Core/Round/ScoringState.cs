using Core.Commands;
using Core.Scoring;

namespace Core.Round
{
    public class ScoringState : IRoundState
    {
        
        private readonly ScoringService _scoringService = new ScoringService();

        public void OnEnter(Round round)
        {
            RoundResult result = _scoringService.Calculate(
                round.FinishOrder,
                round.Teams,
                round.TichuCalls
            );
            
            foreach (var (team, points) in result.PointsEarned)
                team.UpdateScore(points);
            
            foreach (var team in round.Teams)
            {
                if (team.Score < 1000) continue;
                
                round.Events.RaiseGameWon(team.Id);
                break;
            }
            round.TransitionToNext();
        }

        public void OnExit(Round round)
        {
        }

        public IRoundState NextState()
        {
            return RoundStateFactory.Create(RoundPhase.Finished);
        }
    }
}
