using Godot;
using RustyRedemption.Common;
using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.UI;

public partial class FocusScreen : Control, IEventHandler<CombatFocusSelectedEvent>
{
    [Export] private Control waves;
    [Export] private Control heart;
    [Export] private AnimatedSprite2D pressButtonIcon;
    
    [Export] private Vector2 wavesInitialPosition;
    [Export] private Vector2 wavesTargetPosition;

    [Export] private float timeLimit;
    [Export] private float percentagePerPress;

    private bool active = false;
    private float currentProgress = 0f;
    
    public override void _EnterTree()
    {
        Game.INSTANCE.EventBus.AddHandler<CombatFocusSelectedEvent>(this);
    }

    public override void _Process(double delta)
    {
        if (active)
        {
            if (Input.IsActionJustPressed("ui_accept"))
            {
                currentProgress += percentagePerPress / 100f;
                if (currentProgress > 100f) currentProgress = 100f;
            }
            
            UpdateWavePosition(currentProgress);
        }
    }

    public void Handle(CombatFocusSelectedEvent evt)
    {
        currentProgress = 0f;

        Color wavesTargetColor = PartyMemberData.Of(evt.Source).Color;
        Color wavesTransparentColor = PartyMemberData.Of(evt.Source).Color;
        wavesTransparentColor.A = 0f;

        Color heartTargetColor = PartyMemberData.OfOpposite(evt.Source).Color;
        Color heartTransparentColor = PartyMemberData.OfOpposite(evt.Source).Color;
        heartTransparentColor.A = 0f;
        
        waves.Position = wavesInitialPosition;
        waves.Modulate = wavesTransparentColor;
        heart.Modulate = heartTransparentColor;
        pressButtonIcon.Visible = false;
        
        Visible = true;

        Tween tween = GetTree().CreateTween();
        tween.TweenInterval(0.2f);
        tween.TweenProperty(waves, "modulate", wavesTargetColor, 0.2f);
        tween.Parallel().TweenProperty(heart, "modulate", heartTargetColor, 0.2f);
        tween.TweenInterval(0.2f);
        tween.TweenCallback(Callable.From(() =>
        {
            active = true;
            pressButtonIcon.Visible = true;
            pressButtonIcon.Play("default");
        }));
        tween.TweenInterval(timeLimit);
        tween.TweenCallback(Callable.From(() =>
        {
            active = false;
            pressButtonIcon.Visible = false;
        }));
        tween.TweenProperty(waves, "modulate", wavesTransparentColor, 0.2f);
        tween.Parallel().TweenProperty(heart, "modulate", heartTransparentColor, 0.2f);
        tween.TweenInterval(0.2f);
        tween.TweenCallback(Callable.From(() =>
        {
            Visible = false;

            float transferValue = Game.INSTANCE.PlayerState.GetHealth(evt.Source) * currentProgress;
            int roundedValue = Mathf.RoundToInt(transferValue);
            
            Game.INSTANCE.EventBus.Post(new DialogBoxTextChangedEvent() {Instant = false, Text = $"* You focus on your souls.      \n* Transferred {roundedValue}% HP!"});

            PartyMembers target = PartyMemberData.GetOpposite(evt.Source);
            
            // TODO actually apply transfer
            // TODO make sure at least 1% HP remains
            // TODO this doesn't belong here but fix HP bar particle effects vanishing during enemy turn
        }));
        tween.TweenInterval(4.0f);
        tween.TweenCallback(Callable.From(() =>
        {
            Game.INSTANCE.EventBus.Post(new CombatAfterPlayerTurnEvent());
        }));
    }
    
    private void UpdateWavePosition(float progress)
    {
        waves.Position = wavesInitialPosition.Lerp(wavesTargetPosition, progress);
    }
}