using Godot;
using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.UI;

public partial class EnemyTurnDarkOverlay : Control, IEventHandler<CombatBeforeEnemyTurnEvent>, IEventHandler<CombatAfterEnemyTurnEvent>
{
    [Export] private Color activeColor;
    [Export] private Color inactiveColor;
    [Export] private float fadeDuration;
    
    private Tween fadeTween;
    
    public override void _EnterTree()
    {
        Game.INSTANCE.EventBus.AddHandler<CombatBeforeEnemyTurnEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<CombatAfterEnemyTurnEvent>(this);
    }

    public void Handle(CombatBeforeEnemyTurnEvent evt)
    {
        FadeToColor(activeColor);
    }

    public void Handle(CombatAfterEnemyTurnEvent evt)
    {
        FadeToColor(inactiveColor);
    }

    private void FadeToColor(Color color)
    {
        if(fadeTween != null && fadeTween.IsValid()) fadeTween.Kill();

        fadeTween = GetTree().CreateTween();
        fadeTween.TweenProperty(this, "color", color, fadeDuration);
    }
}