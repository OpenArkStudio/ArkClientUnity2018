using GameFramework.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Google.Protobuf;
using AFTCPClient;
using GameFramework;

namespace ARKGame
{
    public class AFNetworkChannelHelper : INetworkChannelHelper
    {
        AFNet m_net;
        public AFNetworkChannelHelper(AFNet net)
        {
            m_net = net;
        }
        public int PacketHeaderLength
        {
            get
            {
                return (int)AFTCPClient.ConstDefine.AF_PACKET_HEAD_SIZE;
            }
        }

        public Packet DeserializePacket(IPacketHeader packetHeader, Stream source, out object customErrorData)
        {
            customErrorData = null;
            DownwardPacketHeader downwardPacketHeader = packetHeader as DownwardPacketHeader;
            ushort msgID = downwardPacketHeader.MsgHead.unMsgID;

            PacketBase packet = m_net.DeserializePacket(msgID, source);
            packet.MsgHead = downwardPacketHeader.MsgHead;
            return packet ;
        }

        public IPacketHeader DeserializePacketHeader(Stream source, out object customErrorData)
        {
            customErrorData = null;
            DownwardPacketHeader packetHeader = ReferencePool.Acquire<DownwardPacketHeader>();
            if (!packetHeader.Deserialize(source))
            {
                Log.Warning("!packetHeader.Deserialize(source)!");
                return null;
            }
            return packetHeader;
        }

        public void Initialize(INetworkChannel networkChannel)
        {
        }

        public bool SendHeartBeat()
        {
            return true;
        }

        public byte[] Serialize<T>(T packet) where T : Packet
        {
            IMessage xData = packet as IMessage;
            MemoryStream stream = new MemoryStream();
            MessageExtensions.WriteTo(xData, stream);
            MsgHead head = (packet as PacketBase).MsgHead;

            return SendMsg(head, xData.ToByteArray());
        }

        public void Shutdown()
        {
            
        }

        private byte[] SendMsg(MsgHead head, byte[] bodyByte)
        {
            head.unDataLen = (UInt32)bodyByte.Length + (UInt32)ConstDefine.AF_PACKET_HEAD_SIZE;

            byte[] headByte = StructureTransform.StructureToByteArrayEndian(head);

            byte[] sendBytes = new byte[head.unDataLen];
            headByte.CopyTo(sendBytes, 0);
            bodyByte.CopyTo(sendBytes, headByte.Length);
            return sendBytes;
        }
    }
}