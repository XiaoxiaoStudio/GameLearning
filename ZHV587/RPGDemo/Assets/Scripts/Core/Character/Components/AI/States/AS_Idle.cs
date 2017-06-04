using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Air2000;
using Mono.Xml;
using System.Security;
using System.Collections;

public class AS_Idle : AS_CommandState
{
    public AS_Idle() : base(CharacterCommand.CC_Idle) { }
    public override void Begin(AIMachine machine)
    {
        base.Begin(machine);
    }
}
