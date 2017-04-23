using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class flyState : Istate
{
    private statemachine m_statemachine;

    public flyState(string name) : base(name)
    {
    }

    public override string GetName()
    {
        return StateType.fly.ToString();
    }

    public override void Start(statemachine statemachine)
    {
        m_statemachine = statemachine;
        Debug.Log(GetName() + "Start，framecount is" + Time.frameCount);
    }

    public override void Update()
    {

    }

    public override void Leave()
    {
        Debug.Log(GetName() + "Leave，framecount is" + Time.frameCount);
    }

    public override void Resume()
    {
        Debug.Log(GetName() + "Resume，framecount is" + Time.frameCount);
    }
}
