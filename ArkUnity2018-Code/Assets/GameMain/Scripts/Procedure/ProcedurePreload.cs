using System.Collections;
using System.Collections.Generic;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using System;
using GameFramework;

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
    }
    private void LoadDataTable(string dataTableName)
    {
        m_LoadedFlag.Add(string.Format("DataTable.{0}", dataTableName), false);
        ARKGameEntry.DataTable.LoadDataTable(dataTableName, this);
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
