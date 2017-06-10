using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainForm : UIBase
{
    public override void InitUIBase()
    {
        UIType = new UIType(WindowType.Fixed, ShowType.DoNothing, UIWindowID.MainForm);
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
}