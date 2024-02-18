using RustyRedemption.Common;

namespace RustyRedemption.Events;

public class CombatActSelectedEvent : ICombatActionSelectButtonPressedEvent
{
    public PartyMembers Source { get; set; }
}