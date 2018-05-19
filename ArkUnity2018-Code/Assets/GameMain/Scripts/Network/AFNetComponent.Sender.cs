using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ARKGame
{
    public partial class AFNetComponent
    {

        //test
        public void LoginPB(string strAccount, string strPassword, string strSessionKey)
        {
            AFMsg.ReqAccountLogin xData = new AFMsg.ReqAccountLogin();
            xData.Account = strAccount;
            xData.Password = strPassword;
            xData.SecurityCode = strSessionKey;
            xData.SignBuff = "";
            xData.ClientVersion = 1;
            xData.LoginMode = 0;
            xData.ClientIP = 0;
            xData.ClientMAC = 0;
            xData.DeviceInfo = "";
            xData.ExtraInfo = "";
            m_net.SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EgmiReqLogin, xData);
        }
        public void RequireWorldList()
        {
            AFMsg.ReqServerList xData = new AFMsg.ReqServerList();
            m_net.SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EgmiReqWorldList, xData);
        }
        public void RequireConnectWorld(int nWorldID)
        {
            AFMsg.ReqConnectWorld xData = new AFMsg.ReqConnectWorld();
            xData.WorldId = nWorldID;

            m_net.SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EgmiReqConnectWorld, xData);
        }
        public void RequireVerifyWorldKey(string strAccount, string strKey)
        {
            AFMsg.ReqAccountLogin xData = new AFMsg.ReqAccountLogin();
            xData.Account = strAccount;
            xData.Password = "";
            xData.SecurityCode = strKey;
            xData.SignBuff = "";
            xData.ClientVersion = 1;
            xData.LoginMode = 0;
            xData.ClientIP = 0;
            xData.ClientMAC = 0;
            xData.DeviceInfo = "";
            xData.ExtraInfo = "";
            m_net.SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EgmiReqConnectKey, xData);
        }
        public void RequireGameServerList()
        {
            AFMsg.ReqServerList xData = new AFMsg.ReqServerList();
            xData.Type = AFMsg.ReqServerListType.RsltGamesErver;
            m_net.SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EgmiReqWorldList, xData);
        }
        public void RequireSelectGameServer(int nServerID)
        {
            AFMsg.ReqSelectServer xData = new AFMsg.ReqSelectServer();
            xData.WorldId = nServerID;
            m_net.SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EgmiReqSelectServer, xData);
        }
        public void RequireRoleList(string strAccount, int nGameID)
        {
            AFMsg.ReqRoleList xData = new AFMsg.ReqRoleList();
            xData.GameId = nGameID;
            xData.Account = strAccount;
            m_net.SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EgmiReqRoleList, xData);
        }
        public void RequireCreateRole(string strAccount, string strRoleName, int byCareer, int bySex, int nGameID)
        {
            if (strRoleName.Length >= 20 || strRoleName.Length < 1)
            {
                Log.Warning("Role name is invalid! Please input again!");
                return;
            }
            AFMsg.ReqCreateRole xData = new AFMsg.ReqCreateRole();
            xData.Career = byCareer;
            xData.Sex = bySex;
            xData.NoobName = strRoleName;
            xData.Account = strAccount;
            xData.Race = 0;
            xData.GameId = nGameID;
            m_net.SendMsg(new AFCoreEx.AFIDENTID(), AFMsg.EGameMsgID.EgmiReqCreateRole, xData);
        }
        public void RequireEnterGameServer(AFCoreEx.AFIDENTID objectID, string strAccount, string strRoleName, int nServerID)
        {
            AFMsg.ReqEnterGameServer xData = new AFMsg.ReqEnterGameServer();
            xData.Name = strRoleName;
            xData.Account = strAccount;
            xData.GameId = nServerID;
            xData.Id = ARKGameEntry.AFData.AFToPB(objectID);
            m_net.SendMsg(objectID, AFMsg.EGameMsgID.EgmiReqEnterGame, xData);
        }
        public void RequireHeartBeat(AFCoreEx.AFIDENTID objectID)
        {
            AFMsg.ReqHeartBeat xData = new AFMsg.ReqHeartBeat();
            m_net.SendMsg(objectID, AFMsg.EGameMsgID.EgmiStsHeartBeat, xData);
        }
        public void RequireMove(AFCoreEx.AFIDENTID objectID, float fX, float fZ, Vector3 sourcePos)
        {
            AFMsg.ReqAckPlayerMove xData = new AFMsg.ReqAckPlayerMove();
            xData.Mover = ARKGameEntry.AFData.AFToPB(objectID);
            xData.MoveType = 0;

            AFMsg.Position xTargetPos = new AFMsg.Position();
            xTargetPos.X = fX;
            xTargetPos.Z = fZ;
            xData.TargetPos.Add(xTargetPos);
            AFMsg.Position xSourcePos = ARKGameEntry.AFData.Vector3ToAFPosition(sourcePos);
            xData.SourcePos.Add(xSourcePos);
            m_net.SendMsg(objectID, AFMsg.EGameMsgID.EgmiReqMove, xData);
        }
    }
}