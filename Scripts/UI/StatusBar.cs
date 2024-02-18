using Godot;
using RustyRedemption.Common;
using RustyRedemption.Events;
using RustyRedemption.EventSystem;

namespace RustyRedemption.UI;

public partial class StatusBar : Control, IEventHandler<HealthUpdatedEvent>
{
    [Export] private Label kanakoHealthLabel;
    [Export] private Range kanakoHealthBar;
    [Export] private Label cloverHealthLabel;
    [Export] private Range cloverHealthBar;

    public override void _EnterTree()
    {
        Game.INSTANCE.EventBus.AddHandler(this);
    }

    public override void _Ready()
    {
        UpdateHealth(kanakoHealthLabel, kanakoHealthBar, Game.INSTANCE.PlayerState.GetHealth(PartyMembers.KANAKO));
        UpdateHealth(cloverHealthLabel, cloverHealthBar, Game.INSTANCE.PlayerState.GetHealth(PartyMembers.CLOVER));
    }

    public void Handle(HealthUpdatedEvent evt)
    {
        if(evt.PartyMember == PartyMembers.KANAKO)
            UpdateHealth(kanakoHealthLabel, kanakoHealthBar, evt.Value);
        else
            UpdateHealth(cloverHealthLabel, cloverHealthBar, evt.Value);
    }

    private void UpdateHealth(Label label, Range healthBar, int value)
    {
        label.Text = $"{value}%";
        healthBar.Value = value;
    }
}