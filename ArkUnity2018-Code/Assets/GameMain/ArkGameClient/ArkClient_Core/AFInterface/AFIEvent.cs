using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFCoreEx
{
	public abstract class AFIEvent
	{
		public delegate void EventHandler(AFIDENTID self, int nEventID, AFIDataList initValueList, AFIDataList valueList);

		public abstract void RegisterCallback(AFIEvent.EventHandler handler);
		public abstract void DoEvent(AFIDataList valueList);
        public abstract void RemoveCallback(AFIEvent.EventHandler handler);
	}

}
