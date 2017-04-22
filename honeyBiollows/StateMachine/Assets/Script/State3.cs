using UnityEngine;
using System.Collections;

public class State3 : StateBase {

    public State3()
    {
        mCurrenType = StateType.StateThree;
    }


    public override void StateStart()
    {
        Debug.Log("state3 Start");
    }

    public override void StateUpdate()
    {
        Debug.Log("state3 Update");
    }

    public override void StateEnd()
    {
        Debug.Log("state3 End");
    }
}
