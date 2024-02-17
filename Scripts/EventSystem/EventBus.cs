using System;
using System.Collections.Generic;
using Godot;

namespace RustyRedemption.EventSystem
{
    public class EventBus : IEventBus
    {
        private IDictionary<Type, ISet<IEventHandler>> listeners;
        private IList<IEventHandler> garbage;
        
        public EventBus()
        {
            listeners = new Dictionary<Type, ISet<IEventHandler>>();
            garbage = new List<IEventHandler>();
        }
        
        public void AddHandler<TEvent>(IEventHandler<TEvent> handler)
        {
            if (!listeners.ContainsKey(typeof(TEvent)))
            {
                listeners.Add(typeof(TEvent), new HashSet<IEventHandler>());
            }

            listeners[typeof(TEvent)].Add(handler);
        }

        public void RemoveHandler<TEvent>(IEventHandler<TEvent> handler)
        {
            if (listeners.TryGetValue(typeof(TEvent), out ISet<IEventHandler> targets))
            {
                targets.Remove(handler);
            }
        }
        
        public void Post<TEvent>(TEvent evt)
        {
            if (listeners.TryGetValue(typeof(TEvent), out ISet<IEventHandler> targets))
            {
                foreach (IEventHandler genericHandler in targets)
                {
                    if (genericHandler is GodotObject godotObject)
                    {
                        IEventHandler<TEvent> handler = (IEventHandler<TEvent>)genericHandler;

                        if (GodotObject.IsInstanceValid(godotObject))
                        {
                            handler.Handle(evt);
                        }
                        else
                        {
                            garbage.Add(handler);
                        }
                    }
                    else
                    {
                        IEventHandler<TEvent> handler = (IEventHandler<TEvent>)genericHandler;
                        handler.Handle(evt);
                    }
                }

                if (garbage.Count > 0)
                {
                    foreach (IEventHandler handler in garbage)
                    {
                        RemoveHandler((IEventHandler<TEvent>)handler);
                    }
                    
                    garbage.Clear();
                }
            }
        }
    }
}
