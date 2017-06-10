using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arena : UIBase
{
    public override void InitUIBase()
    {
        UIType = new UIType(WindowType.Normal, ShowType.HideOther, UIWindowID.arena);
    }

    public void OpenKanpsack()
    {
        UIBaseManager.GetInstance.ShowWindow(UIWindowID.kanpsack);
    }

    public void OpenHero()
    {
        UIBaseManager.GetInstance.ShowWindow(UIWindowID.hero);
    }

    public void OpenShop()
    {
        UIBaseManager.GetInstance.ShowWindow(UIWindowID.shop);
    }

    public void Openarena()
    {
        UIBaseManager.GetInstance.ShowWindow(UIWindowID.arena);
    }

    public void CloseThisWindow()
    {
        UIBaseManager.GetInstance.CloseWindow(UIWindowID.arena);
    }
}