using System.Collections.Generic;
using JetBrains.Annotations;

namespace Core.Game
{
    using Player;
    using Combination;
    
    public class Trick
    {
        public List<Move> Moves { get; }
        public Player Leader { get; }
        [CanBeNull] public Combination  CurrentCombination { get; private set; }
        [CanBeNull] public Player CurrentWinner { get; private set; }

        public Trick(Player player, List<Move> moves)
        {
            Moves = moves;
            Leader = player;
        }

        public bool TryAddMove(Move move)
        {
            if (Moves.Count == 0)
            {
                if (move.Combination == null) return false;
                
                CurrentCombination = move.Combination;
                CurrentWinner = move.Player;
                Moves.Add(move);
                return true;
            }

            if (move.Combination == null)
            {
                Moves.Add(move);
                return true;
            }

            if (!move.Combination.Beats(CurrentCombination)) return false;
            
            CurrentCombination = move.Combination;
            CurrentWinner = move.Player;
            Moves.Add(move);
            
            return true;
        }

        private bool IsFinished()
        {
            if (Moves.Count == 0) return false;

            var consecutivePasses = 0;

            for (var i = Moves.Count - 1; i >= 0; i--)
            {
                if (Moves[i].IsPass) consecutivePasses++;
                else break;
            }

            const int passesToLeader = 3;
            return consecutivePasses >= passesToLeader;
        }

        [CanBeNull]
        public Player DetermineWinner()
        {
            return !IsFinished() ? null : CurrentWinner;
        }
    }
}