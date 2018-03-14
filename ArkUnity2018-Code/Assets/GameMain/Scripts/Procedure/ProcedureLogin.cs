using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using GameFramework;
using System.IO;
using AFTCPClient;
using Google.Protobuf;

namespace ARKGame
{
    public class ProcedureLogin : ProcedureBase
    {
         
        bool m_enterHome;
        int m_formId;
        public override bool UseNativeDialog
        {
            get
            {
                return true;
            }
        }

        protected override void OnEnter(ProcedureOwner procedureOwner)
        {
            base.OnEnter(procedureOwner);
            ARKGameEntry.Event.Subscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            ARKGameEntry.Event.Subscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
            //ARKGameEntry.Event.Subscribe(NetworkConnectedEventArgs.EventId, OnNetworkConnected);
            //ARKGameEntry.Event.Subscribe(NetworkErrorEventArgs.EventId, OnNetworkError);
            //ARKGameEntry.Event.Subscribe(NetworkSendPacketEventArgs.EventId, OnNetworkSendPacket);
            ARKGameEntry.UI.OpenUIForm(UIFormId.LoginForm,this);

            m_enterHome = false;
            m_formId = 0;
            ////connect server

            ARKGameEntry.AFNet.CreateChannel(Constant.Network.LoginChannel).Connect("127.0.0.1", 14001);
            ARKGameEntry.AFNet.SetChannel(Constant.Network.LoginChannel);
            ARKGameEntry.AFNet.CreateChannel(Constant.Network.WorldChannel);
            ARKGameEntry.AFNet.CreateChannel(Constant.Network.GameChannel);

        }

        

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            if (m_enterHome)
            {
                m_enterHome = false;
                procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, (int)SceneId.Home);
                ChangeState<ProcedureChangeScene>(procedureOwner);
            }
        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
            ARKGameEntry.Event.Unsubscribe(OpenUIFormSuccessEventArgs.EventId, OnOpenUIFormSuccess);
            ARKGameEntry.Event.Unsubscribe(OpenUIFormFailureEventArgs.EventId, OnOpenUIFormFailure);
            if (m_formId > 0)
            {
                m_formId = 0;
                ARKGameEntry.UI.CloseAllLoadedUIForms();
            }
        }
        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }

        #region Event Callback

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

        #region Button Click

        public void Login()
        {
            Log.Info("Login.");
            //m_enterHome = true;
           ARKGameEntry.AFNet.LoginPB("mengdong", "123456", "");
        }
        public void LoginSuccess()
        {
            m_enterHome = true;
        }
        public void ShowRoleListForm()
        {
            ARKGameEntry.UI.OpenUIForm(UIFormId.RoleListForm, this);
        }
        public void ShowWorldServerListForm()
        {
            ARKGameEntry.UI.OpenUIForm(UIFormId.WorldServerListForm, this);
        }
        public void ShowGameServerListForm()
        {
            ARKGameEntry.UI.OpenUIForm(UIFormId.GameServerListForm, this);

        }
        public void ConnectWorldServer()
        {
            ARKGameEntry.AFNet.Disconnect(Constant.Network.LoginChannel);
            ARKGameEntry.Event.Subscribe(NetworkConnectedEventArgs.EventId, OnConnectedWorldServer);
            ARKGameEntry.Event.Subscribe(NetworkErrorEventArgs.EventId, OnNetworkWorldServerError);
            ARKGameEntry.AFNet.ConnectChannel(Constant.Network.WorldChannel, ARKGameEntry.AFNet.m_worldIP, ARKGameEntry.AFNet.m_worldPort);
            ARKGameEntry.AFNet.SetChannel(Constant.Network.WorldChannel);
        }

      

        private void OnConnectedWorldServer(object sender, GameEventArgs e)
        {
            NetworkConnectedEventArgs ne = e as NetworkConnectedEventArgs;
            if(ne.NetworkChannel.Name == Constant.Network.WorldChannel)
            {
                Log.Debug("Connect World Server Success!");
                ARKGameEntry.AFNet.RequireVerifyWorldKey(ARKGameEntry.AFNet.m_account, ARKGameEntry.AFNet.m_worldKey);
            }
            ARKGameEntry.Event.Unsubscribe(NetworkConnectedEventArgs.EventId, OnConnectedWorldServer);
            ARKGameEntry.Event.Unsubscribe(NetworkErrorEventArgs.EventId, OnNetworkWorldServerError);
        }
        private void OnNetworkWorldServerError(object sender, GameEventArgs e)
        {
            Log.Debug("Connect World Server Error!");
        }
        #endregion

    }
}

