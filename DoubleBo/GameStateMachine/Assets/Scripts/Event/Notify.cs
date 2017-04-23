using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notify : MonoBehaviour {

	void Update () {

		if(Input.GetKeyDown(KeyCode.Q))
        {
            EventManager<int>.GetSingle().NotifyEvent((int)EventType.event1, 5);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            EventManager<string>.GetSingle().NotifyEvent((int)EventType.event2, "你好。");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            EventManager<int>.GetSingle().NotifyEvent((int)EventType.event3, 1);
        }
    }
}
