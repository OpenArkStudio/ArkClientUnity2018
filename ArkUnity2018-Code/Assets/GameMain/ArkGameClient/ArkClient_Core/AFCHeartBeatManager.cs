using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace AFCoreEx
{
	public class AFCHeartBeatManager : AFIHeartBeatManager
    {
		public AFCHeartBeatManager(AFIDENTID self)
		{
			mSelf = self;
            mhtHeartBeat = new Dictionary<string, AFIHeartBeat>();
		}

        public override void AddHeartBeat(string strHeartBeatName, float fTime, AFIHeartBeat.HeartBeatEventHandler handler, AFIDataList valueList)
		{
			if (!mhtHeartBeat.ContainsKey(strHeartBeatName))
			{
                AFIHeartBeat xHeartBeat = new AFCHeartBeat(mSelf, strHeartBeatName, fTime, valueList);
                mhtHeartBeat.Add(strHeartBeatName, xHeartBeat);
                xHeartBeat.RegisterCallback(handler);
			}
		}

		public override void Update(float fPassTime)
		{

            AFIDataList keyList = null;

            foreach (KeyValuePair<string, AFIHeartBeat> kv in mhtHeartBeat)
            {
                AFIHeartBeat heartBeat = (AFIHeartBeat)kv.Value;
                if (heartBeat.Update(fPassTime))
                {
                    if (null == keyList)
                    {
                        keyList = new AFCDataList();
                    }

                    keyList.AddString((string)kv.Key);
                }
            }

            if (null != keyList)
            {
                for (int i = 0; i < keyList.Count(); i++)
                {
                    mhtHeartBeat.Remove(keyList.StringVal(i));
                }
            }
		}

		AFIDENTID mSelf;
        Dictionary<string, AFIHeartBeat> mhtHeartBeat;
    }
}