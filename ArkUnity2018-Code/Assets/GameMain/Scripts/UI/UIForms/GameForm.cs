using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARKGame
{
    public class GameForm : UGuiForm
    {
        ProcedureGame m_procedure;

#if UNITY_2017_3_OR_NEWER
        protected override void OnOpen(object userData)
#else
        protected internal override void OnOpen(object userData)
#endif
        {
            base.OnOpen(userData);

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


        public void OnExitClick()
        {
            m_procedure.Exit();
        }
    }
}