using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum testEnum
{
    event1,
    event2,
    event3,
    event4,
    event5,
}

public class testEvent : MonoBehaviour {

	// Use this for initialization
	void Start () {
        EventManager<int>.GetSingle().RegistEvent((int)testEnum.event1, test1);
        EventManager<int>.GetSingle().RegistEvent((int)testEnum.event1, test2);
        EventManager<string>.GetSingle().RegistEvent((int)testEnum.event2, test3);
    }
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Q))
        {
            EventManager<int>.GetSingle().NotifyEvent((int)testEnum.event1, 5);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            EventManager<string>.GetSingle().NotifyEvent((int)testEnum.event2, "qwertry");
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            EventManager<int>.GetSingle().UnRegist((int)testEnum.event1);
        }
    }

    public void test1(int q)
    {
        Debug.Log("test1"+q);
    }
    public void test2(int q)
    {
        Debug.Log("test2" +q);
    }
    public void test3(string s)
    {
        Debug.Log("test3"+s);
    }
    public void test4()
    {
        Debug.Log("test4");
    }
    public void test5()
    {
        Debug.Log("test5");
    }

}
