using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace ARKGame
{

    public class ScrollCircle : ScrollRect
    {
        protected Vector2 screenDirection = Vector2.zero;
        protected float mRadius = 0f;
        protected override void Start()
        {
            base.Start();

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
            screenDirection = contentPosition / mRadius;
            Debug.Log("Drag. contentPosition="+ screenDirection);
        }
        public override void OnBeginDrag(PointerEventData eventData)
        {
            base.OnBeginDrag(eventData);
            Debug.Log("Begin Drag. contentPosition=" + screenDirection);
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            base.OnEndDrag(eventData);
            screenDirection = Vector2.zero;
            Debug.Log("End Drag. contentPosition=" + screenDirection);
        }
    }
}