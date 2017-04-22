using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
    public StateMachine m_StateMachine;
    // Use this for initialization
    void Start () {
        m_StateMachine = new StateMachine();
        m_StateMachine.RegistState(new idelState(StateEnum.idel.ToString()));
        m_StateMachine.RegistState(new flyState(StateEnum.fly.ToString()));
    }
	
	// Update is called once per frame
	void Update () {
        m_StateMachine.OnUpdate();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            m_StateMachine.SwitchState(StateEnum.idel.ToString());
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            m_StateMachine.SwitchState(StateEnum.fly.ToString());
        }
    }
}
