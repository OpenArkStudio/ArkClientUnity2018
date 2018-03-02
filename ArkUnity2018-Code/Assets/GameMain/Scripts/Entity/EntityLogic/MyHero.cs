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

        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            GameScene.Instance.GetComponent<Follower>().SetTarget(transform);
            m_mainCamera = ARKGameEntry.Scene.MainCamera;
            m_dataNodeComponent = ARKGameEntry.DataNode;

            m_moveSpeed = 3.0f;
        }
        protected override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            Vector3 forward = m_mainCamera.transform.forward;
            forward.y = 0;
            float x = m_dataNodeComponent.GetData<VarFloat>(Constant.DataNodeData.ScreenDirectionX);
            float y = m_dataNodeComponent.GetData<VarFloat>(Constant.DataNodeData.ScreenDirectionY);
            transform.position = Vector3.Lerp(transform.position, transform.position + new Vector3(x, 0, y)*m_moveSpeed, Time.deltaTime);
        }
    }
}