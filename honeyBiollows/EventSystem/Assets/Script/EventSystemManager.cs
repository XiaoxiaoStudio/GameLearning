using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum GameEventType
{
    One,Two,Three
}

public delegate void EventDelegate<T>(T arg);

public class EventSystemManager<T>
{
    #region 单例
    private static EventSystemManager<T> _instance;
    public static EventSystemManager<T> Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new EventSystemManager<T>();
            }
            return _instance;
        }
    }

    private EventSystemManager()
    {
        eventDic = new Dictionary<int, List<EventDelegate<T>>>();
        removeEvent = new List<EventDelegate<T>>();
    }

    #endregion

    private Dictionary<int, List<EventDelegate<T>>> eventDic = null;

    private List<EventDelegate<T>> removeEvent = null; 
    //通知
    public void Notification(int id,T data )
    {
        if (removeEvent.Count > 0)
        {
            removeEvent.Clear();
        }
        List<EventDelegate<T>> tempList = null;

        if (eventDic.TryGetValue(id,out tempList))
        {
            for (int i = tempList.Count-1; i >= 0; i--)
            {
                EventDelegate<T> temp = tempList[i];
                if (temp == null || temp.Target == null)
                {
                    removeEvent.Add(temp);
                }
                else
                {
                    temp(data);
                }
            }
            if (removeEvent.Count > 0)
            {
                foreach (var temp in removeEvent)
                {
                    tempList.Remove(temp);
                }
            }
        }
    }
    //注册
    public void Register(int id,EventDelegate<T> func )
    {
        if (func == null || func.Target == null) return;

        List<EventDelegate<T>> tempList = null;
        if (eventDic.TryGetValue(id,out tempList) == false)
        {
            tempList = new List<EventDelegate<T>>();
            tempList.Add(func);
            eventDic.Add(id,tempList);
            return;
        }
        if (!tempList.Contains(func))
        {
            tempList.Add(func);
        }
    }
    //取消注册
    public void Unregister(int id, EventDelegate<T> func)
    {
        if (removeEvent.Count > 0)
        {
            removeEvent.Clear();
        }
        List<EventDelegate<T>> tempList = null;

        if (eventDic.TryGetValue(id,out tempList))
        {
            for (int i = 0; i <tempList.Count; i++)
            {
                EventDelegate<T> temp = tempList[i];
                if (temp == null || temp.Target == null)
                {
                    removeEvent.Add(temp);
                }
                if (temp == func && temp.Target == func.Target)
                {
                    tempList.Remove(temp);
                    break;
                }
            }
            if (removeEvent.Count > 0)
            {
                foreach (var temp in removeEvent)
                {
                    tempList.Remove(temp);
                }
            }
        }
    }
    //清除
    public void ClearAll()
    {
        if (eventDic.Count >0)
        {
            eventDic.Clear();
        }
    }
}
