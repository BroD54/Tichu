namespace Core.Commands
{
    using Scoring;
    using Events;

    public class ScoreRoundCommand : Command
    {
        private readonly Round.Round _round;
        private readonly ScoringService _scoringService;
        public RoundResult Result { get; private set; }

        public ScoreRoundCommand(Round.Round round, ScoringService scoringService)
        {
            _round          = round;
            _scoringService = scoringService;
        }

        public override void Execute()
        {
            Result = _scoringService.Calculate(
                _round.FinishOrder,
                _round.Teams,
                _round.TichuCalls);
            
            foreach (var (team, points) in Result.PointsEarned)
                team.UpdateScore(points);

            //notify observers 
            
            foreach (var team in _round.Teams)
                
            {
                if (team.Score >= 1000)
                {
                    TichuEventBus.RaiseGameWon(team.Id);
                    break;
                }
            }
        }
    }
}