using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityGameFramework.Runtime;

namespace ARKGame
{
    public class MyHero: Hero
    {
        Camera m_mainCamera;
        DataNodeComponent m_dataNodeComponent;
        [SerializeField]
        float m_moveSpeed;
        //是否在游戏中
        [SerializeField]bool m_playing = false;
        public bool Playing { get { return m_playing; }set { m_playing = value; } }
        //mover ctrl
        private MoverCtrl m_moverCtrl;

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            GameScene.Instance.GetComponent<Follower>().SetTarget(transform);
            m_mainCamera = ARKGameEntry.Scene.MainCamera;
            m_dataNodeComponent = ARKGameEntry.DataNode;
            m_dataNodeComponent.SetData<VarFloat>(Constant.DataNodeData.ScreenDirectionX, 0f);
            m_dataNodeComponent.SetData<VarFloat>(Constant.DataNodeData.ScreenDirectionY, 0f);
            m_moveSpeed = 3.0f;

            m_moverCtrl = gameObject.GetOrAddComponent<MoverCtrl>();
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (m_playing)
            {
                //Vector3 forward = m_mainCamera.transform.forward;
                //forward.y = 0;
                //float x = m_dataNodeComponent.GetData<VarFloat>(Constant.DataNodeData.ScreenDirectionX);
                //float y = m_dataNodeComponent.GetData<VarFloat>(Constant.DataNodeData.ScreenDirectionY);
                //transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(x, 0, y) * m_moveSpeed, Time.deltaTime);
            }
        }

    }
}