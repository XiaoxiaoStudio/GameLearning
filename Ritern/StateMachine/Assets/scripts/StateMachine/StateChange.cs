using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateChange : MonoBehaviour {
    OneState one;
    TwoState two;
	// Use this for initialization
	void Start () {
        one = new OneState(State.one);
        two = new TwoState(State.two);
        StateManage.GetIntance().RegisterState(one.state, one);
        StateManage.GetIntance().RegisterState(two.state, two);
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StateManage.GetIntance().ChangeState(State.one);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            StateManage.GetIntance().ChangeState(State.two);
        }
        StateManage.GetIntance().OnUpdate();

    }
}
