using Core.Scoring;

namespace Core.Round
{
    public class ScoringState : IRoundState
    {
        
        private readonly ScoringService _scoringService = new ScoringService();

        public void OnEnter(Round round)
        {
            var result = _scoringService.Calculate(
                round.FinishOrder,
                round.Teams,
                round.TichuCalls);

            int team1Round = 0, team2Round = 0;

            foreach (var (team, points) in result.PointsEarned)
            {
                team.UpdateScore(points);
                if (team == round.Teams[0]) team1Round = points;
                if (team == round.Teams[1]) team2Round = points;
            }

            round.Events.RaiseRoundScored(
                team1Round,
                team2Round,
                round.Teams[0].Score,
                round.Teams[1].Score);

            foreach (var team in round.Teams)
            {
                if (team.Score >= 1000)
                {
                    round.Events.RaiseGameWon(round.Teams.IndexOf(team));
                    round.TransitionToNext();
                    return;
                }
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
