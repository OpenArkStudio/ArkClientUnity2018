using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace AFCoreEx
{
    public abstract class AFIHeartBeatManager
    {
        public abstract void AddHeartBeat(string strHeartBeatName, float fTime, AFIHeartBeat.HeartBeatEventHandler handler, AFIDataList valueList);
		public abstract void Update(float fPassTime);
    }
}