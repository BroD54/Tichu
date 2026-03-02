using System.Collections.Generic;
using System.Diagnostics;
using JetBrains.Annotations;

namespace Core.Game
{
    using Player;
    using Combination;
    public class Round
    {
        public List<Player> Players { get; }
        public List<Team>  Teams { get; }
        public Deck Deck { get; }
        public int CurrentPlayerIndex { get; private set; }
        public RoundPhase Phase { get; private set; }
        [CanBeNull] public Trick CurrentTrick { get; private set; }
        public List<Player> FinishOrder { get; }
        public Dictionary<Player, TichuCall> TichuCalls { get; }

        public Round(List<Player> players, List<Team> teams, Deck deck)
        {
            Players = players;
            Teams = teams;
            Deck = deck;
            
            FinishOrder = new List<Player>();
            TichuCalls = new Dictionary<Player, TichuCall>();

            CurrentTrick = null;
            
            SetPhase(RoundPhase.DealingFirstCards);
        }

        private void SetPhase(RoundPhase next)
        {
            Phase = next;
            switch (next)
            {
                case RoundPhase.DealingFirstCards:
                    DealFirstEightCards();
                    break;
                case RoundPhase.GrandTichuCalls:
                    // 
                    break;
                case RoundPhase.DealingRemainingCards:
                    DealRemainingCards();
                    break;
                case RoundPhase.CardExchange:
                    //
                    break;
                case RoundPhase.Playing:
                    Player lead = Players.Find(player => player.HasMahjong);
                    CurrentPlayerIndex = Players.IndexOf(lead);
                    StartNewTrick(Players[CurrentPlayerIndex]);
                    break;
                case RoundPhase.Scoring:
                    //
                    break;
            }
        }

        private void DealFirstEightCards()
        {
            Deck.Deal(Players, Deck.FirstDealCount);
        }
        
        private void DealRemainingCards()
        {
            Deck.Deal(Players, Deck.SecondDealCount);
        }

        
        public bool ApplyMove(Move move)
        {
            if (move.Player != Players[CurrentPlayerIndex]) return false;

            var isLegalMove = CurrentTrick!.TryAddMove(move);
            if  (!isLegalMove) return false;

            AdvanceTurn();
            
            return true;
        }

        private void AdvanceTurn()
        {
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
        }

        public void StartNewTrick(Player leader)
        {
            CurrentTrick = new Trick(leader, new List<Move>());
            CurrentPlayerIndex = Players.IndexOf(leader);
        }

    }
}