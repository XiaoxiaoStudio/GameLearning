using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    event1,
    event2,
    event3
}

public delegate void EventFuncDele<T>(T arg);

public class EventManager<T>
{
    public Dictionary<int, List<EventFuncDele<T>>> EventFuncDic;

    private static EventManager<T> _instance;

    public static EventManager<T> GetSingle()
    {
        if (null == _instance)
        {
            _instance = new EventManager<T>();
        }
        return _instance;
    }

    public EventManager()
    {
        EventFuncDic = new Dictionary<int, List<EventFuncDele<T>>>();
    }

    /// <summary>
    /// 注册事件
    /// </summary>
    /// <param name="common"></param>
    /// <param name="function"></param>
    public void Register(int common, EventFuncDele<T> function)
    {
        if (null == function || function.Target == null)
        {
            return;
        }
        //该委托集合不存在
        List<EventFuncDele<T>> tempList = null;
        if (!EventFuncDic.TryGetValue(common, out tempList))
        {
            tempList = new List<EventFuncDele<T>>();
            tempList.Add(function);
            EventFuncDic.Add(common, tempList);
            return;
        }
        //委托集合存在
        foreach (var item in tempList)
        {
            if(item == function)//方法是否已存在
            return;
        }
        tempList.Add(function);
    }

    /// <summary>
    /// 发布通知
    /// </summary>
    /// <param name="id"></param>
    /// <param name="parma1"></param>
    public void NotifyEvent(int id, T parma1)
    {
        if (parma1 == null)
        {
            return;
        }

        List<EventFuncDele<T>> tempList = null;
        if (EventFuncDic.TryGetValue(id, out tempList))
        {
            foreach (var item in tempList)
            {
                item(parma1);
            }
        }
    }

    /// <summary>
    /// 解注册事件
    /// </summary>
    /// <param name="common"></param>
    /// <param name="function"></param>
    public void UnRegister(int common, EventFuncDele<T> function)
    {
        if (function == null)
        {
            return;
        }

        List<EventFuncDele<T>> tempList = null;
        if (EventFuncDic.TryGetValue(common, out tempList))
        {
            foreach (var item in tempList)
            {
                if (item == function)
                {
                    tempList.Remove(item);
                    return;
                }
            }
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// 完全解注册
    /// </summary>
    public void Clear()
    {
        EventFuncDic.Clear();
    }
}
