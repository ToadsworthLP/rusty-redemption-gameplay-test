using Godot;
using RustyRedemption.Common;
using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.UI;

public partial class ActOptionSelectButton : BaseButton, IEventHandler<CombatActivateCharacterEvent>
{
    [Export] private ActOption option;
    
    public override void _EnterTree()
    {
        Game.INSTANCE.EventBus.AddHandler(this);
    }

    public override void _Pressed()
    {
        switch (option)
        {
            case ActOption.CHECK:
                // Game.INSTANCE.EventBus.Post(new CombatActOptionSelectedEvent() { Option = ActOption.CHECK });
                break;
            case ActOption.TALK:
                Game.INSTANCE.EventBus.Post(new CombatActOptionSelectedEvent() { Option = ActOption.TALK });
                break;
        }
    }

    public void Handle(CombatActivateCharacterEvent evt)
    {
        Modulate = PartyMemberData.Of(evt.Target).Color;
    }
}