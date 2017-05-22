using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class Game : MonoBehaviour
{
    // Use this for initialization
    void Start()
    {
        NetManager.RegisterEvent(99, (evt) =>
        {
            Debug.Log("Recv a msg success.");
            MemoryStream ms = new MemoryStream(evt.Param as byte[]);
            PBMessage.B msg = ProtoBuf.Serializer.Deserialize<PBMessage.B>(ms);
            ms.Close();
        });
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 150, 30), "Connect To Server"))
        {
            NetManager.ConnectTo(1, "127.0.0.1", 8885,
            (con, arg) =>
            {
                Debug.Log("onConnected");
            },
            (con, arg) =>
            {
                Debug.Log("onDisconnected");
            },
            (con, arg) =>
            {
                Debug.Log("onReconnected");
            },
            (con, arg) =>
            {
                Debug.Log("onErrorOccupied: " + arg.ToString());
            });
        }
        if (GUI.Button(new Rect(10, 50, 150, 30), "Send NetPacket"))
        {
            PBMessage.B b = new PBMessage.B();
            b.list.Add(new PBMessage.A());
            b.list.Add(new PBMessage.A());
            b.list.Add(new PBMessage.A());

            MemoryStream ms = new MemoryStream();
            ProtoBuf.Serializer.Serialize<PBMessage.B>(ms, b);
            NetManager.SendEvent(99, ms.ToArray(), 10086, 1, 1);
            ms.Close();
        }
    }
}
