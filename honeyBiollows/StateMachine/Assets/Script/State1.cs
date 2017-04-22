using UnityEngine;
using System.Collections;

public class State1 : StateBase {

    public State1()
    {
        mCurrenType = StateType.StateOne;
    }

    public override void StateStart()
    {
        Debug.Log("state1 Start");
    }
    public override void StateUpdate()
    {
        Debug.Log("state1 Update");

    }
    public override void StateEnd()
    {
        Debug.Log("state1 End");
    }
}
