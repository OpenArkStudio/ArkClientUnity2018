using System;
using System.Collections;
using System.Collections.Generic;
using GameFramework.Event;
using UnityEngine;
using UnityGameFramework.Runtime;
using GameFramework;

namespace ARKGame
{
    public abstract class GameBase
    {
        public abstract GameMode GameMode { get; }

        public bool GameOver
        {
            get;
            protected set;
        }
        private MyHero m_myHero;
        
        public virtual void Initialize()
        {
            ARKGameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            ARKGameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            GameOver = false;

            //show hero entity
            ARKGameEntry.Entity.ShowMyHero(new HeroData(ARKGameEntry.Entity.GenerateSerialId(), 10000, CampType.Player)
            {
                Position = new Vector3(10f, 1f, 5f),
            });
        }

        public virtual void Shutdown()
        {
            ARKGameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            ARKGameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        }

        public virtual void Update(float elspseSeconds,float realElapseSeconds)
        {
            if (m_myHero != null)
            {
                
            }
        }

        protected virtual void OnShowEntityFailure(object sender, GameEventArgs e)
        {
            ShowEntityFailureEventArgs ne = (ShowEntityFailureEventArgs)e;
            Log.Warning("Show entity failure with error message '{0}'.", ne.ErrorMessage);
        }

        protected virtual void OnShowEntitySuccess(object sender, GameEventArgs e)
        {
            ShowEntitySuccessEventArgs ne = (ShowEntitySuccessEventArgs)e;
            Log.Info("Show Entity Success. name="+ne.Id);
            if(ne.EntityLogicType == typeof(MyHero))
            {
                m_myHero = (MyHero)ne.Entity.Logic;
            }
        }
    }
}