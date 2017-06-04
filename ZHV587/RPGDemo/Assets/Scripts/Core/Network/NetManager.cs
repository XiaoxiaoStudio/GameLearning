using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Air2000
{
    public class NetManager : EventManager
    {
        private static NetManager m_Instance;

        private static Dictionary<int, NetConnection> m_Connections = new Dictionary<int, NetConnection>();

        public static NetManager Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new NetManager();
                }
                return m_Instance;
            }
        }

        private NetManager() : base() { }

        public static void Initialize()
        {
            m_Connections = new Dictionary<int, NetConnection>();
        }

        public static NetConnection ConnectTo(int type, string host, int post, NetConnection.StatusDelegate onConnected, NetConnection.StatusDelegate onDisconnected, NetConnection.StatusDelegate onReconnected, NetConnection.StatusDelegate onErrorOccupied)
        {
            if (m_Connections.ContainsKey(type))
            {
                DisconnectFrom(type);
            }
            NetConnection connection = new NetConnection(host, post, onConnected, onDisconnected, onReconnected, onErrorOccupied);
            connection.OnConnected = onConnected;
            connection.OnDisconnected = onDisconnected;
            connection.OnReconnected = onReconnected;
            connection.Connect();
            m_Connections.Add(type, connection);
            return connection;
        }

        public static void DisconnectFrom(int type)
        {
            NetConnection connection = null;
            if (m_Connections.TryGetValue(type, out connection))
            {
                connection.Disconnect();
                m_Connections.Remove(type);
            }
        }

        public static NetConnection GetConnection(int type)
        {
            NetConnection connection;
            m_Connections.TryGetValue(type, out connection);
            return connection;
        }

        public static void DisconnectAll()
        {
            Dictionary<int, NetConnection>.Enumerator it = m_Connections.GetEnumerator();
            for (int i = 0; i < m_Connections.Count; i++)
            {
                it.MoveNext();
                NetConnection connection = it.Current.Value;
                if (connection != null)
                {
                    connection.Disconnect();
                }
            }
            m_Connections.Clear();
        }

        public static void RegisterEvent(int id, EventHandlerDelegate func)
        {
            Instance.Register(id, func);
        }

        public static void UnregisterEvent(int id, EventHandlerDelegate func)
        {
            Instance.Unregister(id, func);
        }

        public static void SendEvent(int msgID, byte[] msgBuffer, int playerID, int serverID, int serverType = 1)
        {
            NetConnection connection = null;
            if (m_Connections.TryGetValue(serverType, out connection))
            {
                NetPacket packet = new NetPacket(msgID, msgBuffer.Length);
                packet.SetBody(msgBuffer);
                packet.SetPlayerID(playerID);
                packet.SetServerID(serverID);
                connection.Send(packet);
            }
        }

        public static void NotifyEvent(Evt evt)
        {
            Instance.Notify(evt);
        }




        public static void SendNetPacket<T>(int msgID, T obj)
    where T : class, ProtoBuf.IExtensible
        {
            //byte[] buffer = ProtoBuf.Serializer.Serialize<T>(obj);
            //NetConnection connection = null;
            //if (m_Connections.TryGetValue((int)ServerType.Logic, out connection))
            //{
            //    NetPacket packet = new NetPacket(msgID, msgBuffer.Length);
            //    packet.SetBody(msgBuffer);
            //    packet.SetPlayerID(playerID);
            //    packet.SetServerID(serverID);
            //    connection.Send(packet);
            //}
        }
        public static T DeserializeNetPacket<T>(Evt obj)
          where T : class, ProtoBuf.IExtensible
        {
            T returnObj = default(T);
            if (obj == null)
            {
                return returnObj;
            }
            EvtEx<NetPacket> eventObj = obj as EvtEx<NetPacket>;
            if (eventObj == null || eventObj.Param == null)
            {
                return returnObj;
            }
            using (var ms = new MemoryStream(eventObj.Param.GetBuffer(), NetPacket.PACK_HEAD_SIZE, eventObj.Param.GetTotalSize() - NetPacket.PACK_HEAD_SIZE))
            {
                return ProtoBuf.Serializer.Deserialize(typeof(T), ms) as T;
            }
        }
        public static void BindNetworkEvent(int id, EventHandlerDelegate handler)
        {
            Instance.Register(id, handler);
        }
        public static void UnbindNetworkEvent(int id, EventHandlerDelegate handler)
        {
            Instance.Unregister(id, handler);
        }

        #region lantency test 

        public class LantencyIno
        {
            public float ReqTime;
            public float RecvTime;
            public float DeltaTime
            {
                get
                {
                    return RecvTime - ReqTime;
                }
            }
        }

        private List<LantencyIno> m_ReceivedLatencyInfos = new List<LantencyIno>();
        private static int m_CurrentRecvCount = 0;
        private static readonly int TOTAL_SEND_COUNT = 5;
        public static int CurrentLatency;
        private void InitLatencyTest()
        {
            GTimer.In(5.0f, SendLatencyTest, int.MaxValue);
            BindNetworkEvent((int)AccountMessage.GO_TEST_LATENCY, OnRecvLatencyTest);
        }
        private void SendLatencyTest()
        {
            m_ReceivedLatencyInfos.Clear();
            m_CurrentRecvCount = 0;
            for (int i = 0; i < TOTAL_SEND_COUNT; i++)
            {
                LantencyIno info = new LantencyIno();
                info.ReqTime = Time.realtimeSinceStartup;
                m_ReceivedLatencyInfos.Add(info);
                SendNetPacket((int)AccountMessage.GO_TEST_LATENCY, new PBMessage.go_time_return() { nowtime = i });
            }
        }
        private void OnRecvLatencyTest(Evt obj)
        {
            if (m_CurrentRecvCount < m_ReceivedLatencyInfos.Count)
            {
                m_ReceivedLatencyInfos[m_CurrentRecvCount].RecvTime = Time.realtimeSinceStartup;
            }
            m_CurrentRecvCount++;
            if (m_CurrentRecvCount == TOTAL_SEND_COUNT)
            {
                CalculateLatency();
            }
        }
        private void CalculateLatency()
        {
            if (m_ReceivedLatencyInfos.Count == 0 || m_ReceivedLatencyInfos == null)
            {
                return;
            }
            float delay = 0.0f;
            for (int i = 0; i < m_ReceivedLatencyInfos.Count; i++)
            {
                delay += m_ReceivedLatencyInfos[i].DeltaTime;
            }
            if (delay == 0.0f)
            {
                return;
            }
            CurrentLatency = (int)((delay / (m_ReceivedLatencyInfos.Count * 2.0f)) * 1000);
            //Debug.LogError("CurrentLatency: " + CurrentLatency);
        }
        #endregion
    }
}