using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyState : IState
{
    StateMachine m_StateMachine;

    public FlyState(StateEnum m_State) : base(m_State)
    {
    }

    public override int GetStateID()
    {
        return (int)StateEnum.Fly;
    }

    public override void StartState(StateMachine stateMachine)
    {
        m_StateMachine = stateMachine;
        Debug.Log("已经入飞行状态,上次的状态为：" + m_StateMachine.LastState);
    }

    public override void UpdateState()
    {
        //Debug.Log("现在是等待状态");
    }

    public override void EndState()
    {
        Debug.Log("退出飞行状态,下个状态为：" + m_StateMachine.NextState);
    }
}
