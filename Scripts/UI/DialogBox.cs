using Godot;
using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.UI;

public partial class DialogBox : Control, IEventHandler<CombatBeforeEnemyTurnEvent>, IEventHandler<CombatPlayerTurnEvent>
{
    public override void _EnterTree()
    {
        Game.INSTANCE.EventBus.AddHandler<CombatBeforeEnemyTurnEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<CombatPlayerTurnEvent>(this);
    }

    public void Handle(CombatBeforeEnemyTurnEvent evt)
    {
        Visible = false;
    }

    public void Handle(CombatPlayerTurnEvent evt)
    {
        Visible = true;
        Game.INSTANCE.EventBus.Post(new DialogBoxTextChangedEvent() { Text = "* You encountered the Dummy.", Instant = false });
    }
}