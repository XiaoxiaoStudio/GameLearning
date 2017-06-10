using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        UIBaseManager.GetInstance.ShowWindow(UIWindowID.MainForm);
    }

    // Update is called once per frame
    private void Update()
    {
    }
}