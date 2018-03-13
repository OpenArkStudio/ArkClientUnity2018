using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace ARKGame
{
    public class ServerItem : MonoBehaviour
    {
        public delegate void OnSelectDelegate(AFMsg.ServerInfo serverInfo);
        public OnSelectDelegate OnSelect;

        [SerializeField]
        Text m_name;
        AFMsg.ServerInfo m_serverInfo { get; set; }
        // Use this for initialization
        void Start()
        {

        }

        public void Set(AFMsg.ServerInfo server)
        {
            m_serverInfo = server;
            m_name.text = Constant.Setting.AccountLogin+ server.Name;
        }
        public void OnSelectClick()
        {
            if (OnSelect != null)
            {
                OnSelect(m_serverInfo);
            }
        }
    }
}