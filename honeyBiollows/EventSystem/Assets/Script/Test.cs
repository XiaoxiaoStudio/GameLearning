using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    // Update is called once per frame
    // Use this for initialization
    void Start()
    {
        EventSystemManager<int>.Instance.Register((int)GameEventType.One, One);
        EventSystemManager<string>.Instance.Register((int)GameEventType.Two, Two);
        EventSystemManager<long>.Instance.Register((int)GameEventType.Three, Three);
    }

    void One(int param)
    {
        Debug.Log("类型是:" + typeof(int) + "参数是:" + param);
    }

    void Two(string param)
    {
        Debug.Log("类型是:" + typeof(string) + "参数是:" + param);
    }

    void Three(long param)
    {
        Debug.Log("类型是:" + typeof(long) + "参数是:" + param);
    }

    void Update () {

        if (Input.GetKeyDown(KeyCode.Q))
        {
            EventSystemManager<int>.Instance.Notification((int)GameEventType.One, 100);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            EventSystemManager<string>.Instance.Notification((int)GameEventType.Two, "hello word");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            EventSystemManager<long>.Instance.Notification((int)GameEventType.Three, 10000);
        }

    }
   
}
