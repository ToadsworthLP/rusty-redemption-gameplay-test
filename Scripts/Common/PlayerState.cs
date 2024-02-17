using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.Common;

public class PlayerState
{
    public  PartyMember ActivePartyMember { get; set; }
    private int[] health;

    public PlayerState()
    {
        health = new int[2];
        health[(int)PartyMember.KANAKO] = 50;
        health[(int)PartyMember.CLOVER] = 50;
    }

    public int GetHealth(PartyMember partyMember)
    {
        return health[(int)partyMember];
    }

    public void SetHealth(PartyMember partyMember, int value)
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