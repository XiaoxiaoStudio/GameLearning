﻿using System;
using System.Net.Sockets;
using System.Net;

namespace Air2000
{
    public class NetConnection
    {
        public enum CallbackType
        {
            OnConnected,
            OnDisconnected,
            OnReconnected,
            OnErrorrOccurred,
        }

        public delegate void StatusDelegate(NetConnection connection, object param);
        public string Host;
        public int Port;
        public Socket Socket;
        public string Error;
        public float ReconnectInterval = 1f;
        public byte[] Header = new byte[NetPacket.PACK_HEAD_SIZE];
        public StatusDelegate OnConnected;
        public StatusDelegate OnDisconnected;
        public StatusDelegate OnReconnected;
        public StatusDelegate OnErrorOccurred;

        public NetConnection(string host, int port, StatusDelegate onConnected, StatusDelegate onDisconnected, StatusDelegate onReconnected, StatusDelegate onErrorOccurred)
        {
            Host = host;
            Port = port;
            OnConnected = onConnected;
            OnDisconnected = onDisconnected;
            OnReconnected = onReconnected;
            OnErrorOccurred = onErrorOccurred;
        }

        public bool IsConnected
        {
            get
            {
                if (Socket == null)
                {
                    return false;
                }
                else
                {
                    return Socket.Connected;
                }
            }
        }

        public void Connect()
        {
            try
            {
                IPAddress[] addresses = Dns.GetHostAddresses(Host);
                IPEndPoint remoteEP = new IPEndPoint(addresses[0], Port);
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket.BeginConnect(remoteEP, ConnectCallback, this);
            }
            catch (Exception e)
            {
                ErrorrOccurred(e.Message);
            }
        }

        public void Reconnect()
        {
            ExecuteReconnect();
        }

        public void Disconnect()
        {
            if (Socket != null)
            {
                if (IsConnected)
                {
                    Socket.Shutdown(SocketShutdown.Both);
                }
                Socket.Close();
            }
            Callback(CallbackType.OnDisconnected);
        }

        public void Send(NetPacket packet)
        {
            if (IsConnected)
            {
                Socket.BeginSend(packet.GetBuffer(), 0, packet.GetTotalSize(), SocketFlags.None, new AsyncCallback(SendCallback), this);
            }
            else
            {
                Helper.LogError("Can not send data,caused by not connected.");
            }
        }

        private void Callback(CallbackType type, object param = null)
        {
            StatusDelegate func = null;
            switch (type)
            {
                case CallbackType.OnConnected:
                    func = OnConnected;
                    break;

                case CallbackType.OnDisconnected:
                    func = OnDisconnected;
                    break;
                case CallbackType.OnReconnected:
                    func = OnReconnected;
                    break;
                case CallbackType.OnErrorrOccurred:
                    func = OnErrorOccurred;
                    break;
                default:
                    break;
            }
            Loom.QueueOnMainThread(() =>
            {
                if (func != null)
                {
                    func(this, param);
                }
            });
        }

        private void ErrorrOccurred(string error)
        {
            Error = error;
            if (Socket != null)
            {
                if (IsConnected)
                {
                    Socket.Shutdown(SocketShutdown.Both);
                }
                Socket.Close();
            }
            Callback(CallbackType.OnErrorrOccurred, error);
        }

        private void RepeatReconnect()
        {
            Loom.QueueOnMainThread(() =>
            {
                ExecuteReconnect();
            }, ReconnectInterval);
        }

        private void ExecuteReconnect()
        {
            try
            {
                IPAddress[] addresses = Dns.GetHostAddresses(Host);
                IPEndPoint remoteEP = new IPEndPoint(addresses[0], Port);
                Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                Socket.BeginConnect(remoteEP, ReconnectCallback, this);
            }
            catch
            {
                RepeatReconnect();
            }
        }

        private void ConnectCallback(IAsyncResult result)
        {
            try
            {
                Socket.EndConnect(result);
                Callback(CallbackType.OnConnected);
                StartReceivePacketHeader();
            }
            catch (Exception e)
            {
                ErrorrOccurred(e.Message);
            }
        }

        private void ReconnectCallback(IAsyncResult result)
        {
            try
            {
                Socket.EndConnect(result);
                Callback(CallbackType.OnReconnected);
                StartReceivePacketHeader();
            }
            catch
            {
                RepeatReconnect();
            }
        }

        private void StartReceivePacketHeader()
        {
            Socket.BeginReceive(Header, 0, Header.Length, SocketFlags.None, new AsyncCallback(ReceivePacketHeaderCallback), this);
        }

        private void StartReceivePacketBody(NetPacket packet)
        {
            Socket.BeginReceive(packet.GetBody(), 0, packet.GetBodySize(), SocketFlags.None, new AsyncCallback(ReceivePacketBodyCallback), packet);
        }

        private void ReceivePacketHeaderCallback(IAsyncResult result)
        {
            try
            {
                int bytesRead = Socket.EndReceive(result);
                if (bytesRead > 0)
                {
                    int msgID = BitConverter.ToInt32(Header, NetPacket.PACK_MESSAGEID_OFFSET);
                    int bufferSize = BitConverter.ToInt32(Header, NetPacket.PACK_LENGTH_OFFSET);
                    int bodySize = bufferSize - NetPacket.PACK_HEAD_SIZE;
                    NetPacket packet = new NetPacket(msgID, bodySize);
                    packet.SetHeader(Header);
                    if (bodySize <= 0)
                    {
                        Loom.QueueOnMainThread(() =>
                        {
                            NetManager.NotifyEvent(new Evt(packet.GetMessageID(), packet.GetBody()));
                        });
                        StartReceivePacketHeader();
                    }
                    else
                    {
                        StartReceivePacketBody(packet);
                    }
                }
                else
                {
                    string err = "bytes read count is zero";
                    ErrorrOccurred(err);
                }
            }
            catch (Exception e)
            {
                ErrorrOccurred(e.Message);
            }
        }

        private void ReceivePacketBodyCallback(IAsyncResult result)
        {
            try
            {
                int bytesRead = Socket.EndReceive(result);
                if (bytesRead > 0)
                {
                    NetPacket packet = result.AsyncState as NetPacket;
                    Loom.QueueOnMainThread(() =>
                    {
                        NetManager.NotifyEvent(new Evt(packet.GetMessageID(), packet.GetBody()));
                    });
                    StartReceivePacketHeader();
                }
                else
                {
                    string err = "bytes read count is zero";
                    ErrorrOccurred(err);
                }
            }
            catch (Exception e)
            {
                ErrorrOccurred(e.Message);
            }
        }

        private void SendCallback(IAsyncResult result)
        {
            try
            {
                int bytesSent = Socket.EndSend(result);
            }
            catch (Exception e)
            {
                ErrorrOccurred(e.Message);
            }
        }
    }
}
