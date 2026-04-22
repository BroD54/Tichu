using System.Collections.Generic;
using System.Linq;
using Core.Combination;


namespace Core.Round
{
    using Player;
    using Game;
    using Card;
    
    public class PlayingState : IRoundState
    {
        CombinationFactory _combinationFactory = new  CombinationFactory();
        
        public void OnEnter(Round round)
        {
            Player lead = round.Players.Find(player => player.HasMahjong);
            round.CurrentPlayerIndex = round.Players.IndexOf(lead);
            round.CurrentTrick = new Trick(lead, new List<Move>());
            round.Events.RaiseTurnChanged(round.CurrentPlayerIndex);
        }

        public void OnExit(Round round)
        {
        }

        public IRoundState NextState()
        {
            return RoundStateFactory.Create(RoundPhase.Scoring);
        }

        public bool SubmitMove(Round round, Player player, List<string> cardIds)
        {
            if (player != round.Players[round.CurrentPlayerIndex]) return false;

            var hasPassed = cardIds == null || cardIds.Count == 0;
            if (hasPassed)
            {
                var passMove = new Move(player, null);
                if (!round.CurrentTrick.TryAddMove(passMove)) return false;
                
                round.Events.RaisePlayerPassed(round.Players.IndexOf(player));
            }
            else
            {
                List<Card> cards = cardIds
                    .Select(id => player.Hand.FirstOrDefault(card => card.ToString() == id))
                    .Where(card => card != null)
                    .ToList();
            
                if(cards.Count != cardIds.Count) return false;
            
                var combination = _combinationFactory.Create(cards);
                if (combination == null) return false;
            
                var move = new Move(player, combination);
                if (!round.CurrentTrick.TryAddMove(move)) return false;
            
                player.RemoveCards(cards);
                round.Events.RaiseCardsPlayed(round.Players.IndexOf(player), cardIds);

                if (player.Hand.Count == 0)
                {
                    round.FinishOrder.Add(player);
                    round.Events.RaisePlayerFinished(round.Players.IndexOf(player));
                }
            }

            AdvanceTurn(round);
            CheckTrickOver(round);
            CheckRoundOver(round);
            
            return true;
        }

        private void AdvanceTurn(Round round)
        {
            round.CurrentPlayerIndex = (round.CurrentPlayerIndex + 1) % round.Players.Count;
            round.Events.RaiseTurnChanged(round.CurrentPlayerIndex);
        }

        private void CheckTrickOver(Round round)
        {
            Player winner = round.CurrentTrick.DetermineWinner();
            if (winner == null) return;
            
            List<Card> trickCards = round.CurrentTrick.Cards;
            winner.TricksWon.Add(trickCards);
            
            round.Events.RaiseTrickWon(
                round.Players.IndexOf(winner), 
                trickCards.Select(card => card.ToString()).ToList()
            );
            
            round.CurrentTrick = new Trick(winner, new List<Move>());
            round.CurrentPlayerIndex = round.Players.IndexOf(winner);
            round.Events.RaiseTurnChanged(round.CurrentPlayerIndex);
        }
        
        
        private void CheckRoundOver(Round round)
        {
            var activePlayers = round.Players.Count(p => p.Hand.Count > 0);
            if (activePlayers > 1) return;

            round.TransitionToNext();
        }
    }
}