using RustyRedemption.Common;

namespace RustyRedemption.Events;

public class CombatFocusSelectedEvent : ICombatActionSelectButtonPressedEvent
{
    public PartyMembers Source { get; set; }
}