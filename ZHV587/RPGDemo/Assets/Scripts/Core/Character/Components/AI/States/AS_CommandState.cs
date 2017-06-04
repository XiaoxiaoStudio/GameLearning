using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Air2000;
using Mono.Xml;
using System.Security;
using System.Collections;

[Serializable]
public class AS_CommandState : AIState
{
    public CharacterCommand Command;
    public AS_CommandState(CharacterCommand command)
    {
        Command = command;
    }
    public override void Begin(AIMachine machine)
    {
        base.Begin(machine);
        if (ForceStop == false)
        {
            if (machine.Commander.IsProcessing(Command) == false)
            {
                machine.Commander.ExecuteCommand(Command);
            }
        }
        else
        {
            machine.Commander.ExecuteCommand(Command);
        }
    }
    public override void End(AIMachine machine)
    {
        base.End(machine);
    }
    public override void ParseXML(SecurityElement element, AIMachine machine)
    {
        base.ParseXML(element, machine);
    }
}
