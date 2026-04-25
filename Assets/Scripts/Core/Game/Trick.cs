using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Core.Game
{
    using Player;
    using Combination;
    using Card;
    
    public class Trick
    {
        public List<Move> Moves { get; }
        public Player Leader { get; }
        [CanBeNull] public Combination  CurrentCombination { get; private set; }
        [CanBeNull] public Player CurrentWinner { get; private set; }
        
        public List<Card> Cards =>
            Moves.Where(move => !move.IsPass)
                .SelectMany(move => move.Combination.Cards)
                .ToList();

        public bool WonWithDragon =>
            CurrentWinner != null &&
            Moves.Any(m => m.Combination != null && 
                           m.Combination.Cards.Any(c => c.IsDragon) &&
                           m.Player == CurrentWinner);
        
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
            
            if (move.Combination is Single phoenixSingle && phoenixSingle.Cards[0].IsPhoenix)
                phoenixSingle.CoverWith(CurrentCombination.Strength);
            
            CurrentCombination = move.Combination;
            CurrentWinner = move.Player;
            Moves.Add(move);
            
            return true;
        }

        public bool IsFinished(List<Player> activePlayers)
        {
            if (Moves.Count == 0) return false;

            var consecutivePasses = 0;
            for (var i = Moves.Count - 1; i >= 0; i--)
            {
                if (Moves[i].IsPass) consecutivePasses++;
                else break;
            }

            int passesNeeded = activePlayers.Count(p => p.Hand.Count > 0) - 1;
            passesNeeded = Math.Max(passesNeeded, 1);
            return consecutivePasses >= passesNeeded;
        }

        [CanBeNull]
        public Player DetermineWinner(List<Player> activePlayers)
        {
            return !IsFinished(activePlayers) ? null : CurrentWinner;
        }
    }
}