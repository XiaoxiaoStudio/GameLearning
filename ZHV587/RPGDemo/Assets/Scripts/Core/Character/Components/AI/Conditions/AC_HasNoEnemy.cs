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
public class AC_HasNoEnemy : AICondition
{
    public override bool IsSatisfied(AIMachine machine, AITrigger trigger)
    {
        if (machine.Player.Enemy == null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
