using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManage{
    private static StateManage intance;
    Dictionary<State, StateBase> stateManage;
    StateBase lastState = null;
    StateBase curState = null;
    StateBase nextState = null;

    public static StateManage GetIntance()
    {
        if (intance == null)
        {
            intance = new StateManage();
        }
        return intance;
    }
    public StateManage()
    {
        stateManage = new Dictionary<State, StateBase>();
    }

    public bool RegisterState(State state,StateBase statebase)
    {
        if (stateManage == null)
            stateManage = new Dictionary<State, StateBase>();
        StateBase team;
        if(stateManage.TryGetValue(state,out team) == false)
        {
            stateManage.Add(state, statebase);
            return true;
        }
        return false;
    }
    public bool ChangeState(State state)
    {
        if (stateManage.ContainsKey(state))
        {
            nextState = stateManage[state];
            lastState = curState;
            return true;
        }
        return false;
    }

    public void OnUpdate()
    {
        if (nextState != null)
        {
            if (curState != null)
            {
                curState.OnEnd();
            }
            curState = nextState;
            nextState = null;
            curState.OnStart();
        }
        if (curState != null)
        {
            curState.OnUpdate();
        }
    }

}
