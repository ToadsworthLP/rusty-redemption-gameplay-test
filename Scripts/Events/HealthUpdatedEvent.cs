using RustyRedemption.Common;

namespace RustyRedemption.Events;

public class HealthUpdatedEvent
{
    public PartyMembers PartyMember { get; set; }
    public int Value { get; set; }
}