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
        Hashtable m_handlers = new Hashtable();
        GameFramework.Network.INetworkChannel m_loginChannel;

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

        public void CreateAndConnect(string channelName, string ip, int port)
        {
            //connect server
            AFNetworkChannelHelper channelHelper = new AFNetworkChannelHelper(this);
            m_loginChannel = ARKGameEntry.Network.CreateNetworkChannel(channelName, channelHelper);
            m_loginChannel.Connect(System.Net.IPAddress.Parse(ip), port);

            
        }
        public void OnApplicationQuit()
        {
            Log.Debug("OnApplicationQuit");
        }

        #region Receiver
        public void RegisterHandler(ushort msgID, PacketHandlerBase handler)
        {
            m_loginChannel.RegisterHandler(handler);
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
            m_loginChannel.Send(xData);
            //SendMsg(head, stream.ToArray());
        }
        //public void SendMsg(AFCoreEx.AFIDENTID xID, AFMsg.EGameMsgID unMsgID, IMessage xData)
        //{
        //    MemoryStream stream = new MemoryStream();
        //    MessageExtensions.WriteTo(xData, stream);
        //    //SendMsg(xID, unMsgID, stream);
        //    MsgHead head = new MsgHead();
        //    head.unMsgID = (UInt16)unMsgID;
        //    head.nHead64 = xID.nHead64;
        //    head.nData64 = xID.nData64;
            
        //    SendMsg(head, stream.ToArray());
        //}
        //private void SendMsg(AFCoreEx.AFIDENTID xID, AFMsg.EGameMsgID unMsgID, MemoryStream stream)
        //{
        //    MsgHead head = new MsgHead();
        //    head.unMsgID = (UInt16)unMsgID;
        //    head.nHead64 = xID.nHead64;
        //    head.nData64 = xID.nData64;
        //    SendMsg(head, stream.ToArray());
        //}


        //public void SendMsg(MsgHead head, byte[] bodyByte)
        //{
        //    head.unDataLen = (UInt32)bodyByte.Length + (UInt32)ConstDefine.AF_PACKET_HEAD_SIZE;

        //    byte[] headByte = StructureTransform.StructureToByteArrayEndian(head);
            
        //    byte[] sendBytes = new byte[head.unDataLen];
        //    headByte.CopyTo(sendBytes, 0);
        //    bodyByte.CopyTo(sendBytes, headByte.Length);

        //    //m_loginChannel.Send(sendBytes);

        //    //m_loginChannel.Send(new AFPacket());
        //}
        #endregion

        #region Event Callback
        private void OnNetworkConnected(object sender, GameEventArgs e)
        {
            Log.Debug("OnNetworkConnected.event.id = " + e.Id);
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