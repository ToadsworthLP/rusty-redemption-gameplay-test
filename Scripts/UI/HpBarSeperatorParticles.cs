using Godot;
using RustyRedemption.Common;
using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.UI;

public partial class HpBarSeperatorParticles : Node2D, IEventHandler<CombatActivateCharacterEvent>
{
    [Export] private PartyMembers character;
    
    public override void _EnterTree()
    {
        Game.INSTANCE.EventBus.AddHandler(this);
    }

    public void Handle(CombatActivateCharacterEvent evt)
    {
        Visible = evt.Target == character;
    }
}