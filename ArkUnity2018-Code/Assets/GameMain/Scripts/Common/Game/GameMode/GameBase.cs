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
        //游戏场开始游戏延时时间
        static float DelayPlayingTime = 2.0f;
        public abstract GameMode GameMode { get; }
        public bool GameBegin { get; protected set; }
        public bool GameOver{ get; protected set; }
        private MyHero m_myHero;
        private float m_tmpTime = 0f;
        public virtual void Initialize()
        {
            ARKGameEntry.Event.Subscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            ARKGameEntry.Event.Subscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
            GameOver = false;

            //show hero entity
            ARKGameEntry.Entity.ShowMyHero(new HeroData(ARKGameEntry.Entity.GenerateSerialId(), (int)ARKGameEntry.AFData.m_selfHeroId, CampType.Player)
            {
                Position = new Vector3(10f, 0f, 5f),
            });
        }

        public virtual void Shutdown()
        {
            ARKGameEntry.Event.Unsubscribe(ShowEntitySuccessEventArgs.EventId, OnShowEntitySuccess);
            ARKGameEntry.Event.Unsubscribe(ShowEntityFailureEventArgs.EventId, OnShowEntityFailure);
        }

        public virtual void Update(float elspseSeconds,float realElapseSeconds)
        {
            m_tmpTime += realElapseSeconds;
            if (GameOver)
            {
                m_myHero.Playing = false;
                return;
            }
           
            if (!GameBegin && m_tmpTime >= DelayPlayingTime)
            {
                GameBegin = true;
                
                m_myHero.Playing = true;
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