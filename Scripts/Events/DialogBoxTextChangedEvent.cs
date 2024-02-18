namespace RustyRedemption.Events;

public class DialogBoxTextChangedEvent
{
    public string Text { get; set; }
    public bool Instant { get; set; }
}