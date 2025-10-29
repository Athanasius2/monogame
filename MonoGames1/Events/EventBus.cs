using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGames1.Events
{
    public class EventBus
    {
        private readonly Dictionary<Type, List<Delegate>> _handlers = [];
        private readonly Queue<EventArgs> _events = [];

        public EventBus() { }

        /// <summary>
        /// Register an event handler for the specified data type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void Subscribe<TEventArgs>(Action<TEventArgs> handler) where TEventArgs : EventArgs
        {
            Type type = typeof(TEventArgs);
            if (!_handlers.ContainsKey(type)) _handlers[type] = [];
            _handlers[type].Add(handler);
        }

        /// <summary>
        /// Push event data onto the event queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventData"></param>
        public void Push<TEventArgs>(TEventArgs eventArgs) where TEventArgs : EventArgs
        {
            _events.Enqueue(eventArgs);
        }

        /// <summary>
        /// Run all event handlers for all enqueued event data
        /// </summary>
        public void DispatchAll()
        {
            while(_events.TryDequeue(out EventArgs? eventArgs))
            {
                Type type = eventArgs.GetType();
                if(_handlers.TryGetValue(type, out List<Delegate>? handlers))
                {
                    handlers.ForEach(h => h.DynamicInvoke(eventArgs));
                }
            }
        }
    }
}
