using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.Common;

public class PlayerState
{
    public  PartyMembers ActivePartyMember { get; set; }
    private int[] health;

    public PlayerState()
    {
        health = new int[2];
        health[(int)PartyMembers.KANAKO] = 50;
        health[(int)PartyMembers.CLOVER] = 50;
    }

    public int GetHealth(PartyMembers partyMember)
    {
        return health[(int)partyMember];
    }

    public void SetHealth(PartyMembers partyMember, int value)
    {
        health[(int)partyMember] = value;

        if (health[(int)partyMember] < 0) health[(int)partyMember] = 0;
        
        Game.INSTANCE.EventBus.Post(new HealthUpdatedEvent()
        {
            PartyMember = partyMember,
            Value = value
        });
    }
}