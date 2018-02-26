using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ARKGame
{
    public class Mover : MonoBehaviour
    {
        //移动速度
        public float mSpeed = 1.5f;
        public float mSmooth = 5f;
        public Vector2 mDirection = Vector2.zero;
        // Use this for initialization
        void Start()
        {
            
        }

        private void Update()
        {
            Vector3 target = transform.position + Time.deltaTime * mSpeed * new Vector3(mDirection.x,0,mDirection.y);
            transform.position = Vector3.Lerp(target,transform.position, Time.deltaTime*mSmooth);
        }
    }
}