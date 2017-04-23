using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager<int>.GetSingle().Register((int)EventType.event1, Event1);
        EventManager<string>.GetSingle().Register((int)EventType.event2, Event2);
        EventManager<int>.GetSingle().Register((int)EventType.event3, Event3);
    }

    public void Event1(int a)
    {
        Debug.Log("a = " + a);
    }

    public void Event2(string b)
    {
        Debug.Log("B = " + b);
    }

    public void Event3(int b)
    {
        Debug.Log("这是事件3。");
    }
}
