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
    }

    public void RigisterEvent(int command, eventDelegate<T> fuction)
    {
        if (fuction == null)
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
                if (fuction == eventList[i])
                {
                    eventList.Remove(fuction);
                }
            }
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
