using RustyRedemption.Common;

namespace RustyRedemption.Events;

public class CombatActivateCharacterEvent : ICombatActionSelectButtonPressedEvent
{
    public PartyMember Source { get; set; }
    public PartyMember Target { get; set; }
}