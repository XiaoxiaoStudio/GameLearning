using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Notify : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.E))
        {
            EventManager<Damage>.GetIntance().NotifyEvent((int)EventType.eventOne, null);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            EventManager<Damage>.GetIntance().NotifyEvent((int)EventType.eventTwo, new Damage(5));
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            EventManager<int>.GetIntance().NotifyEvent((int)EventType.eventThree, 15);
        }
    }
}
