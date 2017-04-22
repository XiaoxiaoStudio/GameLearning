using UnityEngine;
using System.Collections;

public enum StateType
{
    None = 0,StateOne = 1,StateTwo = 2,StateThree = 3
}
public class StateBase
{
    public StateType mCurrenType;

    public virtual void StateStart() { }

    public virtual  void StateUpdate() { }

    public virtual void StateEnd() { }
}
