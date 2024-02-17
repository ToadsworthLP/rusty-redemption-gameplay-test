using RustyRedemption.Common;

namespace RustyRedemption.Events;

public class HealthUpdatedEvent
{
    public PartyMember PartyMember { get; set; }
    public int Value { get; set; }
}