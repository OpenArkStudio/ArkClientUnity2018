using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityGameFramework.Runtime;
using GameFramework;

namespace ARKGame
{

    public class ScrollCircle : ScrollRect
    {
        protected Vector2 screenDirection = Vector2.zero;
        protected float mRadius = 0f;
        DataNodeComponent dataNodeComponent; //= ARKGameEntry.DataNode;
        protected override void Start()
        {
            base.Start();
            dataNodeComponent = ARKGameEntry.DataNode;
            if (dataNodeComponent == null)
            {
                Log.Error("dataNodeComponent == null!");
                return;
            }
            dataNodeComponent.SetData<VarFloat>(Constant.DataNodeData.ScreenDirectionX, screenDirection.x);
            dataNodeComponent.SetData<VarFloat>(Constant.DataNodeData.ScreenDirectionY, screenDirection.y);
            mRadius = (transform as RectTransform).sizeDelta.x * 0.5f;
            Debug.Log("mRadius = "+mRadius);
        }
        public override void OnDrag(PointerEventData eventData)
        {
            base.OnDrag(eventData);
            var contentPosition = content.anchoredPosition;
            if(contentPosition.magnitude > mRadius)
            {
                contentPosition = contentPosition.normalized * mRadius;
                SetContentAnchoredPosition(contentPosition);
            }
            screenDirection = (contentPosition / mRadius).normalized;
            //Debug.Log("Drag. contentPosition="+ screenDirection);
            dataNodeComponent.SetData<VarFloat>(Constant.DataNodeData.ScreenDirectionX, screenDirection.x);
            dataNodeComponent.SetData<VarFloat>(Constant.DataNodeData.ScreenDirectionY, screenDirection.y);
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            //Debug.Log("Begin Drag. contentPosition=" + screenDirection);
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            screenDirection = Vector2.zero;
            //Debug.Log("End Drag. contentPosition=" + screenDirection);
            
            dataNodeComponent.SetData<VarFloat>(Constant.DataNodeData.ScreenDirectionX, screenDirection.x);
            dataNodeComponent.SetData<VarFloat>(Constant.DataNodeData.ScreenDirectionY, screenDirection.y);
        }
    }
}