using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARKGame
{
    public class Hero : MonoBehaviour
    {
        public bool mIsUser;
        // Use this for initialization
        void Start()
        {
            if (mIsUser)
            {
                GameScene.Instance.GetComponent<Follower>().SetTarget(transform);
            }
        }
    }
}