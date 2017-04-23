using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void EventDele<T>(T t);

public class EventManager<T>
{
    private static EventManager<T> _instance;

    public static EventManager<T> GetSingle()
    {
        if (null == _instance)
        {
            _instance = new EventManager<T>();
            Debug.Log("单例");
        }
        return _instance;
    }

    public Dictionary<int, List<EventDele<T>>> m_EventDic;

    private EventManager()
    {
        m_EventDic = new Dictionary<int, List<EventDele<T>>>();
    }

    public void RegistEvent(int EventID, EventDele<T> Event)
    {
        if (Event == null)
            return;

        List<EventDele<T>> tempEventList = null;

        if (m_EventDic.ContainsKey(EventID))
        {
            m_EventDic.TryGetValue(EventID, out tempEventList);
            if (tempEventList != null)
            {
                tempEventList.Add(Event);
                return;
            }
        }

        if(!m_EventDic.ContainsKey(EventID))
        {
            tempEventList = new List<EventDele<T>>();
            tempEventList.Add(Event);
            m_EventDic.Add(EventID, tempEventList);
            return;
        }
    }

    public void NotifyEvent(int EventID,T t)
    {
        if (t == null)
            return;

        List<EventDele<T>> tempEventList = null;

        if (m_EventDic.ContainsKey(EventID))
        {
            m_EventDic.TryGetValue(EventID, out tempEventList);

            foreach (var item in tempEventList)
            {
                item(t);
            }
        }
    }

    /// <summary>
    /// 解除绑定在该事件下的所方法
    /// </summary>
    /// <param name="EventID"></param>
    public void UnRegist(int EventID)
    {
        if (m_EventDic.ContainsKey(EventID))
        {
            m_EventDic.Remove(EventID);
        }
    }
    /// <summary>
    /// 解除绑定在该事件下的某个方法
    /// </summary>
    /// <param name="EventID"></param>
    /// <param name="Event"></param>
    public void UnRegist(int EventID, EventDele<T> Event)
    {
        if (!m_EventDic.ContainsKey(EventID))
            return;

        List<EventDele<T>> tempEventList = null;

        m_EventDic.TryGetValue(EventID, out tempEventList);
        if(tempEventList!=null)
        {
            tempEventList.Remove(Event);
            if (tempEventList.Count <= 0)
            {
                m_EventDic.Remove(EventID);
            }
        }
    }
}
