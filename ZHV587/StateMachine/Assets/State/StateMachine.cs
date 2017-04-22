using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private Dictionary<string, IState> m_StateDic;

    private IState m_CurrState = null;

    private IState m_LastState = null;

    private IState m_NextState = null;

    public StateMachine()
    {
        m_StateDic = new Dictionary<string, IState>();
    }

    public void RegistState(IState state)
    {
        m_StateDic.Add(state.m_StateName, state);
    }

    public IState GetStateByName(string stateName)
    {
        IState tempState = null;
        if (m_StateDic.ContainsKey(stateName))
        {
            m_StateDic.TryGetValue(stateName, out tempState);
        }
        return tempState;
    }

    public void Refresh()
    {
        Debug.Log("已经进入了" + m_CurrState.ToString());
    }

    public void SwitchState(string stateName)
    {
        IState state = GetStateByName(stateName);
        
        if (state == null)
        {
            return;
        }
        
        if (m_CurrState == state)
        {
            Refresh();
            return;
        }
        
        m_NextState = state;

    }

    public void OnUpdate()
    {
        if (m_NextState != null)
        {
            if (m_CurrState != null)
            {
                m_CurrState.OnLeave();
            }

            m_NextState.OnEnter();

            m_LastState = m_CurrState;

            m_CurrState = m_NextState;
            m_NextState = null;
        }

        if (m_CurrState != null)
        {
            m_CurrState.OnUpdate();
        }
    }

    public void RemoveState(string stateName)
    {
        if (!m_StateDic.ContainsKey(stateName))
        {
            return;
        }

        m_StateDic.Remove(stateName);
    }
}
