using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace AFCoreEx
{
	public class AFCHeartBeat : AFIHeartBeat
	{

		public AFCHeartBeat(AFIDENTID self, string strHeartBeatName, float fTime, AFIDataList valueList)
		{
			mSelf = self;
			mstrHeartBeatName = strHeartBeatName;
			mfTime = fTime;
			mfOldTime = fTime;
			mArgValueList = valueList;
		}

		public override void RegisterCallback(AFIHeartBeat.HeartBeatEventHandler handler)
		{
			doHandlerDel += handler;
		}

		public override bool Update(float fPassTime)
		{
			mfTime -= fPassTime;
			if (mfTime < 0.0f)
			{
				if (null != doHandlerDel)
				{
					doHandlerDel(mSelf, mstrHeartBeatName, mfOldTime, mArgValueList);
				}
				return true;
			}

			return false;
		}

		AFIDENTID mSelf;
		string mstrHeartBeatName;
		float mfTime;
		float mfOldTime;
		AFIDataList mArgValueList;

		HeartBeatEventHandler doHandlerDel;
    }
}