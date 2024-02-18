using Godot;
using RustyRedemption.Common;
using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.UI;

public partial class ActionSelectButton : BaseButton, IEventHandler<CombatActionSelectedEvent>, IEventHandler<CombatAfterPlayerTurnEvent>
{
    [Export] private PartyMembers partyMember;
    [Export] private CombatAction action;

    public override void _EnterTree()
    {
        Game.INSTANCE.EventBus.AddHandler<CombatActionSelectedEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<CombatAfterPlayerTurnEvent>(this);
    }

    public override void _Pressed()
    {
        switch (action)
        {
            case CombatAction.FIGHT:
                break;
            case CombatAction.ACT:
                PostPressedEvent<CombatActSelectedEvent>();
                break;
            case CombatAction.ITEM:
                break;
            case CombatAction.MERCY:
                break;
            case CombatAction.FOCUS:
                PostPressedEvent<CombatFocusSelectedEvent>();
                break;
            case CombatAction.SWAP:
                CombatActivateCharacterEvent evt = new CombatActivateCharacterEvent { Source = partyMember };
                evt.Target = evt.Source == PartyMembers.CLOVER ? PartyMembers.KANAKO : PartyMembers.CLOVER;
                Game.INSTANCE.EventBus.Post(evt);
                break;
        }
    }

    private void PostPressedEvent<T>() where T : ICombatActionSelectButtonPressedEvent, new()
    {
        T evt = new T();
        evt.Source = partyMember;
        Game.INSTANCE.EventBus.Post(new CombatActionSelectedEvent());
        Game.INSTANCE.EventBus.Post<T>((T)evt);
    }

    public void Handle(CombatActionSelectedEvent evt)
    {
        Disabled = true;
        FocusMode = FocusModeEnum.None;
    }

    public void Handle(CombatAfterPlayerTurnEvent evt)
    {
        Disabled = false;
        FocusMode = FocusModeEnum.All;
    }
}