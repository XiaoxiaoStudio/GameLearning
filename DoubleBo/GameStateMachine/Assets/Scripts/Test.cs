using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    private StateMachine m_StateMachine;

    // Use this for initialization
    void Start () {
        m_StateMachine = new StateMachine();

        m_StateMachine.RegistState(new FlyState(StateEnum.Fly.ToString()));
        m_StateMachine.RegistState(new WaitState(StateEnum.Wait.ToString()));

        m_StateMachine.SwitchState(StateEnum.Wait.ToString());
}
	
	// Update is called once per frame
	void Update () {
        m_StateMachine.OnUpdate();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_StateMachine.SwitchState(StateEnum.Fly.ToString());
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            m_StateMachine.SwitchState(StateEnum.Wait.ToString());
        }
    }
}
