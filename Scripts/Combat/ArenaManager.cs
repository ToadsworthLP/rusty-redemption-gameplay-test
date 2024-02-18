using Godot;
using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.Combat;

public partial class ArenaManager : Node2D, IEventHandler<CombatBeforeEnemyTurnEvent>, IEventHandler<CombatAfterEnemyTurnEvent>
{
    [Export] private PlayerSoul playerSoul;
    [Export] private Vector2 soulStartPosition;
    
    [Export] private Control arenaVisuals;
    [Export] private float arenaSizeChangeDuration;

    [Export] private Vector2 dialogBoxSize;
    [Export] private Vector2 arenaTargetSize;

    [Export] private PackedScene attackScene;
    
    public override void _EnterTree()
    {
        Game.INSTANCE.EventBus.AddHandler<CombatBeforeEnemyTurnEvent>(this);
        Game.INSTANCE.EventBus.AddHandler<CombatAfterEnemyTurnEvent>(this);
    }

    public void Handle(CombatBeforeEnemyTurnEvent evt)
    {
        Visible = true;
        
        playerSoul.Position = soulStartPosition;
        playerSoul.Visible = true;
        
        Tween arenaSizeTween = GetTree().CreateTween();
        arenaSizeTween.TweenProperty(arenaVisuals, "custom_minimum_size", arenaTargetSize, arenaSizeChangeDuration);
        arenaSizeTween.TweenCallback(Callable.From(() =>
        {
            Game.INSTANCE.EventBus.Post(new CombatEnemyTurnEvent());
            EnemyTurn();
        }));
    }

    public void Handle(CombatAfterEnemyTurnEvent evt)
    {
        playerSoul.Visible = false;
        
        Tween arenaSizeTween = GetTree().CreateTween();
        arenaSizeTween.TweenProperty(arenaVisuals, "custom_minimum_size", dialogBoxSize, arenaSizeChangeDuration);
        arenaSizeTween.TweenCallback(Callable.From(() =>
        {
            Visible = false;
            Game.INSTANCE.EventBus.Post(new CombatPlayerTurnEvent());
        }));
    }

    private void EnemyTurn()
    {
        var scene = attackScene.Instantiate();
        AddChild(scene);
    }
}