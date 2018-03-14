using System;
using System.Collections;
using System.Collections.Generic;
using AFMsg;
using UnityEngine;
using UnityEngine.UI;
using GameFramework;

namespace ARKGame
{
    public class RoleSelectListCtrl : MonoBehaviour
    {

        [SerializeField]
        GameObject m_prefab;

        [SerializeField]
        ScrollRect m_scrollRect;
        // Use this for initialization
        public void Init()
        {
            var listInfo = ARKGameEntry.AFData.m_selfRoleList;
            m_scrollRect.content.DetachChildren();
            foreach (var v in listInfo)
            {
                var go = Instantiate(m_prefab, m_scrollRect.content);
                var item = go.GetComponent<RoleSelectItem>();
                item.Set(v);
                item.OnSelect = Item_OnSelect;
            }
        }

        private void Item_OnSelect(RoleLiteInfo roleInfo)
        {
            Log.Info("Select Role. role name = " + roleInfo.NoobName);
            ARKGameEntry.AFData.m_selfRoleInfo = roleInfo;
            ARKGameEntry.AFNet.RequireEnterGameServer(ARKGameEntry.AFData.m_selfRoleID, ARKGameEntry.AFNet.m_account, roleInfo.NoobName, ARKGameEntry.AFNet.m_serverId);
            ((ProcedureLogin)ARKGameEntry.Procedure.CurrentProcedure).LoginSuccess();
        }
        public void Show(bool show)
        {
            gameObject.SetActive(show);
        }
    }
}