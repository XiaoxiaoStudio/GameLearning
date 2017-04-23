using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyState : IState
{
    StateMachine2 m_StateMachine;

    public FlyState(string name) : base(name)
    {
    }
    /// <summary>
    /// 返回值为状态名
    /// </summary>
    /// <returns></returns>
    public override string GetStateName()
    {
        return StateEnum.Fly.ToString();
    }

    public override void Start(StateMachine2 stateMachine)
    {
        m_StateMachine = stateMachine;
        Debug.Log(GetStateName() + "Start，framecount is" + Time.frameCount);
    }

    public override void Update()
    {
        Debug.Log(GetStateName() + "Update，framecount is" +Time.frameCount);
    }

    public override void End()
    {
        Debug.Log(GetStateName() + "End，framecount is" + Time.frameCount);
    }
}
