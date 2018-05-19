using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace AFCoreEx
{
	public abstract class AFILogicEvent
    {
		public abstract void RegisterCallback(int nEventID, AFIEvent.EventHandler handler, AFIDataList valueList);
        public abstract void RemoveCallback(int nEventID, AFIEvent.EventHandler handler, AFIDataList valueList);
		public abstract void DoEvent(int nEventID, AFIDataList valueList);
    }
}