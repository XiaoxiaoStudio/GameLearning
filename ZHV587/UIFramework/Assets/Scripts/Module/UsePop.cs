using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UsePop : UIBase
{
    public override void InitUIBase()
    {
        UIType = new UIType(WindowType.Pop, ShowType.DoNothing, UIWindowID.UsePop);
    }

    public void Sur()
    {
        UIBaseManager.GetInstance.CloseWindow(UIWindowID.UsePop);
    }

    public void UnSur()
    {
        UIBaseManager.GetInstance.CloseWindow(UIWindowID.UsePop);
    }
}