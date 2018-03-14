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


        public void Init()
        {

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
        }
        public void OnInputValueChanged(InputField input)
        {

        }
        public void OnInputEndEdit(InputField input)
        {

        }
    }
}