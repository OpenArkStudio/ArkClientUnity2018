using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ARKGame
{
    public class GameScene : MonoBehaviour
    {
        private static GameScene _instance;
        public static GameScene Instance { get { return _instance; } }
        private void Awake()
        {
            _instance = this;
        }
        private void OnDestroy()
        {
            _instance = null;
        }
        private void Start()
        {
            
        }
    }
}