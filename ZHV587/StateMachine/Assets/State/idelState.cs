using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class idelState : IState
{
    public idelState(string stateName) : base(stateName)
    {
    }

    public override void OnLeave()
    {
        Debug.Log("idelState + EndState()");
    }

    public override void OnEnter()
    {
        Debug.Log("idelState + StartState()");
    }

    public override void OnUpdate()
    {
        Debug.Log("idelState + Update()");
    }
}
