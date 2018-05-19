using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace AFCoreEx
{
	public class AFCEventManager : AFIEventManager
    {
		public AFCEventManager(AFIDENTID self)
		{
			mSelf = self;
            mhtEvent = new Dictionary<int, AFIEvent>();
		}

		public override void RegisterCallback(int nEventID, AFIEvent.EventHandler handler, AFIDataList valueList)
		{
			if (!mhtEvent.ContainsKey(nEventID))
			{
				mhtEvent.Add(nEventID, new AFCEvent(mSelf, nEventID, valueList));
			}

			AFIEvent identEvent = (AFIEvent)mhtEvent[nEventID];
			identEvent.RegisterCallback(handler);

		}

		public override void DoEvent(int nEventID, AFIDataList valueList)
		{
			if (mhtEvent.ContainsKey(nEventID))
			{
				AFIEvent identEvent = (AFIEvent)mhtEvent[nEventID];
				identEvent.DoEvent(valueList);
			}
		}

        public override void RemoveCallback(int nEventID, AFIEvent.EventHandler handler, AFIDataList valueList)
        {
            if (!mhtEvent.ContainsKey(nEventID))
            {
                return;
            }

            AFIEvent identEvent = (AFIEvent)mhtEvent[nEventID];
            if (null != identEvent)
            {
                identEvent.RemoveCallback(handler);
            }
        }


        AFIDENTID mSelf;
        Dictionary<int, AFIEvent> mhtEvent;
    }
}