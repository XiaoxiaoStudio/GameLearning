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
public class AS_SearchEnemy : AIState
{
    public override void Begin(AIMachine machine)
    {
        base.Begin(machine);
        Player tempEnemy = null;
        if (BattleScene.Instace != null)
        {
            //BattleScene.Instace.GetNearestEnemy(machine.Player.Character.WorldPosition, machine.Player, out tempEnemy);
        }
        machine.Player.Enemy = tempEnemy;
        machine.ExecuteIdleState();
    }
}
