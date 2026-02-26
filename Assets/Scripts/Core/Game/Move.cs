using JetBrains.Annotations;

namespace Core.Game
{
    using Player;
    using Combination;
    public class Move
    {
        public Player Player { get; }
        [CanBeNull] public Combination Combination { get; }
        public bool IsPass =>  Combination != null;
        
        public  Move(Player player, Combination combination)
        {
            Player = player;
            Combination =  combination;
        }
        
    }
}