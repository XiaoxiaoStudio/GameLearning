using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public Dictionary<string, IState> StateDict;

    public IState CurState;

    public IState NextState;

    public IState LastState;

    public StateMachine()
    {
        StateDict = new Dictionary<string, IState>();
    }

    /// <summary>
    /// 注册状态
    /// </summary>
    /// <param name="state"></param>
    public bool RegistState(IState state)
    {
        if (state == null)// || string.IsNullOrEmpty(state.m_StateEnum.ToString())
        {
            return false;
        }

        if (StateDict == null)
        {
            StateDict = new Dictionary<string, IState>();
        }

        if (!StateDict.ContainsKey(state.m_StateEnum.ToString()))
        {
            StateDict.Add(state.m_StateEnum.ToString(), state);
            return true;
        }
        return false;
    }

    /// <summary>
    /// 解除状态的注册
    /// </summary>
    /// <param name="stateName"></param>
    public void RemoveState(string stateName)
    {
        if (!StateDict.ContainsKey(stateName))
        {
            return;
        }
        StateDict.Remove(stateName);
    }

    public void Refash()
    {
        Debug.Log("qqqqq");
    }

    /// <summary>
    /// 切换这个状态
    /// </summary>
    /// <param name="newStateId"></param>
    /// <returns></returns>
    public void SwitchState(string newStateName)
    {
        IState state = GetStateByName(newStateName);

        //判断该状态是否注册
        if (!StateDict.ContainsKey(newStateName))
        {
            return;
        }
        //是否要切换的是现有状态
        if (CurState != null)
        {
            if (CurState.GetStateName() == newStateName)
            {
                Refash();
                return;
            }
        }
        
        NextState = state;
    }

    /// <summary>
    /// 获取状态
    /// </summary>
    /// <returns></returns>
    public IState GetStateByName(string stateName)
    {
        IState tempState = null;
        if (StateDict.ContainsKey(stateName))
        {
            StateDict.TryGetValue(stateName, out tempState);
        }
        return tempState;
    }
    
    public void OnUpdate()
    {
        if(NextState!=null)
        {
            if (CurState != null)
            {
                CurState.End();
            }
            NextState.Start(this);
            CurState = NextState;
            NextState = null;
        }
        if (CurState != null)
        {
            CurState.Update();
        }
    }

}
