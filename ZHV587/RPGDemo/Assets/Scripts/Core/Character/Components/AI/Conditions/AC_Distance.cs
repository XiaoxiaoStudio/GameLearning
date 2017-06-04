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
public class AC_Distance : AICondition
{
    public float Max;
    public float Min;
    public override bool IsSatisfied(AIMachine machine, AITrigger trigger)
    {
        if (machine.Character.Enemy == null) return false;
        float distance = machine.Character.DistanceToCharacter(machine.Character.Enemy);
        if (Min <= distance && distance < Max)
        {
            return true;
        }
        return false;
    }
    public override void ParseXML(SecurityElement element, AITrigger trigger)
    {
        base.ParseXML(element, trigger);
        float.TryParse(element.Attribute("Max"), out Max);
        float.TryParse(element.Attribute("Min"), out Min);
    }
}
