using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    private Dictionary<int, IState> m_StateDict;

    public IState CurState;

    public IState NextState;

    public IState LastState;

    public StateMachine()
    {
        m_StateDict = new Dictionary<int, IState>();
    }

    /// <summary>
    /// 注册状态
    /// </summary>
    /// <param name="state"></param>
    public void RegistState(IState state)
    {
        if (!m_StateDict.ContainsKey((int)state.m_StateEnum))
        {
            m_StateDict.Add((int)state.m_StateEnum, state);
        }
    }

    /// <summary>
    /// 获取这个状态
    /// </summary>
    /// <param name="stateId"></param>
    /// <returns></returns>
    public IState GetState(int stateId)
    {
        IState state = null;
        m_StateDict.TryGetValue(stateId, out state);
        return state;
    }
    
    /// <summary>
    /// 切换这个状态
    /// </summary>
    /// <param name="newStateId"></param>
    /// <returns></returns>
    public bool SwitchState(int newStateId)
    {
        if (!m_StateDict.ContainsKey(newStateId))
        {
            return false;
        }

        //是否要切换的是现有状态
        if (CurState.GetStateID() == newStateId)
        {
            return false;
        }

        IState m_NewState = null;
        m_StateDict.TryGetValue(newStateId, out m_NewState);
        //if (null == m_NewState)
        //{
        //    return false;
        //}

        //IState m_OldState = CurState;

        //if (m_OldState != null)
        //{
        //    m_OldState.EndState();
        //}
        //LastState = m_OldState;
        NextState = m_NewState;
        return true;
    }

    /// <summary>
    /// 获取现有状态
    /// </summary>
    /// <returns></returns>
    public IState GetCurState()
    {
        return CurState;
    }

    ///// <summary>
    ///// 获取现有状态ID
    ///// </summary>
    ///// <returns></returns>
    //public int GetCurStateId()
    //{
    //    IState state = GetCurState();
    //    return (null == state) ? 0 : state.GetStateID();
    //}

    public void OnUpdate()
    {
        if(NextState!=null)
        {
            CurState = NextState;
            NextState = null;
        }
        if (CurState != null)
        {
            CurState.UpdateState();
        }
    }
}
