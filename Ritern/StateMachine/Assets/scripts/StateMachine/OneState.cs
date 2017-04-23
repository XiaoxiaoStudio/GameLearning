using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneState : StateBase
{
    public State state;
    public OneState(State state) : base(state)
    {
        this.state = state;
    }
    public override void OnStart()
    {
        base.OnStart();
        Debug.Log("状态one开始");
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        Debug.Log("状态one执行中");
    }
    public override void OnEnd()
    {
        base.OnEnd();
        Debug.Log("状态one结束");
    }
}
