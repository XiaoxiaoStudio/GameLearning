using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum State
{
    one,
    two
}
public class StateBase {
    public State state;
    public StateBase(State state)
    {
        this.state = state;
    }

    public virtual void OnStart() { }
    public virtual void OnUpdate() { }
    public virtual void OnEnd() { }

}
