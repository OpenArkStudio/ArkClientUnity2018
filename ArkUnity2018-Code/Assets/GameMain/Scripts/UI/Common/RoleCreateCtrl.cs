using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARKGame
{
    public class RoleCreateCtrl : MonoBehaviour
    {
        [SerializeField]
        InputField m_inputRole;


        private void Start()
        {
            m_inputRole.text = "无敌是多么寂寞";
        }
        public void Show(bool show)
        {
            m_inputRole.text = "";
            gameObject.SetActive(show);
        }
        public void OnQueryButtonClick()
        {
            string role = m_inputRole.text;
            Log.Info("create role. name = "+role);
            ARKGameEntry.AFNet.RequireCreateRole(ARKGameEntry.AFNet.m_account, role, 0, 0, ARKGameEntry.AFNet.m_serverId);
        }
        public void OnInputValueChanged(InputField input)
        {

        }
        public void OnInputEndEdit(InputField input)
        {

        }
    }
}