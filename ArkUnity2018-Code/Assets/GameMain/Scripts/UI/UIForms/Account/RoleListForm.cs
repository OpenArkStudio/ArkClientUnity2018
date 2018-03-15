using System;
using System.Collections;
using System.Collections.Generic;
using AFMsg;
using UnityEngine;
using UnityEngine.UI;

namespace ARKGame
{
    public class RoleListForm : UGuiForm
    {
        [SerializeField]
        RoleCreateCtrl m_roleCreateCtrl;
        [SerializeField]
        RoleSelectListCtrl m_roleSelectListCtrl;

        ProcedureCreateRole m_procedure;

#if UNITY_2017_3_OR_NEWER
        protected override void OnInit(object userData)
#else
        protected internal override void OnInit(object userData)
#endif
        {
            m_procedure = (ProcedureCreateRole)userData;


            base.OnInit(userData);
        }

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

            Refresh();
        }



#if UNITY_2017_3_OR_NEWER
        protected override void OnClose(object userData)
#else
        protected internal override void OnClose(object userData)
#endif
        {
            base.OnClose(userData);
            m_procedure = null;
        }


        public void Refresh()
        {
            var listInfo = ARKGameEntry.AFData.m_selfRoleList;
            if (listInfo.Count == 0)
            {
                //create role
                m_roleCreateCtrl.Show(true);
                m_roleSelectListCtrl.Show(false);
            }
            else
            {
                //role list
                m_roleCreateCtrl.Show(false);
                m_roleSelectListCtrl.Show(true);
                m_roleSelectListCtrl.Init();
            }
        }
    }
}