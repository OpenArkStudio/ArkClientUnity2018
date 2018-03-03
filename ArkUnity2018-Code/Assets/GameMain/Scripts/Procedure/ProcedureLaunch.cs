using ProcedureOwner = GameFramework.Fsm.IFsm<GameFramework.Procedure.IProcedureManager>;
using GameFramework.Procedure;
using System.Collections;
using System.Collections.Generic;
using UnityGameFramework.Runtime;
namespace ARKGame
{
    public class ProcedureLaunch : ProcedureBase
    {

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
            // 构建信息：发布版本时，把一些数据以 Json 的格式写入 Assets/GameMain/Configs/BuildInfo.txt，供游戏逻辑读取。
            ARKGameEntry.Config.InitBuildInfo();
            // 默认字典：加载默认字典文件 Assets/GameMain/Configs/DefaultDictionary.xml。
            // 此字典文件记录了资源更新前使用的各种语言的字符串，会随 App 一起发布，故不可更新。
            ARKGameEntry.Config.InitDefaultDictionary();
            //设置场景
            procedureOwner.SetData<VarInt>(Constant.ProcedureData.NextSceneId, (int)SceneId.Head);
        }

        protected override void OnUpdate(ProcedureOwner procedureOwner, float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(procedureOwner, elapseSeconds, realElapseSeconds);
            // TODO: 增加一个 Splash 动画，这里先跳过
            // 编辑器模式下，直接进入预加载流程；否则，检查一下版本
            ChangeState(procedureOwner, ARKGameEntry.Base.EditorResourceMode ? typeof(ProcedurePreload) : typeof(ProcedureCheckVersion));

            //ChangeState<ProcedurePreload>(procedureOwner);
        }
        protected override void OnLeave(ProcedureOwner procedureOwner, bool isShutdown)
        {
            base.OnLeave(procedureOwner, isShutdown);
        }
        protected override void OnDestroy(ProcedureOwner procedureOwner)
        {
            base.OnDestroy(procedureOwner);
        }
    }
}