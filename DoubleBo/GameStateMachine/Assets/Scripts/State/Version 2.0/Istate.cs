using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StateType
{
    none,
    fly,
    wait
}

public abstract class Istate
{
    public abstract string GetName();

    public string Name;

    public Istate(string name)
    {
        Name = name;
    }

    public abstract void Start(statemachine statemachine);

    public abstract void Resume();

    public abstract void Update();

    public abstract void Leave();

}
