using GameFramework.Network;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GameFramework;
using System.IO;
using AFTCPClient;
using System.Runtime.InteropServices;

namespace ARKGame
{
    public enum PacketType
    {
        Undefined,
        DownwardPacket,//下行
        UpPacket,//上行
    }
    public abstract class PacketHeaderBase : IPacketHeader, IReference
    {
        public abstract PacketType PacketType { get; }
        public AFTCPClient.MsgHead MsgHead{ get; set; }
        public int PacketLength
        {
            get;set;
        }
        public bool IsValid
        {
            get {
                return PacketType != PacketType.Undefined && MsgHead!=null && PacketLength > 0;
            }        
        }
        public void Clear()
        {
            MsgHead = null;
            PacketLength = 0;
        }
    }
    public class DownwardPacketHeader : PacketHeaderBase
    {
        //public const int Length = (int)AFTCPClient.ConstDefine.AF_PACKET_HEAD_SIZE;
        public override PacketType PacketType
        {
            get
            {
                return PacketType.DownwardPacket;
            }
        }

        public bool Deserialize(Stream stream)
        {
            if(stream == null)
            {
                Log.Warning("stream == null!");
                return false;
            }
            if (!stream.CanRead)
            {
                Log.Warning("!stream.CanRead.");
                return false;
            }
            if (stream.Length < (int)AFTCPClient.ConstDefine.AF_PACKET_HEAD_SIZE)
            {
                Log.Warning("stream.Length < Length.");
                return false;
            }
            object structType = new MsgHead();
            byte[] headBytes = new byte[Marshal.SizeOf(structType)];
            stream.Read(headBytes, 0, Marshal.SizeOf(structType));

            StructureTransform.ByteArrayToStructureEndian(headBytes, ref structType, 0);
            MsgHead = (MsgHead)structType;
            PacketLength = (int)MsgHead.unDataLen- (int)ConstDefine.AF_PACKET_HEAD_SIZE;
            return true;
        }

    }
}