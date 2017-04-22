using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    private StateMachine m_StateMachine;

    void Start()
    {
        StartCoroutine(TestFunc());
    }

    void Update()
    {
        if (m_StateMachine != null)
        {
            m_StateMachine.Update();
        }
    }

    IEnumerator TestFunc()
    {
        yield return StartCoroutine(Initialize());
        yield return StartCoroutine(SwitchState());
    }

    IEnumerator Initialize()
    {
        m_StateMachine = new StateMachine();
        m_StateMachine.Register(new State("State1"));
        m_StateMachine.Register(new State("State2"));
        m_StateMachine.Register(new State("State3"));
        yield return 0;
    }

    IEnumerator SwitchState()
    {
        m_StateMachine.SetNext("State1");
        yield return new WaitForSeconds(1);
        m_StateMachine.SetNext("State1");

        yield return new WaitForSeconds(2);
        m_StateMachine.SetNext("State2");

        yield return new WaitForSeconds(2);
        m_StateMachine.SetNext("State3");
    }
}
