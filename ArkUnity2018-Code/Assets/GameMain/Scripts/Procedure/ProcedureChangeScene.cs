using System.Collections;
using System.Collections.Generic;
using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Procedure;
using UnityGameFramework.Runtime;
using GameFramework.Event;
using System;
using GameFramework;
using GameFramework.DataTable;

public class ProcedureChangeScene : ProcedureBase {
    private bool m_IsChangeSceneComplete;
    private int m_BackgroundMusicId;

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
        ARKGameEntry.Event.Subscribe(LoadSceneSuccessEventArgs.EventId, OnLoadSceneSuccess);
        ARKGameEntry.Event.Subscribe(LoadSceneFailureEventArgs.EventId, OnLoadSceneFailure);
        ARKGameEntry.Event.Subscribe(LoadSceneUpdateEventArgs.EventId, OnLoadSceneUpdate);
        ARKGameEntry.Event.Subscribe(LoadSceneDependencyAssetEventArgs.EventId, OnLoadSceneDependencyAsset);

        //停止所有声音
        ARKGameEntry.Sound.StopAllLoadingSounds();
        ARKGameEntry.Sound.StopAllLoadedSounds();

        //隐藏所有实体
        ARKGameEntry.Entity.HideAllLoadingEntities();
        ARKGameEntry.Entity.HideAllLoadedEntities();

        //卸载所有场景
        string[] loadedSceneAssetNames = ARKGameEntry.Scene.GetLoadedSceneAssetNames();
        for(int i = 0; i< loadedSceneAssetNames.Length; i++)
        {
            ARKGameEntry.Scene.UnloadScene(loadedSceneAssetNames[i]);
        }
        //还原游戏速度
        ARKGameEntry.Base.ResetNormalGameSpeed();

        int sceneId = procedureOwner.GetData<VarInt>(Constant.ProcedureData.NextSceneId).Value;
        IDataTable<DRScene> dtScene = ARKGameEntry.DataTable.GetDataTable<DRScene>();
        DRScene drScene = dtScene.GetDataRow(sceneId);
        if(drScene == null)
        {
            Log.Warning("Can not load scene '{0}' from data table.", sceneId.ToString());
            return;
        }
        ARKGameEntry.Scene.LoadScene(AssetUtility.GetSceneAsset(drScene.AssetName), this);
    }

    protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
    {
        base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);

    }
    protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
    {
        base.OnLeave(procedureOwner, isShutdown);
    }
    protected override void OnDestroy(ProcedureOwner procedureOwner)
    {
        base.OnDestroy(procedureOwner);
    }

    #region Scene Event Callback

    private void OnLoadSceneDependencyAsset(object sender, GameEventArgs e)
    {
        LoadSceneDependencyAssetEventArgs ne = (LoadSceneDependencyAssetEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        Log.Info("Load scene '{0}' dependency asset '{1}', count '{2}/{3}'.", ne.SceneAssetName, ne.DependencyAssetName, ne.LoadedCount.ToString(), ne.TotalCount.ToString());

    }

    private void OnLoadSceneUpdate(object sender, GameEventArgs e)
    {
        LoadSceneUpdateEventArgs ne = (LoadSceneUpdateEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        Log.Info("Load scene '{0}' update, progress '{1}'.", ne.SceneAssetName, ne.Progress.ToString("P2"));
    }

    private void OnLoadSceneFailure(object sender, GameEventArgs e)
    {
        LoadSceneFailureEventArgs ne = (LoadSceneFailureEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        Log.Error("Load scene '{0}' failure, error message '{1}'.", ne.SceneAssetName, ne.ErrorMessage);
    }

    private void OnLoadSceneSuccess(object sender, GameEventArgs e)
    {
        LoadSceneSuccessEventArgs ne = (LoadSceneSuccessEventArgs)e;
        if (ne.UserData != this)
        {
            return;
        }

        Log.Info("Load scene '{0}' OK.", ne.SceneAssetName);

        if (m_BackgroundMusicId > 0)
        {
            ARKGameEntry.Sound.PlayMusic(m_BackgroundMusicId);
        }

        m_IsChangeSceneComplete = true;
    }
    #endregion
}
