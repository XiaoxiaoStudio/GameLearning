using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateEnum
{
    None,
    Fly,
    Wait
}

public abstract class IState
{
    public abstract string GetStateName();

    public string m_StateEnum;

    public IState(string name)
    {
        m_StateEnum = name;
    }
    
    public abstract void Start(StateMachine stateMachine);

    public abstract void Update();

    public abstract void End();
}
