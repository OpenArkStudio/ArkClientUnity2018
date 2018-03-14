using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ARKGame
{
    public class HomeForm : UGuiForm
    {
        [SerializeField]
        Text m_selfName;
        [SerializeField]
        Image m_selfIcon;

        ProcedureHome m_procedure;

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            m_procedure = (ProcedureHome)userData;

            m_selfName.text = ARKGameEntry.AFData.m_selfRoleInfo.NoobName;
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(object userData)
#else
        protected internal override void OnClose(object userData)
#endif
        {
            base.OnClose(userData);

        }


        public void OnQuickPlayClick()
        {
            m_procedure.Play();
        }


    }
}