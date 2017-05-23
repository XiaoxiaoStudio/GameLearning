using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPrintLog : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.W))
        {
            PrintAndSavaLogInfo.GetSingle().Print_M("你是猪吗？");
        }
	}
}
