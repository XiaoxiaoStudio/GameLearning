using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateManager : MonoBehaviour
{
    private StateMachine _stateMachine;
    void Start()
    {
        _stateMachine = new StateMachine();
    }

    void Update()
    {
        _stateMachine.Action();
        if (Input.GetMouseButtonDown(0))
        {
            _stateMachine.Switch();
        }
    }
}
