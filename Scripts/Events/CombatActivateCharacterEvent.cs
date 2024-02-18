using RustyRedemption.Common;

namespace RustyRedemption.Events;

public class CombatActivateCharacterEvent : ICombatActionSelectButtonPressedEvent
{
    public PartyMembers Source { get; set; }
    public PartyMembers Target { get; set; }
}