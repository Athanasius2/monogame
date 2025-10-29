using System;
using System.Collections.Generic;
using System.Linq;

namespace MonoGames1.Events
{
    public class EventBus
    {
        private Dictionary<Type, List<Action<EventArgs>>> _handlers = new();
        private Queue<EventArgs> _events = new();

        public EventBus() { }

        /// <summary>
        /// Register an event handler for the specified data type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="handler"></param>
        public void Subscribe<T>(Action<T> handler) where T : EventArgs
        {
            Type type = typeof(T);
            if (!_handlers.ContainsKey(type)) _handlers[type] = new();
            _handlers[type].Add(handler);
        }

        /// <summary>
        /// Push event data onto the event queue
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="eventData"></param>
        public void Push<T>(T eventData)
        {
        }

        /// <summary>
        /// Run all event handlers for all enqueued event data
        /// </summary>
        public void Dispatch()
        {
            while(_events.TryDequeue(out EventArgs? eventArgs))
            {
                Type type = eventArgs.GetType();
                if(_handlers.TryGetValue(type, out List<Delegate>? handlers))
                {
                    handlers
                        .Cast<Action<T>>()
                        .ToList()
                        .ForEach(h => h(eventArgs));
                }
            }
        }
    }
}
