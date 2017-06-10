using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test1 : UIBase
{
    public override void InitUIBase()
    {
        UIType = new UIType(WindowType.Fixed, ShowType.HideOther, UIWindowID.test1);
    }

    public override void OnAwake()
    {
        base.OnAwake();
    }

    public void opentest2()
    {
        UIBaseManager.GetInstance.ShowWindow(UIWindowID.test2);
    }
}