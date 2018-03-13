using AFTCPClient;
using GameFramework;
using GameFramework.Event;
using Google.Protobuf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ARKGame
{
    public class AFNet 
    {
        //public delegate void OnConnectedDelegate();
        //private event OnConnectedDelegate m_OnConnected;

        Hashtable m_handlers = new Hashtable();
        GameFramework.Network.INetworkChannel m_channel;

        public AFNet()
        {
            Init();
        }
        private void Init()
        {
            ARKGameEntry.Event.Subscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnected);
            ARKGameEntry.Event.Subscribe(NetworkErrorEventArgs.EventId, OnNetworkError);
            ARKGameEntry.Event.Subscribe(NetworkSendPacketEventArgs.EventId, OnNetworkSendPacket);
        }

        public void CreateChannel(string channelName)
        {
            //connect server
            AFNetworkChannelHelper channelHelper = new AFNetworkChannelHelper(this);
            m_channel = ARKGameEntry.Network.CreateNetworkChannel(channelName, channelHelper);
        }
        public void Connect(string ip, int port)
        {
            m_channel.Connect(System.Net.IPAddress.Parse(ip), port);
        }
        //public void CreateAndConnect(string channelName, string ip, int port, OnConnectedDelegate onConnected)
        //{
        //    //m_OnConnected += onConnected;
        //    CreateAndConnect(channelName,ip,port);
        //}
        public void Disconnect()
        {
            ARKGameEntry.Event.Unsubscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnected);
            ARKGameEntry.Event.Unsubscribe(NetworkErrorEventArgs.EventId, OnNetworkError);
            ARKGameEntry.Event.Unsubscribe(NetworkSendPacketEventArgs.EventId, OnNetworkSendPacket);
            m_channel.Close();
            //ARKGameEntry.Network.DestroyNetworkChannel(m_channel.Name);
        }
        public void OnApplicationQuit()
        {
            Log.Debug("OnApplicationQuit");
            Disconnect();
        }

        #region Receiver
        public void RegisterHandler(ushort msgID, PacketHandlerBase handler)
        {
            m_channel.RegisterHandler(handler);
            if(!m_handlers.ContainsKey(msgID))
            {
                m_handlers.Add(msgID, handler);
            }
            else
            {
                Log.Error("MsgID Regist Double!");
            }
        }

        public PacketBase DeserializePacket(ushort msgID, Stream source)
        {
            if (!m_handlers.ContainsKey(msgID))
            {
                Log.Warning("m_handlers cannot contains msgID="+msgID);
                return null;
            }
            return ((PacketHandlerBase)m_handlers[msgID]).DeserializePacket(source);
        }
        public void Update()
        {
             
        }
        #endregion

        #region Sender

        public void SendMsg(AFCoreEx.AFIDENTID xID, AFMsg.EGameMsgID unMsgID, PacketBase xData)
        {
            MsgHead head = new MsgHead();
            head.unMsgID = (UInt16)unMsgID;
            head.nHead64 = xID.nHead64;
            head.nData64 = xID.nData64;
            xData.MsgHead = head;
            m_channel.Send(xData);
            //SendMsg(head, stream.ToArray());
        }
        #endregion

        #region Event Callback
        private void OnNetworkConnected(object sender, GameEventArgs e)
        {
            Log.Debug("OnNetworkConnected.event.id = " + e.Id);
            //if (m_OnConnected != null)
            //{
            //    m_OnConnected();
            //}
        }
        private void OnNetworkError(object sender, GameEventArgs e)
        {
            NetworkErrorEventArgs ne = (NetworkErrorEventArgs)e;
            Log.Error("OnNetworkError.errorCode=" + ne.ErrorCode+",info="+ne.ErrorMessage);
        }
        private void OnNetworkSendPacket(object sender, GameEventArgs e)
        {
            NetworkSendPacketEventArgs ne = (NetworkSendPacketEventArgs)e;

            Log.Debug("OnNetworkSendPacket.e.id=" + e.Id);
        }
        #endregion
    }
}