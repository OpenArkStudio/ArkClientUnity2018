using UnityEngine;
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
        public AFCoreEx.AFIDENTID PBToAF(AFMsg.PBGUID xID)
        {
            AFCoreEx.AFIDENTID xIdent = new AFCoreEx.AFIDENTID();
            xIdent.nHead64 = xID.High;
            xIdent.nData64 = xID.Low;
            return xIdent;
        }
        public AFMsg.PBGUID AFToPB(AFCoreEx.AFIDENTID xID)
        {
            AFMsg.PBGUID xIdent = new AFMsg.PBGUID();
            xIdent.High = xID.nHead64;
            xIdent.Low = xID.nData64;

            return xIdent;
        }
        public Vector3 AFPostionToVector3(AFMsg.Position xPos)
        {
            return new Vector3(xPos.X, xPos.Y, xPos.Z);
        }
        public AFMsg.Position Vector3ToAFPosition(Vector3 xVec3)
        {
            return new AFMsg.Position() { X = xVec3.x, Y = xVec3.y, Z = xVec3.z };
        }
        #endregion


    }
}