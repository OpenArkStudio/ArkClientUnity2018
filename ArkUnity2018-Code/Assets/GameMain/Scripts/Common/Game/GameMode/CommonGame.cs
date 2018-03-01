using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARKGame
{
    public class CommonGame : GameBase
    {
        
        public override GameMode GameMode
        {
            get
            {
                return GameMode.Common;
            }
        }
        public override void Initialize()
        {
            base.Initialize();
            
            
        }
    }
}