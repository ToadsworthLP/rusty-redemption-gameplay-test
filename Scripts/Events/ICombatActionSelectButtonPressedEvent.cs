using RustyRedemption.Common;

namespace RustyRedemption.Events;

public interface ICombatActionSelectButtonPressedEvent
{
    PartyMembers Source { get; set; }
}