using Godot;
using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.UI;

public partial class GameOverScreen : Control, IEventHandler<CombatPlayerDiedEvent>
{
    public override void _EnterTree()
    {
        Game.INSTANCE.EventBus.AddHandler(this);
    }

    public void Handle(CombatPlayerDiedEvent evt)
    {
        Visible = true;
    }
}