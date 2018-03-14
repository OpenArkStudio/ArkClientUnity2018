using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARKGame
{
    public class RoleSelectItem : MonoBehaviour
    {
        public delegate void OnSelectDelegate(AFMsg.RoleLiteInfo roleInfo);
        public OnSelectDelegate OnSelect;

        [SerializeField]
        Text m_name;
        AFMsg.RoleLiteInfo m_roleInfo;
        // Use this for initialization
        void Start()
        {

        }
        public void Set(AFMsg.RoleLiteInfo roleInfo)
        {
            m_roleInfo = roleInfo;
            m_name.text = roleInfo.NoobName;
        }
        public void OnSelectClick()
        {
            if (OnSelect != null)
            {
                OnSelect(m_roleInfo);
            }
        }
    }
}