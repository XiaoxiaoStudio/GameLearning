using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test2 : UIBase
{
    public override void InitUIBase()
    {
        UIType = new UIType(WindowType.Normal, ShowType.DoNothing, UIWindowID.test2);
    }
}