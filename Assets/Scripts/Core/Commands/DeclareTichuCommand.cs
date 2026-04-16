namespace Core.Commands
{
    using Player;
    using Game;
    using Round;

    public class DeclareTichuCommand : Command
    {
        private readonly Round _round;
        private readonly Player _player;
        private readonly TichuCall _call;

        public DeclareTichuCommand(Round round, Player player, TichuCall call)
        {
            
            _round = round;
            _player = player;
            _call = call;
        }

        public override void Execute()
        {
            _round.TichuCalls[_player] = _call;

            if (_call == TichuCall.GrandTichu)
                _player.DeclareGrandTichu();
            else if (_call == TichuCall.Tichu)
                _player.DeclareTichu();
        }
    }
}