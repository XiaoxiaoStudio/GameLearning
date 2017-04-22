using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateMachine
{
    private StateType CurrenType;

    private StateBase mCurrentState;

    private StateBase mNextState;

    private Dictionary<StateType, StateBase> stateDic = null;

    private StateBase GetState(StateType type)
    {
        StateBase state = null;
        stateDic.TryGetValue(type, out state);
        return state;
    }

    private StateType GetStateType()
    {
        if ((int)CurrenType == 1)
        {
            return StateType.StateTwo;
        }
        else if ((int)CurrenType == 2)
        {
            return StateType.StateThree;
        }
        else if ((int) CurrenType == 3)
        {
            return StateType.StateOne;
        }
        else
        {
            return StateType.None;
        }
    }

    public StateMachine()
    {
        stateDic = new Dictionary<StateType, StateBase>();
        State1 state1 = new State1();
        stateDic.Add(state1.mCurrenType,state1);
        State2 state2 = new State2();
        stateDic.Add(state2.mCurrenType, state2);
        State3 state3 = new State3();
        stateDic.Add(state3.mCurrenType, state3);

        CurrenType = StateType.StateOne;
        mCurrentState = GetState(CurrenType);
    }
    //更新
    public void Action()
    {
        if (mNextState != null && mCurrentState != mNextState)
        {
            mCurrentState.StateEnd();
            mCurrentState = mNextState;
            mCurrentState.StateStart();
        }
        mCurrentState.StateUpdate();
    }

    public void Switch()
    {
        CurrenType = GetStateType();
        mNextState = GetState(CurrenType);
    }

}
