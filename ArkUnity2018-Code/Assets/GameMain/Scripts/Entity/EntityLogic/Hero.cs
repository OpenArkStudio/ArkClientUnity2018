using GameFramework;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ARKGame
{
    /// <summary>
    /// 英雄类
    /// </summary>
    public class Hero : TargetableObject
    {
        [SerializeField]HeroData m_HeroData = null;
        [SerializeField]
        protected Mover m_Mover = null;
#if UNITY_2017_3_OR_NEWER
        protected override void OnShow(object userData)
#else
        protected internal override void OnShow(object userData)
#endif
        {
            base.OnShow(userData);
            m_HeroData = userData as HeroData;
            if (m_HeroData == null)
            {
                Log.Error("Hero data is invalid.");
                return;
            }
            Name = string.Format("Hero ({0})",Id.ToString());
            m_Mover = gameObject.GetOrAddComponent<Mover>();
            if (m_Mover == null)
            {
                Log.Error("m_Mover == null!");
            }
            // ARKGameEntry.Entity.ShowThruster(m_HeroData.GetThrusterData());
        }

        public override ImpactData GetImpactData()
        {
            return new ImpactData(m_HeroData.Camp, m_HeroData.HP, 0, m_HeroData.Defense);
        }

    }
}