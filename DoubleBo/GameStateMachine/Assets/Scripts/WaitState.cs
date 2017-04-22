using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitState : IState
{
    StateMachine m_StateMachine;

    public WaitState(StateEnum m_State) : base(m_State)
    {
    }
    

    public override int GetStateID()
    {
        return (int)StateEnum.Wait;
    }

    public override void StartState(StateMachine stateMachine)
    {
        m_StateMachine = stateMachine;
        Debug.Log("已经入等待状态,上次的状态为：" + m_StateMachine.LastState);
    }

    public override void UpdateState()
    {
        Debug.Log("等待状态");
    }

    public override void EndState()
    {
        Debug.Log("退出等待状态,下一状态为：" + m_StateMachine.NextState);
    }
}
