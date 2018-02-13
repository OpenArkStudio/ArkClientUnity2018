using System.Collections;
using System.Collections.Generic;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using System;
using GameFramework;
using GameFramework.Resource;
using UnityEngine;

public class ProcedurePreload : ProcedureBase {

    private Dictionary<string, bool> m_LoadedFlag = new Dictionary<string, bool>();

    public override bool UseNativeDialog
    {
        get
        {
            return false;
        }
    }

    protected override void OnEnter(ProcedureOwner procedureOwner)
    {
        base.OnEnter(procedureOwner);

        ARKGameEntry.Event.Subscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
        ARKGameEntry.Event.Subscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);

        PreloadResources();
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
        ChangeState<ProcedureChangeScene>(procedureOwner);
        
    }
    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        ARKGameEntry.Event.Unsubscribe(LoadDataTableSuccessEventArgs.EventId, OnLoadDataTableSuccess);
        ARKGameEntry.Event.Unsubscribe(LoadDataTableFailureEventArgs.EventId, OnLoadDataTableFailure);
        base.OnLeave(procedureOwner, isShutdown);
    }
    protected override void OnDestroy(ProcedureOwner procedureOwner)
    {
        base.OnDestroy(procedureOwner);
    }
    #region Preload
    
    private void PreloadResources()
    {
        LoadDataTable("Scene");
        LoadDataTable("UIForm");

        LoadDictionary("Default");
        LoadFont("MainFont");
    }
    private void LoadDataTable(string dataTableName)
    {
        m_LoadedFlag.Add(string.Format("DataTable.{0}", dataTableName), false);
        ARKGameEntry.DataTable.LoadDataTable(dataTableName, this);
    }
    private void LoadDictionary(string dictionaryName)
    {
        m_LoadedFlag.Add(string.Format("Dictionary.{0}", dictionaryName), false);
        ARKGameEntry.Localization.LoadDictionary(dictionaryName, this);
    }
    private void LoadFont(string fontName)
    {
        m_LoadedFlag.Add(string.Format("Font.{0}", fontName), false);
        ARKGameEntry.Resource.LoadAsset(AssetUtility.GetFontAsset(fontName), new LoadAssetCallbacks(
            (assetName, asset, duration, userData) =>
            {
                m_LoadedFlag[string.Format("Font.{0}", fontName)] = true;
                UGuiForm.SetMainFont((Font)asset);
                Log.Info("Load font '{0}' OK.", fontName);
            },

            (assetName, status, errorMessage, userData) =>
            {
                Log.Error("Can not load font '{0}' from '{1}' with error message '{2}'.", fontName, assetName, errorMessage);
            }));
    }
    #endregion

    #region Event Callback


    private void OnLoadDataTableFailure(object sender, GameEventArgs e)
    {
        LoadDataTableFailureEventArgs ne = (LoadDataTableFailureEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        Log.Error("Can not load data table '{0}' from '{1}' with error message '{2}'.", ne.DataTableName, ne.DataTableAssetName, ne.ErrorMessage);

    }

    private void OnLoadDataTableSuccess(object sender, GameEventArgs e)
    {
        LoadDataTableSuccessEventArgs ne = (LoadDataTableSuccessEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        m_LoadedFlag[string.Format("DataTable.{0}", ne.DataTableName)] = true;
        Log.Info("Load data table '{0}' OK.", ne.DataTableName);
    }
    #endregion
}
