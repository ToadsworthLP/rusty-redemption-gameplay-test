using System;
using Godot;
using RustyRedemption.Common;
using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.Combat;

public partial class PlayerSoul : CharacterBody2D, IEventHandler<CombatEnemyTurnEvent>, IEventHandler<CombatAfterEnemyTurnEvent>, IEventHandler<CombatActivateCharacterEvent>, IEventHandler<CombatTakeDamageEvent>, IEventHandler<CombatPlayerDiedEvent>
{
    [Export] private float movementSpeed = 1f;
    [Export] private float invincibilityDuration = 1;
    [Export] private int deteriorationDistance;
    
    [Export] private AnimatedSprite2D soulSprite;
    [Export] private string defaultAnimationName;
    [Export] private string damageAnimationName;
    [Export] private string deathAnimationName;

    public bool Active { get; private set; }
    
    private bool invincible = false;
    private float distanceTraveled = 0f;

    private Tween iFrameTimerTween = null;
    
    public override void _EnterTree()
    {
        Game.INSTANCE.EventBus.AddHandler<CombatEnemyTurnEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<CombatAfterEnemyTurnEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<CombatActivateCharacterEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<CombatTakeDamageEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<CombatPlayerDiedEvent>(this);
    }

    public override void _PhysicsProcess(double delta)
    {
        if(!Active) return;

        Vector2 initialPosition = Position;
        Velocity = GetNormalizedInput() * movementSpeed;
        MoveAndSlide();

        distanceTraveled += (Position - initialPosition).Length();
        HandleSoulDeterioration();

        if(Input.IsPhysicalKeyPressed(Key.Q)) TakeDamage(40);
    }

    private Vector2 GetNormalizedInput()
    {
        return Input.GetVector("move_left", "move_right", "move_up", "move_down");
    }

    private void HandleSoulDeterioration()
    {
        int deterioration = Mathf.RoundToInt(distanceTraveled / deteriorationDistance);
        if (deterioration > 0)
        {
            Game.INSTANCE.EventBus.Post(new CombatSoulDeteriorationEvent { Value = deterioration });
            distanceTraveled = 0;
        }
    }

    public void Handle(CombatEnemyTurnEvent evt)
    {
        Active = true;
        invincible = false;
        distanceTraveled = 0;
    }

    public void Handle(CombatAfterEnemyTurnEvent evt)
    {
        Active = false;
    }

    public void Handle(CombatActivateCharacterEvent evt)
    {
        Modulate = PartyMemberData.Of(evt.Target).Color;
    }

    public void Handle(CombatTakeDamageEvent evt)
    {
        if(invincible || !Active) return;

        soulSprite.Play(damageAnimationName);
        invincible = true;

        iFrameTimerTween = GetTree().CreateTween();
        iFrameTimerTween.TweenInterval(invincibilityDuration);
        iFrameTimerTween.TweenCallback(Callable.From(() =>
        {
            soulSprite.Play(defaultAnimationName);
            invincible = false;
        }));
    }
    
    public void Handle(CombatPlayerDiedEvent evt)
    {
        if(iFrameTimerTween != null && iFrameTimerTween.IsValid()) iFrameTimerTween.Kill();
        
        Active = false;
        soulSprite.Stop();
        soulSprite.Play(deathAnimationName);
    }

    public void TakeDamage(int value)
    {
        if(invincible) return;

        Game.INSTANCE.EventBus.Post(new CombatTakeDamageEvent() { Value = value });
    }
}