namespace Core.Round
{
    public interface IRoundState
    {
        public void OnEnter(Round round);
        public void OnExit(Round round);

        public IRoundState NextState();
    }
}