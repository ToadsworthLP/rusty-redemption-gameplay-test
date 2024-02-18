using Godot;
using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.Attacks;

public partial class TrainingDummyThrowKnivesDownAttack : Node2D, IEventHandler<CombatPlayerDiedEvent>
{
    [Export] private PackedScene knifeScene;
    [Export] private int totalKnives;
    [Export] private Vector2 spawnerEdge1;
    [Export] private Vector2 spawnerEdge2;

    private Timer timer;
    private RandomNumberGenerator rng;
    private int spawnedKnives = 0;

    public override void _EnterTree()
    {
        Game.INSTANCE.EventBus.AddHandler(this);
    }

    public override void _Ready()
    {
        rng = new RandomNumberGenerator();
        rng.Randomize();

        timer = GetChild<Timer>(0);
        
        timer.Start();
        timer.Timeout += () =>
        {
            if (spawnedKnives < totalKnives)
            {
                SpawnKnife();
                spawnedKnives++;
            }
            else
            {
                timer.Stop();
                
                SceneTreeTimer endTimer = GetTree().CreateTimer(1.5f);
                endTimer.Timeout += () =>
                {
                    if(!IsInstanceValid(this)) return;
                    
                    Game.INSTANCE.EventBus.Post(new CombatAfterEnemyTurnEvent());
                    QueueFree();
                };
            }
        };
    }

    private void SpawnKnife()
    {
        Vector2 position = spawnerEdge1.Lerp(spawnerEdge2, rng.Randf());

        var instance = knifeScene.Instantiate<Node2D>();
        instance.Position = position;
        AddChild(instance);
    }

    public void Handle(CombatPlayerDiedEvent evt)
    {
        timer.Stop();
        QueueFree();
    }
}