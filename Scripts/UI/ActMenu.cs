using Godot;
using RustyRedemption.Common;
using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.UI;

public partial class ActMenu : Control, IEventHandler<CombatActSelectedEvent>, IEventHandler<CombatActOptionSelectedEvent>
{
    [Export] private BaseButton defaultFocusedButton;
    
    public override void _EnterTree()
    {
        Game.INSTANCE.EventBus.AddHandler<CombatActSelectedEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<CombatActOptionSelectedEvent>(this);
    }

    public void Handle(CombatActSelectedEvent evt)
    {
        Visible = true;
        defaultFocusedButton.GrabFocus();
    }

    public void Handle(CombatActOptionSelectedEvent evt)
    {
        Visible = false;
        if(evt.Option == ActOption.TALK) HandleTalkAct();
    }

    private void HandleTalkAct()
    {
        Tween tween = GetTree().CreateTween();
        tween.TweenCallback(Callable.From(() =>
        {
            Game.INSTANCE.EventBus.Post(new DialogBoxTextChangedEvent() { Instant = false, Text = "* You tried to talk to the\n  dummy.             \n* No response."});
        }));
        tween.TweenInterval(4.0f);
        tween.TweenCallback(Callable.From(() =>
        {
            Game.INSTANCE.EventBus.Post(new CombatAfterPlayerTurnEvent());
        }));
    }
}