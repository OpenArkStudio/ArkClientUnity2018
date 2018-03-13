using GameFramework.Network;
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
            if(xData.EventCode == AFMsg.EGameEventCode.EgecAccountSuccess)
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
            if (ReqServerListType.RsltWorldServer == xData.Type)
            {
                for (int i = 0; i < xData.Info.Count; ++i)
                {
                    ServerInfo info = xData.Info[i];
                    Log.Debug("world server info. name="+info.Name);
                    ARKGameEntry.AFNet.m_worldServerList.Add(info);
                    //mxPlayerNet.aWorldList.Add(info);
                }
                var procedure =(ProcedureLogin)ARKGameEntry.Procedure.CurrentProcedure;
                procedure.ShowServerListForm();
                //mxPlayerNet.ChangePlayerState(PlayerNet.PLAYER_STATE.E_PLAYER_WORLD_LIST_SUCCESSFUL_WAITING_SELECT_WORLD);
            }
            else if (ReqServerListType.RsltGamesErver == xData.Type)
            {
                for (int i = 0; i < xData.Info.Count; ++i)
                {
                    ServerInfo info = xData.Info[i];
                    Log.Debug("game server info. name=" + info.Name);
                    ARKGameEntry.AFNet.m_gameServerList.Add(info);
                    //mxPlayerNet.aServerList.Add(info);
                }
            }
        }
    }
}