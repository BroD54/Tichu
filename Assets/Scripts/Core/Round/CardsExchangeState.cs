using System.Collections.Generic;
using System.Linq;

namespace Core.Round
{
    using Player;
    using Card;
    public class CardsExchangeState : IRoundState
    {
        private readonly Dictionary<Player, List<Card>> _pendingExchanges = new();
        private RoundStateFactory _roundStateFactory = new();
        
        public void OnEnter(Round round)
        {
            _pendingExchanges.Clear();
            round.Events.RaiseExchangePhaseStarted();
        }

        public void OnExit(Round round)
        {
            for (var giverIndex = 0; giverIndex < round.Players.Count; giverIndex++)
            {
                Player giver =  round.Players[giverIndex];
                if (!_pendingExchanges.TryGetValue(giver, out var cards)) continue;

                for (var i = 0; i < cards.Count; i++)
                {
                    var receiverIndex = (giverIndex + i + 1) % round.Players.Count;
                    Player receiver = round.Players[receiverIndex];
                    
                    giver.RemoveCards(new List<Card>{cards[i]});
                    receiver.ReceiveCards(new List<Card>{cards[i]});
                }
            }
        }

        public IRoundState NextState()
        {
            return _roundStateFactory.Create(RoundPhase.Playing);
        }

        public bool SubmitExchange(Round round, Player player, List<string> cardIds)
        {
            if (_pendingExchanges.ContainsKey(player)) return false;

            var cards = cardIds
                .Select(id => player.Hand.FirstOrDefault(card => card.ToString() == id))
                .Where(card => card != null)
                .ToList();
            
            if (cards.Count != 3) return false;
            
            _pendingExchanges[player] = cards;
            
            round.Events.RaiseCardsExchanged(
                round.Players.IndexOf(player),
                cardIds
            );
            
            if (_pendingExchanges.Count == round.Players.Count)
                round.TransitionToNext();
            
            return true;
        }
    }
}