namespace RustyRedemption.EventSystem
{
    public interface IEventBus
    {
        void AddHandler<TEvent>(IEventHandler<TEvent> handler);
        void RemoveHandler<TEvent>(IEventHandler<TEvent> handler);
        void Post<TEvent>(TEvent evt);
    }
}