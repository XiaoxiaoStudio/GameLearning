using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Air2000;
using Mono.Xml;
using System.Security;
using System.Collections;

[Serializable]
public class AS_Attack_1 : AS_CommandState
{
    public AS_Attack_1() : base(CharacterCommand.CC_Attack_1)
    {

    }
    public override void Begin(AIMachine machine)
    {
        base.Begin(machine);
        if (machine.Commander.IsProcessing(CharacterCommand.CC_Attack_1) == false)
        {
            machine.Commander.ExecuteCommand(CharacterCommand.CC_Attack_1);
        }
    }
}
