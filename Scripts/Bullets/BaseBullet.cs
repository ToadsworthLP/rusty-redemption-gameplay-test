using Godot;
using RustyRedemption.Combat;

namespace RustyRedemption.Bullets;

public partial class BaseBullet : Node2D
{
    [Export] private int attack;
    [Export] private bool destroyAtCollision;
    [Export] private Area2D collider;

    public override void _Ready()
    {
        collider.BodyEntered += OnBodyEntered;
    }

    private void OnBodyEntered(Node2D other)
    {
        if (other is PlayerSoul soul)
        {
            if(soul.Active) OnCollideWithSoul(soul);
        }
    }

    protected virtual void OnCollideWithSoul(PlayerSoul soul)
    {
        soul.TakeDamage(attack);
        if(destroyAtCollision) QueueFree();
    }
}