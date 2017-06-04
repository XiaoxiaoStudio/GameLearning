using System.Collections.Generic;

namespace Air2000
{
    public delegate void EventHandlerDelegate(Evt evt);

    public class EventManager
    {
        private Dictionary<int, List<EventHandlerDelegate>> m_RegisteredHandlers;

        public EventManager()
        {
            m_RegisteredHandlers = new Dictionary<int, List<EventHandlerDelegate>>();
        }

        private void Dispatch(int id, Evt evt)
        {
            List<EventHandlerDelegate> handlers = null;
            if (m_RegisteredHandlers.TryGetValue(id, out handlers))
            {
                if (handlers != null && handlers.Count > 0)
                {
                    for (int i = 0; i < handlers.Count; i++)
                    {
                        EventHandlerDelegate handler = handlers[i];
                        if (handler == null)
                        {
                            handlers.RemoveAt(i);
                            i--;
                        }
                        else
                        {
                            handler(evt);
                        }
                    }
                }
            }
        }

        private void AddHandler(int id, EventHandlerDelegate handler)
        {
            if (handler == null)
            {
                return;
            }
            if (m_RegisteredHandlers == null)
            {
                m_RegisteredHandlers = new Dictionary<int, List<EventHandlerDelegate>>();
            }
            List<EventHandlerDelegate> handlers = null;
            if (m_RegisteredHandlers.TryGetValue((int)id, out handlers) == false)
            {
                handlers = new List<EventHandlerDelegate>();
                m_RegisteredHandlers.Add(id, handlers);
            }
            for (int i = 0; i < handlers.Count; i++)
            {
                EventHandlerDelegate tempHandler = handlers[i];
                if (tempHandler == handler)
                {
                    return;
                }
            }
            handlers.Add(handler);
        }

        private void RemoveHandler(int id, EventHandlerDelegate handler)
        {
            List<EventHandlerDelegate> handlers = null;
            if (m_RegisteredHandlers.TryGetValue(id, out handlers))
            {
                if (handlers != null && handlers.Count > 0)
                {
                    handlers.Remove(handler);
                }
            }
        }

        private void RemoveAllHandlers()
        {
            if (m_RegisteredHandlers != null)
            {
                m_RegisteredHandlers.Clear();
            }
        }

        public void Notify(Evt evt)
        {
            if (evt == null)
            {
                return;
            }
            Dispatch(evt.ID, evt);
        }

        public void Register(int id, EventHandlerDelegate handler)
        {
            if (handler != null)
            {
                AddHandler(id, handler);
            }
        }

        public void Unregister(int id, EventHandlerDelegate handler)
        {
            if (handler != null)
            {
                RemoveHandler(id, handler);
            }
        }

        public void UnregisterAll()
        {
            RemoveAllHandlers();
        }
    }
}
