namespace System.Runtime.CompilerServices
{
    internal static class IsExternalInit {}
}

namespace Core.Events
{
    public record GrandTichuDecisionNeededEvent(string PlayerName, int PlayerIndex);
}