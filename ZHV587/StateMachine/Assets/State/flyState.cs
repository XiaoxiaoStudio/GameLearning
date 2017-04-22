using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyState : IState
{
    public flyState(string stateName) : base(stateName)
    {
    }

    public override void OnLeave()
    {
        Debug.Log("m_flyState + EndState()");
    }

    public override void OnEnter()
    {
        Debug.Log("m_flyState + StartState()");
    }

    public override void OnUpdate()
    {
        Debug.Log("m_flyState + Update()");
    }
}
