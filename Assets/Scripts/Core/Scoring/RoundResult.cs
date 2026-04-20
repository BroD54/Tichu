using System.Collections.Generic;

namespace Core.Scoring
{
    using Player;
    using Game;

    public class RoundResult
    {
        public IReadOnlyList<Player> FinishOrder { get; }
        public IReadOnlyDictionary<Team, int> PointsEarned { get; }
        public bool IsDoubleVictory { get; }
        public Team DoubleVictoryTeam { get; }

        public RoundResult(
            IReadOnlyList<Player> finishOrder,
            IReadOnlyDictionary<Team, int> pointsEarned,
            bool isDoubleVictory,
            Team doubleVictoryTeam)
        {
            FinishOrder      = finishOrder;
            PointsEarned     = pointsEarned;
            IsDoubleVictory  = isDoubleVictory;
            DoubleVictoryTeam = doubleVictoryTeam;
        }
    }
}