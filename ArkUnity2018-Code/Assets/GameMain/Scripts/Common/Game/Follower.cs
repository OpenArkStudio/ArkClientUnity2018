using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARKGame
{
    //跟随者
    public class Follower : MonoBehaviour
    {
        [SerializeField] Vector3 mOffset;
        [SerializeField] float mSmooth = 5f;
        Transform mTarget;
        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (mTarget != null)
            {
                transform.position = Vector3.Lerp(transform.position, mTarget.position+mOffset, Time.deltaTime * mSmooth);
            }
        }

        public void SetTarget(Transform target)
        {
            mTarget = target;
        }
    }
}