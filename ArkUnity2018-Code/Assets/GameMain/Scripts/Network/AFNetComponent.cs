using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ARKGame
{
    public partial class AFNetComponent : GameFrameworkComponent
    {
        
        AFNet m_net;
        Dictionary<string, AFNet> m_nets = new Dictionary<string, AFNet>();
        public List<AFMsg.ServerInfo> m_worldServerList = new List<AFMsg.ServerInfo>();
        public List<AFMsg.ServerInfo> m_gameServerList = new List<AFMsg.ServerInfo>();
        
        //
        public string m_account;
        public string m_worldIP = "";
        public int m_worldPort = 0;
        public string m_worldKey = "";
        public int m_serverId;
        public AFNet SetChannel(string channelName)
        {
            if (!m_nets.ContainsKey(channelName))
            {
                Log.Error("Cannot contains channel. channel name = "+channelName);
                return null;
            }
            m_net = m_nets[channelName];
            return m_net;
        }
        // Use this for initialization
        public AFNet CreateChannel(string channelName)
        {
            var net = new AFNet();
            m_nets.Add(channelName, net);
            net.CreateChannel(channelName);
            RegistAllHandlers(channelName);
            return net;
        }
        public AFNetComponent ConnectChannel(string channelName, string ip, int port)
        {
            var net = m_nets[channelName];
            net.Connect(ip,port);
            return this;
        }

        private void RegistAllHandlers(string channelName)
        {
            var net = m_nets[channelName];
            RegistHandler(net, AFMsg.EGameMsgID.EgmiAckLogin, new AckLoginHandler());
            RegistHandler(net, AFMsg.EGameMsgID.EgmiAckWorldList, new AckWorldListHandler());
            RegistHandler(net, AFMsg.EGameMsgID.EgmiAckConnectWorld, new AckConnectWorldHandler());
            RegistHandler(net, AFMsg.EGameMsgID.EgmiAckConnectKey, new AckConnectKeyHandler());
            RegistHandler(net, AFMsg.EGameMsgID.EgmiAckSelectServer, new AckSelectGameServerHandler());
            RegistHandler(net, AFMsg.EGameMsgID.EgmiAckRoleList, new AckRoleLiteInfoListHandler());
        }
        private void RegistHandler(AFNet net, AFMsg.EGameMsgID msgID, PacketHandlerBase handler)
        {
            net.RegisterHandler((ushort)msgID, handler);
        }
        public void Disconnect(string channelName)
        {
            if (m_nets.ContainsKey(channelName))
            {
                m_nets[channelName].Disconnect();
            }
        }
    }
}