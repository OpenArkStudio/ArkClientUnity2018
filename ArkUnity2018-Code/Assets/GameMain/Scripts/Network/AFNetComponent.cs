using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ARKGame
{
    public class AFNetComponent : GameFrameworkComponent
    {

        AFNet m_net;
        public List<AFMsg.ServerInfo> m_worldServerList = new List<AFMsg.ServerInfo>();
        public List<AFMsg.ServerInfo> m_gameServerList = new List<AFMsg.ServerInfo>();

        // Use this for initialization
        public void CreateAndConnectChannel(string channelName, string ip, int port)
        {
            m_net = new AFNet();
            m_net.CreateAndConnect(channelName, ip, port);
            RegistAllHandlers();
        }
        private void RegistAllHandlers()
        {
            RegistHandler(AFMsg.EGameMsgID.EgmiAckLogin, new AckLoginHandler());
            RegistHandler(AFMsg.EGameMsgID.EgmiAckWorldList, new AckWorldListHandler());
        }
        private void RegistHandler(AFMsg.EGameMsgID msgID, PacketHandlerBase handler)
        {
            m_net.RegisterHandler((ushort)msgID, handler);
        }
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
    }
}