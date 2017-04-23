using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour
{
    private statemachine m_statemachine;
    
    void Start()
    {
        m_statemachine = new statemachine();
        m_statemachine.RegistState(new waitState(StateType.wait.ToString()));
        m_statemachine.RegistState(new flyState(StateType.fly.ToString()));

        m_statemachine.SwitchState(StateType.wait.ToString());
    }

    void Update()
    {
        m_statemachine.Update();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_statemachine.SwitchState(StateType.fly.ToString());
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            m_statemachine.SwitchState(StateType.wait.ToString());
        }
    }
}
