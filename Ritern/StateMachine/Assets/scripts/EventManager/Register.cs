using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Register : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {
        EventManager<Damage>.GetIntance().RigisterEvent((int)EventType.eventOne, OneFuction);
        EventManager<Damage>.GetIntance().RigisterEvent((int)EventType.eventTwo, TwoFuction);
        EventManager<int>.GetIntance().RigisterEvent((int)EventType.eventThree, ThreeFuction);
    }

    private void ThreeFuction(int arg)
    {

        Debug.Log("事件two发生");
        Debug.Log("伤害+" + arg);
    }

    private void TwoFuction(Damage arg)
    {

        Debug.Log("事件two发生");
        Debug.Log("伤害+" + arg.beat);
    }

    private void OneFuction(Damage arg)
    {
        Debug.Log("事件one发生");

    }

    // Update is called once per frame
    void Update()
    {

    }
}
