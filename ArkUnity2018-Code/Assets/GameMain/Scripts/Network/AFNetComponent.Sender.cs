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
    }
}