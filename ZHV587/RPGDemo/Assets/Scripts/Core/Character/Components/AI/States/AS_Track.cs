using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Air2000;
using Mono.Xml;
using System.Security;
using System.Collections;
using UnityEngine;

[Serializable]
public class AS_Track : AIState
{
    public float StopRadius;
    public override void Begin(AIMachine machine)
    {
        base.Begin(machine);
        if (machine.Player.Enemy == null)
        {
            machine.ExecuteIdleState();
        }
        else
        {
            machine.Character.ExecuteCommand(CharacterCommand.CC_Walk);
        }
    }
    public override void Update(AIMachine machine)
    {
        base.Update(machine);
        if (machine.Player.Enemy == null || machine.Player.IsDie)
        {
            machine.ExecuteIdleState();
            return;
        }

        machine.Character.PathFinding.SetDestination(machine.Player.Enemy.Character.WorldPosition);

        float distanceToTarget = machine.Character.DistanceToCharacter(machine.Character.Enemy);
        if ((distanceToTarget - StopRadius) <= 0)
        {
            machine.ExecuteIdleState();
        }
    }
    public override void End(AIMachine machine)
    {
        base.End(machine);
    }
    public override void ParseXML(SecurityElement element, AIMachine machine)
    {
        base.ParseXML(element, machine);
        float.TryParse(element.Attribute("StopRadius"), out StopRadius);
    }
}
