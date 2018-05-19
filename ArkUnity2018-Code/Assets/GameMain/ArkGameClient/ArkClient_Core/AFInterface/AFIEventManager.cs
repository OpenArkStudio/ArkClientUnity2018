using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFCoreEx
{
	public abstract class AFIEventManager
	{
		public abstract void RegisterCallback(int nEventID, AFIEvent.EventHandler handler, AFIDataList valueList);
		public abstract void DoEvent(int nEventID, AFIDataList valueList);
        public abstract void RemoveCallback(int nEventID, AFIEvent.EventHandler handler, AFIDataList valueList);
    }
}
