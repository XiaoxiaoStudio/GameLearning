using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class statemachine : MonoBehaviour {

    public Dictionary<string, Istate> StateDic;

    public Istate Current;

    public Istate Next;

    public Istate Last;

    public statemachine()
    {
        StateDic = new Dictionary<string, Istate>();
    }

    /// <summary>
    /// 注册状态
    /// </summary>
    /// <param name="state"></param>
    /// <returns></returns>
    public bool RegistState(Istate state)
    {
        if (state == null)
        {
            return false;
        }

        if (StateDic == null)
        {
            StateDic = new Dictionary<string, Istate>();
        }

        if (!StateDic.ContainsKey(state.GetName()))
        {
            StateDic.Add(state.GetName(), state);
            return true;
        }
        return false;
    }
    /// <summary>
    /// 切换状态
    /// </summary>
    /// <param name="newStateName"></param>
    public void SwitchState(string newStateName)
    {
        if (!StateDic.ContainsKey(newStateName))
        {
            return;
        }
        if (Current != null)
        {
            if (newStateName == Current.GetName())
            {
                Current.Resume();
                return;
            }
        }
        Istate tempState = null;
        StateDic.TryGetValue(newStateName, out tempState);
        Next = tempState;
    }

	public void Update ()
    {
        if (Next != null)
        {
            if (Current != null)
            {
                Current.Leave();
            }
            Next.Start(this);
            Last = Current;
            Current = Next;
            Next = null;
        }
        if (Current != null)
        {
            Current.Resume();
        }
	}
}
