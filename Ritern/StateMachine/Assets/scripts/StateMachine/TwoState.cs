using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoState : StateBase {

    public State state;
    public TwoState(State state) : base(state)
    {
        this.state = state;
    }
    public override void OnStart()
    {
        base.OnStart();
        Debug.Log("状态two开始");
    }
    public override void OnUpdate()
    {
        base.OnUpdate();
        Debug.Log("状态two执行中");
    }
    public override void OnEnd()
    {
        base.OnEnd();
        Debug.Log("状态two结束");
    }
}
