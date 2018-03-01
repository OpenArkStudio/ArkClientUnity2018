using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARKGame
{
    public class MyHero: Hero
    {
        protected override void OnShow(object userData)
        {
            base.OnShow(userData);
            GameScene.Instance.GetComponent<Follower>().SetTarget(transform);
        }
    }
}