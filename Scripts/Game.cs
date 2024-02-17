using Godot;
using RustyRedemption.Common;
using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption;

public partial class Game : Node
{
	public static Game INSTANCE { get; private set; }
	
	public EventBus EventBus { get; private set; }
	public PlayerState PlayerState { get; private set; }

	private Game()
	{
		EventBus = new EventBus();
		PlayerState = new PlayerState();
	}
	
	public override void _EnterTree()
	{
		if (INSTANCE != null)
		{
			QueueFree();
			return;
		}
		
		INSTANCE = this;
	}

	public override void _Ready()
	{
		EventBus.Post(new CombatStartEvent());
	}
}