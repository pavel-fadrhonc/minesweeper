using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public interface IEventListenerBase {}

    public interface IEventListener<TEvent> : IEventListenerBase
    {
        void OnEvent(TEvent evt);
    }
    
    public static class EventDispatcher
    {
        private static Dictionary<Type, List<IEventListenerBase>> _listeners = new();

        [RuntimeInitializeOnLoadMethod]
        private static void Init()
        {
            _listeners.Clear();
        }
        
        public static void Dispatch<TEvent>(TEvent _event)
            where TEvent : struct 
        {
            var type = _event.GetType();

            if (!_listeners.ContainsKey(type))
                return;

            foreach (var listener in _listeners[type])
            {
                (listener as IEventListener<TEvent>).OnEvent(_event);    
            }
        }

        public static void AddListener<TEvent>(IEventListener<TEvent> listener)
        {
            var type = typeof(TEvent);
            
            if (!_listeners.ContainsKey(type))
                _listeners.Add(type, new List<IEventListenerBase>());
            
            _listeners[type].Add(listener);
        }

        public static void RemoveListener<TEvent>(IEventListener<TEvent> listener)
            where TEvent : struct
        {
            var type = typeof(TEvent);

            if (!_listeners.ContainsKey(type))
                return;
            
            _listeners[type].Remove(listener);
        }
    }
}