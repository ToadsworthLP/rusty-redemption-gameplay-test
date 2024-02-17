using Godot;
using RustyRedemption.Common;
using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.UI;

public partial class ActionSelectGroup : Control, IEventHandler<CombatActivateCharacterEvent>, IEventHandler<CombatAfterPlayerTurnEvent>
{
    [Export] private PartyMember partyMember;
    [Export] private Control defaultFocusedButton;

    public override void _EnterTree()
    {
        Game.INSTANCE.EventBus.AddHandler<CombatActivateCharacterEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<CombatAfterPlayerTurnEvent>(this);
    }

    public void Handle(CombatActivateCharacterEvent evt)
    {
        Visible = evt.Target == partyMember;
        if(Visible) defaultFocusedButton.GrabFocus();
    }

    public void Handle(CombatAfterPlayerTurnEvent evt)
    {
        Visible = false;
    }
}