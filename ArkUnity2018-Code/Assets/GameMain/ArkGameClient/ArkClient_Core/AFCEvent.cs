using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFCoreEx
{
	class AFCEvent : AFIEvent
	{
		public AFCEvent(AFIDENTID self, int nEventID, AFIDataList valueList)
		{
			mSelf = self;
			mnEventID = nEventID;
            mArgValueList = valueList;
		}

		public override void RegisterCallback(AFIEvent.EventHandler handler)
		{
			mHandlerDel += handler;
		}

		public override void DoEvent(AFIDataList valueList)
		{
			if (null != mHandlerDel)
			{
				mHandlerDel(mSelf, mnEventID, mArgValueList, valueList);
			}
		}

        public override void RemoveCallback(AFIEvent.EventHandler handler)
        {
            mHandlerDel -= handler;
        }

        AFIDENTID mSelf;
		int mnEventID;
		AFIDataList mArgValueList;

		AFIEvent.EventHandler mHandlerDel;
	}
}
