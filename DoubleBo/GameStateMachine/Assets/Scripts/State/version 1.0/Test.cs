using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour {

    private StateMachine m_StateMachine;

    private StateMachine1 m_StateMachine1;

    // Use this for initialization
    void Start () {
        //m_StateMachine = new StateMachine();
        m_StateMachine1 = new StateMachine1();

        m_StateMachine1.Register(new FlyState1(StateType1.Fly.ToString()));
        m_StateMachine1.Register(new WaitState1(StateType1.Wait.ToString()));

        m_StateMachine1.SwitchState(StateType1.Wait.ToString());

        //m_StateMachine.RegistState(new FlyState(StateEnum.Fly.ToString()));
        //m_StateMachine.RegistState(new WaitState(StateEnum.Wait.ToString()));

        //m_StateMachine.SwitchState(StateEnum.Wait.ToString());
}
	
	// Update is called once per frame
	void Update () {

        m_StateMachine1.Update();

        if (Input.GetKeyDown(KeyCode.E))
        {
            m_StateMachine1.SwitchState(StateType1.Fly.ToString());
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            m_StateMachine1.SwitchState(StateType1.Wait.ToString());
        }

        //m_StateMachine.OnUpdate();
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    m_StateMachine.SwitchState(StateEnum.Fly.ToString());
        //}
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    m_StateMachine.SwitchState(StateEnum.Wait.ToString());
        //}
    }
}
