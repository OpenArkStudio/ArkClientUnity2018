using GameFramework.Network;
using GameFramework;
using System.IO;
using System;
using AFMsg;

namespace ARKGame
{
    #region Login
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
               
                Log.Debug("Select Game Server Success.");
                //enter Home Scene.
                Log.Debug("req role list. account="+ ARKGameEntry.AFNet.m_account+", serverId="+ARKGameEntry.AFNet.m_serverId);
                ARKGameEntry.AFNet.RequireRoleList(ARKGameEntry.AFNet.m_account, ARKGameEntry.AFNet.m_serverId);
            }
        }
    }
    public class AckRoleLiteInfoListHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return (int)AFMsg.EGameMsgID.EgmiAckRoleList;
            }
        }

        public override PacketBase DeserializePacket(Stream source)
        {
            var xData = new AFMsg.AckRoleLiteInfoList();
            xData = AckRoleLiteInfoList.Parser.ParseFrom(source);
            return xData as PacketBase;
        }

        public override void Handle(object sender, Packet packet)
        {
            var xData = packet as AckRoleLiteInfoList;
            Log.Debug("Role List Count ="+ xData.CharData.Count);
            ARKGameEntry.AFData.m_selfRoleList.Clear();
            for (int i = 0; i < xData.CharData.Count; ++i)
            {
                AFMsg.RoleLiteInfo info = xData.CharData[i];
                ARKGameEntry.AFData.m_selfRoleList.Add(info);
            }
            if(xData.CharData.Count == 0)//第一次登陆app需要创建角色
            {
                //create role
                ((ProcedureLogin)ARKGameEntry.Procedure.CurrentProcedure).CreateRole();
            }
            else
            {
                ARKGameEntry.AFData.m_selfHeroId = EntityId.Hero_XiaoMing;
                var roleInfo = xData.CharData[0];
                ARKGameEntry.AFData.m_selfRoleInfo = roleInfo;
                ARKGameEntry.AFNet.RequireEnterGameServer(ARKGameEntry.AFData.m_selfRoleID, ARKGameEntry.AFNet.m_account, roleInfo.NoobName, ARKGameEntry.AFNet.m_serverId);
                
                var procedure = ARKGameEntry.Procedure.CurrentProcedure;
                if(procedure.GetType() == typeof(ProcedureLogin))
                {
                    //enter home
                    ((ProcedureLogin)procedure).LoginSuccess();
                }else if(procedure.GetType() == typeof(ProcedureCreateRole))
                {
                    //enter home
                    ((ProcedureCreateRole)procedure).LoginSuccess();
                }
            }
           
        }
    }
    #endregion
    #region Home

    #endregion
    #region Game
    public class AckJoinPvpHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return (int)AFMsg.EGameMsgID.EgmtAckStartPvp;
            }
        }

        public override PacketBase DeserializePacket(Stream source)
        {
            //AFMsg.
            return null;
        }

        public override void Handle(object sender, Packet packet)
        {
            throw new NotImplementedException();
        }
    }
    public class AckPlayerMoveHandler : PacketHandlerBase
    {
        public override int Id
        {
            get
            {
                return (int)AFMsg.EGameMsgID.EgmiAckMove;
            }
        }

        public override PacketBase DeserializePacket(Stream source)
        {
            var xData =  ReqAckPlayerMove.Parser.ParseFrom(source);
            return xData as PacketBase;
        }

        public override void Handle(object sender, Packet packet)
        {
            var xData = packet as ReqAckPlayerMove;
            MoverCtrl moveCtrl = ((ProcedureGame)ARKGameEntry.Procedure.CurrentProcedure).m_CurrentGame.m_myHero.m_moverCtrl;
            if (xData.TargetPos.Count > 0)
            {
                Log.Debug("player move. target pos =" + xData.TargetPos[0].ToString());
                var pos = xData.TargetPos[0];
                float h = pos.X;
                float v = pos.Z;
                moveCtrl.Set(h, v);
            }
            if (xData.SourcePos.Count > 0)
            {
                Log.Debug("player move. source pos =" + xData.TargetPos[0].ToString());
                var pos = ARKGameEntry.AFData.AFPostionToVector3( xData.SourcePos[0]);
                moveCtrl.SetPosition(pos);
            }
        }
    }
    #endregion
}