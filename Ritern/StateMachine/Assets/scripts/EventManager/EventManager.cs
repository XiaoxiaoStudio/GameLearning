using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EventType
{
    eventOne,
    eventTwo,
    eventThree
}


public delegate void eventDelegate<T>(T arg);
public class EventManager<T>
{
    private static EventManager<T> intance;
    public Dictionary<int, List<eventDelegate<T>>> eventManager;
    public List<eventDelegate<T>> removeEventDic;//清除

    public static EventManager<T> GetIntance()
    {
        if (intance == null)
        {
            intance = new EventManager<T>();
            Debug.Log("sss");
        }
        return intance;
    }

    private EventManager()
    {
        eventManager = new Dictionary<int, List<eventDelegate<T>>>();
        removeEventDic = new List<eventDelegate<T>>();
    }

    public void RigisterEvent(int command, eventDelegate<T> fuction)
    {
        if (fuction == null||fuction.Target==null)
            return;

        List<eventDelegate<T>> eventList = null;
        if (eventManager.TryGetValue(command, out eventList)  == false)
        {
            if (eventList == null)
            {
                eventList = new List<eventDelegate<T>>();
                eventList.Add(fuction);
                eventManager.Add(command, eventList);
                return;
            }
        }
        foreach (var item in eventList)
        {
            if (fuction == item )
                return;
        }
        eventList.Add(fuction);

        //foreach (var item in eventList)
        //{
        //    if (fuction == item)
        //        return;
        //}
        // eventList.Add(fuction);
        //else
        //{
        //    eventList = new List<eventDelegate>();
        //    eventList.Add(fuction);
        //    eventManager.Add(command, eventList);

        //}

    }
    public void UnRigisterEvent(int command, eventDelegate<T> fuction)
    {
        List<eventDelegate<T>> eventList = null;
        if(eventManager.TryGetValue(command,out eventList))
        {
            for(int i = 0; i < eventList.Count; i++)
            {
                if (eventList[i] == null || eventList[i].Target == null)
                {
                    removeEventDic.Add(eventList[i]);
                }
                if (fuction == eventList[i])
                {
                    eventList.Remove(fuction);
                    break;
                }
            }
        }
        if (removeEventDic.Count > 0)
        {
            removeEventDic.Clear();
        }
    }
    public void NotifyEvent(int id, T arg)
    {

        List<eventDelegate<T>> eventList = null;
        if(eventManager.TryGetValue(id,out eventList))
        {
            for(int i = 0; i < eventList.Count; i++)
            {
                eventList[i](arg);
            }
        
        }

    }
    public void Clear()
    {
        if(eventManager.Count>0)
             eventManager.Clear();
    }

}
