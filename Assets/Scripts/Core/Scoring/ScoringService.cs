using System.Collections.Generic;
using System.Linq;

namespace Core.Scoring
{
    using Game;
    using Player;
    using Card;

    public class ScoringService
    {
        private static int CardPoints(Card card)
        {
            return card.Type switch
            {
                CardType.Dragon  =>  25,
                CardType.Phoenix => -25,
                CardType.Mahjong =>   0,
                CardType.Dog     =>   0,
                CardType.Standard => card.Rank switch
                {
                    Rank.Five => 5,
                    Rank.Ten  => 10,
                    Rank.King => 10,
                    _         => 0
                },
                _ => 0
            };
        }

        public RoundResult Calculate(
            List<Player> finishOrder,
            List<Team> teams,
            Dictionary<Player, TichuCall> tichuCalls)
        {

            Team doubleVictoryTeam = null;
            foreach (var team in teams)
            {
                if (finishOrder.Count >= 2 &&
                    finishOrder[0] == team.Player1 && finishOrder[1] == team.Player2 ||
                    finishOrder[0] == team.Player2 && finishOrder[1] == team.Player1)
                {
                    doubleVictoryTeam = team;
                    break;
                }
            }

            var pointsEarned = new Dictionary<Team, int>();
            foreach (var team in teams)
                pointsEarned[team] = 0;

            if (doubleVictoryTeam != null)
            {
                pointsEarned[doubleVictoryTeam] = 200;
            }
            else
            {
                var lastOut = finishOrder.Last();
                var firstOut = finishOrder.First();
                var lastOutTeam = TeamOf(lastOut, teams);
                var firstOutTeam = TeamOf(firstOut, teams);

                foreach (var team in teams)
                {
                    int trickPoints = 0;

                    foreach (var player in new[] { team.Player1, team.Player2 })
                    {
                        if (player == lastOut)
                        {
                            var opponentFirstOut = firstOutTeam == team
                                ? OpposingTeam(team, teams).Player1
                                : firstOut;

                            var opponentTeam = TeamOf(opponentFirstOut, teams);
                            pointsEarned[opponentTeam] += player.TricksWon
                                .SelectMany(t => t)
                                .Sum(CardPoints);

                            pointsEarned[opponentTeam] += player.Hand.Sum(CardPoints);
                        }
                        else
                        {
                            trickPoints += player.TricksWon
                                .SelectMany(t => t)
                                .Sum(CardPoints);
                        }
                    }

                    pointsEarned[team] += trickPoints;
                }

                foreach (var (player, call) in tichuCalls)
                {
                    if (call == TichuCall.None) continue;

                    bool succeeded = finishOrder.Count > 0 && finishOrder[0] == player;
                    int stake = call == TichuCall.GrandTichu ? 200 : 100;
                    int delta = succeeded ? stake : -stake;

                    TeamOf(player, teams).UpdateScore(0);
                    pointsEarned[TeamOf(player, teams)] += delta;
                }
            }

            return new RoundResult(
                finishOrder.AsReadOnly(),
                pointsEarned,
                doubleVictoryTeam != null,
                doubleVictoryTeam);
        }

        private static Team TeamOf(Player player, List<Team> teams)
            => teams.First(t => t.Player1 == player || t.Player2 == player);

        private static Team OpposingTeam(Team team, List<Team> teams)
            => teams.First(t => t != team);
    }
}