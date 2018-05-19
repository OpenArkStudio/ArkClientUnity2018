using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace AFCoreEx
{
	public abstract class AFIHeartBeat
	{
		public delegate void HeartBeatEventHandler(AFIDENTID self, string strHeartBeat, float fTime, AFIDataList valueList);

		public abstract void RegisterCallback(AFIHeartBeat.HeartBeatEventHandler handler);
		public abstract bool Update(float fPassTime);
	}
}