using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State
{
    public string Name;

    public State(string name)
    {
        Name = name;
    }

    public void Start()
    {
        Debug.Log(Name + " start,framecount is " + Time.frameCount);
    }

    public void Resume()
    {
        Debug.Log(Name + " resume,framecount is " + Time.frameCount);
    }

    public void Update()
    {

    }

    public void End()
    {
        Debug.Log(Name + " end,framecount is " + Time.frameCount);
    }
}
