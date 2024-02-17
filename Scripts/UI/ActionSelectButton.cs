using Godot;
using RustyRedemption.Common;
using RustyRedemption.Events;

namespace RustyRedemption.UI;

public partial class ActionSelectButton : BaseButton
{
    [Export] private PartyMember partyMember;
    [Export] private CombatAction action;

    public override void _Pressed()
    {
        switch (action)
        {
            case CombatAction.FIGHT:
                Game.INSTANCE.EventBus.Post(new CombatAfterPlayerTurnEvent());
                break;
            case CombatAction.ACT:
                break;
            case CombatAction.ITEM:
                break;
            case CombatAction.MERCY:
                break;
            case CombatAction.FOCUS:
                break;
            case CombatAction.SWAP:
                CombatActivateCharacterEvent evt = new CombatActivateCharacterEvent { Source = partyMember };
                evt.Target = evt.Source == PartyMember.CLOVER ? PartyMember.KANAKO : PartyMember.CLOVER;
                Game.INSTANCE.EventBus.Post(evt);
                break;
        }
    }

    private void PostPressedEvent<T>() where T : ICombatActionSelectButtonPressedEvent, new()
    {
        T evt = new T();
        evt.Source = partyMember;
        Game.INSTANCE.EventBus.Post<T>((T)evt);
    }
}