﻿using Godot;
using RustyRedemption.Common;
using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.Combat;

public partial class CombatManager : Node, IEventHandler<CombatStartEvent>, IEventHandler<CombatAfterPlayerTurnEvent>, IEventHandler<CombatPlayerTurnEvent>, IEventHandler<CombatActivateCharacterEvent>, IEventHandler<CombatTakeDamageEvent>, IEventHandler<CombatSoulDeteriorationEvent>, IEventHandler<CombatSoulFocusEvent>
{
    [Export] private PartyMembers initialActivePartyMember;
    
    public override void _EnterTree()
    {
        Game.INSTANCE.EventBus.AddHandler<CombatStartEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<CombatAfterPlayerTurnEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<CombatPlayerTurnEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<CombatTakeDamageEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<CombatActivateCharacterEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<CombatSoulDeteriorationEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<CombatSoulFocusEvent>(this);
    }

    public void Handle(CombatStartEvent evt)
    {
        Game.INSTANCE.EventBus.Post(new CombatPlayerTurnEvent());
        Game.INSTANCE.EventBus.Post(new CombatActivateCharacterEvent { Target = initialActivePartyMember });
    }

    public void Handle(CombatAfterPlayerTurnEvent evt)
    {
        Game.INSTANCE.EventBus.Post(new CombatBeforeEnemyTurnEvent());
    }

    public void Handle(CombatPlayerTurnEvent evt)
    {
        Game.INSTANCE.EventBus.Post(new CombatActivateCharacterEvent { Target = initialActivePartyMember });
    }
    
    public void Handle(CombatTakeDamageEvent evt)
    {
        PartyMembers partyMember = Game.INSTANCE.PlayerState.ActivePartyMember;
        Game.INSTANCE.PlayerState.SetHealth(partyMember, Game.INSTANCE.PlayerState.GetHealth(partyMember) - evt.Value);
        
        if (Game.INSTANCE.PlayerState.GetHealth(partyMember) <= 0)
            Game.INSTANCE.EventBus.Post(new CombatPlayerDiedEvent());
    }

    public void Handle(CombatActivateCharacterEvent evt)
    {
        Game.INSTANCE.PlayerState.ActivePartyMember = evt.Target;
    }

    public void Handle(CombatSoulDeteriorationEvent evt)
    {
        PartyMembers target = Game.INSTANCE.PlayerState.ActivePartyMember;
        PartyMembers other = PartyMemberData.GetOpposite(target);

        int targetHealth = Game.INSTANCE.PlayerState.GetHealth(target);
        int otherHealth = Game.INSTANCE.PlayerState.GetHealth(other);

        if (targetHealth - evt.Value > 0)
        {
            Game.INSTANCE.PlayerState.SetHealth(target, targetHealth - evt.Value);
            
            if(otherHealth + evt.Value <= 100)
                Game.INSTANCE.PlayerState.SetHealth(other, otherHealth + evt.Value);
        }
    }

    public void Handle(CombatSoulFocusEvent evt)
    {
        PartyMembers source = Game.INSTANCE.PlayerState.ActivePartyMember;
        PartyMembers target = PartyMemberData.GetOpposite(source);
        
        int sourceHealth = Game.INSTANCE.PlayerState.GetHealth(source);
        int targetHealth = Game.INSTANCE.PlayerState.GetHealth(target);
        
        Game.INSTANCE.PlayerState.SetHealth(source, sourceHealth - evt.Value);
        Game.INSTANCE.PlayerState.SetHealth(target, targetHealth + evt.Value);
    }
}