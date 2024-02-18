using Godot;

namespace RustyRedemption.Bullets;

public partial class ThrowKnivesDownBullet : BaseBullet
{
    [Export] private float fadeInDuration;
    [Export] private float throwSpeed;
    [Export] private float lifetime;
    
    private Tween spawnAnimation = null;
    private bool thrown = false;
    private ulong spawnTime;
    
    public override void _Ready()
    {
        base._Ready();

        spawnTime = Time.GetTicksMsec();

        Modulate = new Color(Modulate.R, Modulate.G, Modulate.B, 0f);
        Color targetColor = new Color(Modulate.R, Modulate.G, Modulate.B, 1f);
        
        spawnAnimation = GetTree().CreateTween();
        spawnAnimation.TweenProperty(this, "modulate", targetColor, fadeInDuration);
        spawnAnimation.TweenCallback(Callable.From(() =>
        {
            thrown = true;
        }));
    }

    public override void _PhysicsProcess(double delta)
    {
        if (thrown)
        {
            Position += new Vector2(0f, 1f * throwSpeed * (float)delta);
        }

        if (Time.GetTicksMsec() > spawnTime + (lifetime * 1000))
        {
            QueueFree();
        }
    }
}