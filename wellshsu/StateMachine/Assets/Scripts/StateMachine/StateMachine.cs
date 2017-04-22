using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine
{
    public Dictionary<string, State> States;

    public State Last;

    public State Current;

    public State Next;

    public bool Register(State state)
    {
        if (state == null || string.IsNullOrEmpty(state.Name))
        {
            return false;
        }
        if (States == null)
        {
            States = new Dictionary<string, State>();
        }
        if (States.ContainsKey(state.Name) == false)
        {
            States.Add(state.Name, state);
            return true;
        }
        return false;
    }

    public void Update()
    {
        if (Next != null)
        {
            Last = Current;
            Current = Next;
            Next = null;
            if (Last != null)
            {
                Last.End();
            }
            Current.Start();
        }
        if (Current != null)
        {
            Current.Update();
        }
    }

    public bool SetNext(string name)
    {
        if (Current != null && Current.Name == name)
        {
            Current.Resume();
            return true;
        }
        State state = null;
        if (States.TryGetValue(name, out state))
        {
            Next = state;
            Debug.Log("Set next state,framecount is " + Time.frameCount);
            return true;
        }
        return false;
    }
}
