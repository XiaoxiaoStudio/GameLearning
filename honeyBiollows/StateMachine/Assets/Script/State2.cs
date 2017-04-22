using UnityEngine;
using System.Collections;

public class State2 : StateBase {

    public State2()
    {
        mCurrenType = StateType.StateTwo;
    }

    public override void StateStart()
    {
        Debug.Log("state2 Start");
    }

    public override void StateUpdate()
    {
        Debug.Log("state2 Update");
    }

    public override void StateEnd()
    {
        Debug.Log("state2 End");
    }
}
