using Core.Scoring;

namespace Core.Commands
{
    using Player;
    using Card;
    using Game;
    using Round;
    
    public class CommandFactory
    {
        public ICommand CreateAwardTrickCommand(Round round, Player recipient, Trick trick)  
            => new AwardTrickCommand(round, recipient, trick);
        
        public ICommand CreateAdvanceTurnCommand(Round round)
           => new AdvanceTurnCommand(round);
        
        public ICommand CreateDeclareTichuCommand(Round round, Player player, TichuCall call) 
            => new DeclareTichuCommand(round, player, call); 

        public ICommand CreateExchangeCardsCommand(Player from, Player to, Card card)
            => new ExchangeCardsCommand(from, to, card);
        
        public ICommand CreatePlayCardsCommand(Round round, Move move) 
            => new PlayCardsCommand(round, move);
        
        public ICommand CreateScoreRoundCommand(Round round, ScoringService scoringService) 
            => new ScoreRoundCommand(round, scoringService);
    
    }
}