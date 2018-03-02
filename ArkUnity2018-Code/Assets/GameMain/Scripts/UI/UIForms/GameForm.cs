using GameFramework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;

namespace ARKGame
{
    public class GameForm : UGuiForm
    {
        ProcedureGame m_procedure;
        [SerializeField]ScrollCircle m_ScrollCircle;

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);
            if (m_ScrollCircle == null)
            {
                Log.Error("m_ScrollCircle == null!");
            }

            m_procedure = (ProcedureGame)userData;
            
          
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
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            
        }

        public void OnExitClick()
        {
            m_procedure.Exit();
        }
    }
}