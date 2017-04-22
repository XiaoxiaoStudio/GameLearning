using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateEnum
{
    None,
    idel,
    fly,
}

public abstract class IState
{
    public string m_StateName;

    public IState(string stateName)
    {
        m_StateName = stateName;
    }

    public virtual void OnEnter()
    {
    }
    
    public virtual void OnEnter(params object[] param)
    {
    }

    public abstract void OnUpdate();

    public virtual void OnLeave()
    {
    }
    
    public virtual void OnLeave(params object[] param)
    {
    }

}
