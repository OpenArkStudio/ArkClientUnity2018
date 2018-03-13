﻿using GameFramework.Network;
using GameFramework;
using System.IO;
using System;
using AFMsg;

namespace ARKGame
{
    public class AckLoginHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return (int)AFMsg.EGameMsgID.EgmiAckLogin;
            }
        }

        public override PacketBase DeserializePacket(Stream source)
        {
            AFMsg.AckEventResult xData = new AFMsg.AckEventResult();
            xData = AFMsg.AckEventResult.Parser.ParseFrom(source);
            return xData as PacketBase;
        }

        public override void Handle(object sender, Packet packet)
        {
            var xData = packet as AFMsg.AckEventResult;
            if (xData.EventCode == AFMsg.EGameEventCode.EgecAccountSuccess)
            {
                Log.Debug("Login success.");
                ARKGameEntry.AFNet.RequireWorldList();
            }
            else
            {
                Log.Warning("Login failed! AckLoginHandler.id=" + packet.Id);
            }
            
        }



    }

    public class AckWorldListHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return (int)AFMsg.EGameMsgID.EgmiAckWorldList;
            }
        }

        public override PacketBase DeserializePacket(Stream source)
        {
            AFMsg.AckServerList xData = new AFMsg.AckServerList();
            xData = AFMsg.AckServerList.Parser.ParseFrom(source);
            return xData as PacketBase;
        }

        public override void Handle(object sender, Packet packet)
        {
            var xData = packet as AFMsg.AckServerList;
            if (ReqServerListType.RsltWorldServer == xData.Type)//world server list
            {
                for (int i = 0; i < xData.Info.Count; ++i)
                {
                    ServerInfo info = xData.Info[i];
                    Log.Debug("world server info. name="+info.Name);
                    ARKGameEntry.AFNet.m_worldServerList.Add(info);
                }
                var procedure =(ProcedureLogin)ARKGameEntry.Procedure.CurrentProcedure;
                procedure.ShowWorldServerListForm();
            }
            else if (ReqServerListType.RsltGamesErver == xData.Type)//game server list
            {
                for (int i = 0; i < xData.Info.Count; ++i)
                {
                    ServerInfo info = xData.Info[i];
                    Log.Debug("game server info. name=" + info.Name);
                    ARKGameEntry.AFNet.m_gameServerList.Add(info);
                }
                var procedure = (ProcedureLogin)ARKGameEntry.Procedure.CurrentProcedure;
                procedure.ShowGameServerListForm();
            }
        }
    }

    public class AckConnectWorldHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return (int)AFMsg.EGameMsgID.EgmiAckConnectWorld;
            }
        }

        public override PacketBase DeserializePacket(Stream source)
        {
            AckConnectWorldResult xData = new AckConnectWorldResult();
            xData = AckConnectWorldResult.Parser.ParseFrom(source);
            return xData as PacketBase;
        }

        public override void Handle(object sender, Packet packet)
        {
            var xData = packet as AckConnectWorldResult;
            Log.Debug("AckConnectWorld.Handler.worldip=" +xData.WorldIp+",worldport="+xData.WorldPort+",worldkey="+xData.WorldKey+",account="+xData.Account);
            ARKGameEntry.AFNet.m_account = xData.Account;
            ARKGameEntry.AFNet.m_worldIP = xData.WorldIp;
            ARKGameEntry.AFNet.m_worldPort = xData.WorldPort;
            ARKGameEntry.AFNet.m_worldKey = xData.WorldKey;

            ((ProcedureLogin)ARKGameEntry.Procedure.CurrentProcedure).ConnectWorldServer();
        }
    }

    public class AckConnectKeyHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return (int)AFMsg.EGameMsgID.EgmiAckConnectKey;
            }
        }

        public override PacketBase DeserializePacket(Stream source)
        {
            AFMsg.AckEventResult xData = new AFMsg.AckEventResult();
            xData = AFMsg.AckEventResult.Parser.ParseFrom(source);
            return xData as PacketBase;
        }

        public override void Handle(object sender, Packet packet)
        {
            var xData = packet as AFMsg.AckEventResult;
            if (xData.EventCode == EGameEventCode.EgecVerifyKeySuccess)
            {
                Log.Debug("Verify Key Success.");
                ARKGameEntry.AFData.m_selfRoleID = ARKGameEntry.AFData.PBToAF(xData.EventObject);
                ARKGameEntry.AFNet.RequireGameServerList();
            }
            else
            {
                Log.Warning("Login failed! AckLoginHandler.id=" + packet.Id);
            }
        }
    }

    public class AckSelectGameServerHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return (int)AFMsg.EGameMsgID.EgmiAckSelectServer;
            }
        }

        public override PacketBase DeserializePacket(Stream source)
        {
            AFMsg.AckEventResult xData = new AFMsg.AckEventResult();
            xData = AFMsg.AckEventResult.Parser.ParseFrom(source);
            return xData as PacketBase;
        }

        public override void Handle(object sender, Packet packet)
        {
            var xData = packet as AckEventResult;
            if (xData.EventCode == EGameEventCode.EgecSelectserverSuccess)
            {
                //req role list
                Log.Debug("Select Game Server Success.");
                //enter Home Scene.
                ((ProcedureLogin)ARKGameEntry.Procedure.CurrentProcedure).LoginSuccess();
            }
        }
    }
}