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
    public abstract int GetStateID();

    public StateEnum m_StateEnum;

    public IState(StateEnum m_State)
    {
        m_StateEnum = m_State;
    }


    public abstract void StartState(StateMachine stateMachine);

    public abstract void UpdateState();

    public abstract void EndState();
}
