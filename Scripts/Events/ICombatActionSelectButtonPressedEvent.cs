using RustyRedemption.Common;

namespace RustyRedemption.Events;

public interface ICombatActionSelectButtonPressedEvent
{
    PartyMember Source { get; set; }
}