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
            if (lead != null)
            {
                round.CurrentPlayerIndex = round.Players.IndexOf(lead);
                round.CurrentTrick = new Trick(lead, new List<Move>());
            }
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
                
                if (combination.ContainsDog())
                {
                    HandleDog(round, player);
                    return true;
                }
                
                if (!SatisfiesWish(round, player, cards)) return false;
            
                var move = new Move(player, combination);
                if (!round.CurrentTrick.TryAddMove(move)) return false;
            
                player.RemoveCards(cards);
                if (round.ActiveWish != null && cards.Any(c => c.Rank == round.ActiveWish)) round.ClearWish();
                round.Events.RaiseCardsPlayed(round.Players.IndexOf(player), cardIds);

                if (player.Hand.Count == 0)
                {
                    round.FinishOrder.Add(player);
                    round.Events.RaisePlayerFinished(round.Players.IndexOf(player));
                }
                
                if (combination.ContainsMahjong())
                {
                    round.TransitionTo(RoundStateFactory.CreateWish(round.CurrentPlayerIndex));
                    return true;
                }
            }
            
            AdvanceTurn(round);
            CheckTrickOver(round);
            CheckRoundOver(round);
            
            return true;
        }

        private void AdvanceTurn(Round round)
        {
            int startIndex = round.CurrentPlayerIndex;
            int next = startIndex;

            do
            {
                next = (next + 1) % round.Players.Count;
                if (next == startIndex) break;
            } while (round.Players[next].Hand.Count == 0);

            round.CurrentPlayerIndex = next;
            round.Events.RaiseTurnChanged(round.CurrentPlayerIndex);
        }
        private void CheckTrickOver(Round round)
        {
            Player winner = round.CurrentTrick.DetermineWinner(round.Players);
            if (winner == null) return;

            List<Card> trickCards = round.CurrentTrick.Cards;

            if (round.CurrentTrick.WonWithDragon)
            {
                round.TransitionTo(RoundStateFactory.CreateDragonGift(winner, trickCards));
                return;
            }

            AwardTrick(round, winner, trickCards);
        }

        private void AwardTrick(Round round, Player winner, List<Card> trickCards)
        {
            winner.TricksWon.Add(trickCards);
            round.Events.RaiseTrickWon(
                round.Players.IndexOf(winner),
                trickCards.Select(c => c.ToString()).ToList());

            if (winner.Hand.Count == 0)
            {
                if (!round.FinishOrder.Contains(winner))
                {
                    round.FinishOrder.Add(winner);
                    round.Events.RaisePlayerFinished(round.Players.IndexOf(winner));
                }
            }

            CheckRoundOver(round);
            if (round.IsInState<FinishedState>()) return;

            // Find next active player to lead — skip finished players
            Player nextLeader = FindNextActivePlayer(round, winner);

            if (nextLeader == null)
            {
                round.TransitionToNext();
                return;
            }

            round.CurrentTrick = new Trick(nextLeader, new List<Move>());
            round.CurrentPlayerIndex = round.Players.IndexOf(nextLeader);
            round.Events.RaiseTurnChanged(round.CurrentPlayerIndex);
        }

        private Player FindNextActivePlayer(Round round, Player startingFrom)
        {
            if (startingFrom.Hand.Count > 0) return startingFrom;

            int startIndex = round.Players.IndexOf(startingFrom);
            for (int i = 1; i < round.Players.Count; i++)
            {
                int index = (startIndex + i) % round.Players.Count;
                if (round.Players[index].Hand.Count > 0)
                    return round.Players[index];
            }

            return null;
        }
        
        
        private void CheckRoundOver(Round round)
        {
            var activePlayers = round.Players.Count(p => p.Hand.Count > 0);

            if (activePlayers <= 1)
            {
                var lastPlayer = round.Players.FirstOrDefault(p => p.Hand.Count > 0);
                if (lastPlayer != null && !round.FinishOrder.Contains(lastPlayer))
                {
                    round.FinishOrder.Add(lastPlayer);
                    round.Events.RaisePlayerFinished(round.Players.IndexOf(lastPlayer));
                }
                round.TransitionToNext();
                return;
            }

            if (round.FinishOrder.Count >= 2)
            {
                var first  = round.FinishOrder[0];
                var second = round.FinishOrder[1];

                bool sameTeam = round.Teams.Any(team =>
                    team.Contains(first) && team.Contains(second));

                if (sameTeam)
                {
                    foreach (var player in round.Players)
                    {
                        if (round.FinishOrder.Contains(player)) continue;
                        round.FinishOrder.Add(player);
                        round.Events.RaisePlayerFinished(round.Players.IndexOf(player));
                    }
                    round.TransitionToNext();
                }
            }
        }
        
        private bool SatisfiesWish(Round round, Player player, List<Card> cards)
        {
            if (round.ActiveWish == null) return true;
            Rank wish = round.ActiveWish.Value;
            
            bool playerHasWish = player.Hand.Any(c => c.Rank == wish);
            if (!playerHasWish) return true;
            
            bool moveContainsWish = cards.Any(c => c.Rank == wish);

            return moveContainsWish;
        }
        
        private void HandleDog(Round round, Player player)
        {
            int playerIndex = round.Players.IndexOf(player);
    
            var dogCard = player.Hand.First(c => c.IsDog);
            player.RemoveCards(new List<Card> { dogCard });
            round.Events.RaiseCardsPlayed(playerIndex, new List<string> { dogCard.ToString() });

            Team myTeam = round.Teams.First(team => team.Contains(player));
            Player teammate = myTeam.Player1 == player ? myTeam.Player2 : myTeam.Player1;

            Player nextLeader;
            if (teammate.Hand.Count > 0)
            {
                nextLeader = teammate;
            }
            else
            {
                nextLeader = null;
                for (int i = 1; i < round.Players.Count; i++)
                {
                    int index = (playerIndex + i) % round.Players.Count;
                    if (round.Players[index].Hand.Count > 0)
                    {
                        nextLeader = round.Players[index];
                        break;
                    }
                }
            }

            if (player.Hand.Count == 0 && !round.FinishOrder.Contains(player))
            {
                round.FinishOrder.Add(player);
                round.Events.RaisePlayerFinished(playerIndex);
            }

            CheckRoundOver(round);
            if (round.IsInState<FinishedState>()) return;

            if (nextLeader == null)
            {
                round.TransitionToNext();
                return;
            }

            round.CurrentTrick = new Trick(nextLeader, new List<Move>());
            round.CurrentPlayerIndex = round.Players.IndexOf(nextLeader);
            round.Events.RaiseTurnChanged(round.CurrentPlayerIndex);
        }
        
    }
}