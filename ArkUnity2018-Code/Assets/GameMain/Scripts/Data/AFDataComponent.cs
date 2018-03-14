
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace ARKGame
{
    public class AFDataComponent : GameFrameworkComponent
    {
        #region Static
        public AFCoreEx.AFIDENTID PBToAF(AFMsg.Ident xID)
        {
            AFCoreEx.AFIDENTID xIdent = new AFCoreEx.AFIDENTID();
            xIdent.nHead64 = xID.High;
            xIdent.nData64 = xID.Low;
            return xIdent;
        }
        #endregion

        public AFCoreEx.AFIDENTID m_selfRoleID;
        public AFMsg.RoleLiteInfo m_selfRoleInfo;
        public List<AFMsg.RoleLiteInfo> m_selfRoleList = new List<AFMsg.RoleLiteInfo>();

    }
}