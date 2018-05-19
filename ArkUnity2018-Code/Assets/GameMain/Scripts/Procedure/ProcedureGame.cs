using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using GameFramework;
using System.Collections.Generic;
using GameFramework.Fsm;
using GameFramework.Procedure;

namespace ARKGame
{
    public class ProcedureGame : ProcedureBase
    {
        private int m_formId;
        bool m_exit;
        private readonly Dictionary<GameMode, GameBase> m_Games = new Dictionary<GameMode, GameBase>();
        public GameBase m_CurrentGame { get; private set; }

        public override bool UseNativeDialog
        {
            get
            {
                return false;
            }
        }
        protected override void OnInit(ProcedureOwner procedureOwner)
        {
            base.OnInit(procedureOwner);
            m_Games.Add(GameMode.Common, new CommonGame());
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            ARKGameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            ARKGameEntry.Event.Subscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
            

            GameMode gameMode = (GameMode)procedureOwner.GetData<VarInt>(Constant.ProcedureData.GameMode).Value;
            m_CurrentGame = m_Games[gameMode];
            m_CurrentGame.Initialize();

           
        }


        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (m_exit)
            {
                m_exit = false;
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, (int)SceneId.Home);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
            if (m_CurrentGame != null) {
                if (m_CurrentGame.GameBegin)
                {
                    ARKGameEntry.UI.OpenUIForm(UIFormId.GameForm, this);
                }
                if (!m_CurrentGame.GameOver)
                {
                    m_CurrentGame.Update(elapseSeconds, realElapseSeconds);
                }
            }

        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            ARKGameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            ARKGameEntry.Event.Unsubscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
            if (m_formId > 0)
            {
                ARKGameEntry.UI.CloseUIForm(m_formId);
                m_formId = 0;
            }
            if (m_CurrentGame != null)
            {
                m_CurrentGame.Shutdown();
                m_CurrentGame = null;
            }
        }
        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
            m_Games.Clear();
        }

        #region Custom Event

        public void Exit()
        {
            Log.Info("Exit Game.");
            m_exit = true;
        }

        //public void OpenGameUIForm()
        //{
        //    ARKGameEntry.UI.OpenUIForm(UIFormId.GameForm, this);
        //}

        #endregion
        #region Callback

        private void OnOpenUIFormFailure(object sender, GameEventArgs e)
        {
            OpenUIFormFailureEventArgs ne = (OpenUIFormFailureEventArgs)e;

            Log.Error("Open ui form failed. error info =" + ne.ErrorMessage);
        }

        private void OnOpenUIFormSuccess(object sender, GameEventArgs e)
        {
            OpenUIFormSuccessEventArgs ne = (OpenUIFormSuccessEventArgs)e;
            if (ne.UserData != this)
            {
                return;
            }
            m_formId = ne.UIForm.SerialId;
            Log.Info("Open ui form success. name =" + ne.UIForm.name);
        }
        #endregion

    }
}