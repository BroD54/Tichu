using System.Collections.Generic;
using JetBrains.Annotations;

namespace Core.Game
{
    using Player;
    using Combination;
    
    public class Trick
    {
        private List<Move> _moves;
        public Player Leader { get; }
        [CanBeNull] public Combination  CurrentCombination { get; private set; }
        [CanBeNull] public Player CurrentWinner { get; private set; }

        public Trick(Player player, List<Move> moves)
        {
            _moves = moves;
            Leader = player;
        }

        public bool TryAddMove(Move move)
        {
            if (_moves.Count == 0)
            {
                if (move.Combination == null) return false;
                
                CurrentCombination = move.Combination;
                CurrentWinner = move.Player;
                _moves.Add(move);
                return true;
            }

            if (move.Combination == null)
            {
                _moves.Add(move);
                return true;
            }

            if (!move.Combination.Beats(CurrentCombination)) return false;
            
            CurrentCombination = move.Combination;
            CurrentWinner = move.Player;
            _moves.Add(move);
            
            return true;
        }

        private bool IsFinished()
        {
            if (_moves.Count == 0) return false;

            var consecutivePasses = 0;

            for (var i = _moves.Count - 1; i >= 0; i--)
            {
                if (_moves[i].IsPass) consecutivePasses++;
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