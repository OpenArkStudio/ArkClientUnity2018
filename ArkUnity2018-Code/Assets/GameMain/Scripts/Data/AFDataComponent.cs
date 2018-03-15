
using System.Collections.Generic;
using UnityGameFramework.Runtime;

namespace ARKGame
{
    public class AFDataComponent : GameFrameworkComponent
    {

        public AFCoreEx.AFIDENTID m_selfRoleID;
        public AFMsg.RoleLiteInfo m_selfRoleInfo;
        public List<AFMsg.RoleLiteInfo> m_selfRoleList = new List<AFMsg.RoleLiteInfo>();
        public EntityId m_selfHeroId;

        #region Data Transform
        public AFCoreEx.AFIDENTID PBToAF(AFMsg.Ident xID)
        {
            AFCoreEx.AFIDENTID xIdent = new AFCoreEx.AFIDENTID();
            xIdent.nHead64 = xID.High;
            xIdent.nData64 = xID.Low;
            return xIdent;
        }
        public AFMsg.Ident AFToPB(AFCoreEx.AFIDENTID xID)
        {
            AFMsg.Ident xIdent = new AFMsg.Ident();
            xIdent.High = xID.nHead64;
            xIdent.Low = xID.nData64;

            return xIdent;
        }
        #endregion


    }
}