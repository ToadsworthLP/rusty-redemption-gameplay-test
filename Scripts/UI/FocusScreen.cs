using System;
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
    [Export] private Control finishBrightPart;
    
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

        Color targetColor = PartyMemberData.Of(evt.Source).Color;
        Color transparentColor = PartyMemberData.Of(evt.Source).Color;
        transparentColor.A = 0f;
        
        waves.Position = wavesInitialPosition;
        waves.Modulate = transparentColor;
        heart.Modulate = transparentColor;
        finishBrightPart.Modulate = transparentColor;
        pressButtonIcon.Visible = false;
        
        Visible = true;

        Tween tween = GetTree().CreateTween();
        tween.TweenInterval(0.2f);
        tween.TweenProperty(waves, "modulate", targetColor, 0.2f);
        tween.Parallel().TweenProperty(heart, "modulate", targetColor, 0.2f);
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
        tween.TweenProperty(finishBrightPart, "modulate", targetColor, 0.025f);
        tween.TweenProperty(waves, "modulate", transparentColor, 0.0f);
        tween.Parallel().TweenProperty(heart, "modulate", transparentColor, 0.2f);
        tween.Parallel().TweenProperty(finishBrightPart, "modulate", transparentColor, 0.2f);
        tween.TweenInterval(0.2f);
        tween.TweenCallback(Callable.From(() =>
        {
            Visible = false;
            int transferValue = CalculateTransferAmount(Game.INSTANCE.PlayerState.GetHealth(evt.Source), Game.INSTANCE.PlayerState.GetHealth(PartyMemberData.GetOpposite(evt.Source)), currentProgress);
            
            Game.INSTANCE.EventBus.Post(new DialogBoxTextChangedEvent() {Instant = false, Text = $"* You focus on your SOULs.      \n* Transferred {transferValue}% HP!"});
            Game.INSTANCE.EventBus.Post(new CombatSoulFocusEvent() { Value = transferValue });
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

    private int CalculateTransferAmount(int sourceHealth, int targetHealth, float multiplier)
    {
        int amount = Mathf.RoundToInt(sourceHealth * multiplier);
        
        if (sourceHealth - amount <= 1) amount = sourceHealth - 1;
        if (targetHealth + amount > 100) amount = 100 - targetHealth;
        
        return amount;
    }
}