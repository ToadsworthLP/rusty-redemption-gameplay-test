namespace RustyRedemption.EventSystem
{
    public interface IEventHandler { }
    
    public interface IEventHandler<T> : IEventHandler
    {
        void Handle(T evt);
    }
}