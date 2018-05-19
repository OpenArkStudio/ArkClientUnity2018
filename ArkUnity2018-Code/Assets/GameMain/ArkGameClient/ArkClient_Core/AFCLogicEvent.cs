using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace AFCoreEx
{
	public class AFCLogicEvent : AFILogicEvent
    {
		public AFCLogicEvent()
		{

		}

        #region Instance
        private static AFCLogicEvent _Instance = null;
        public static AFCLogicEvent Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new AFCLogicEvent();
                }
                return _Instance;
            }
        }
        #endregion

		public override void RegisterCallback(int nEventID, AFIEvent.EventHandler handler, AFIDataList valueList)
		{
			mxEventManager.RegisterCallback(nEventID, handler, valueList);
		}

        public override void RemoveCallback(int nEventID, AFIEvent.EventHandler handler, AFIDataList valueList)
        {
             mxEventManager.RemoveCallback(nEventID, handler, valueList);
        }

		public override void DoEvent(int nEventID, AFIDataList valueList)
		{
			mxEventManager.DoEvent(nEventID, valueList);
		}

		AFIEventManager mxEventManager = new AFCEventManager(new AFIDENTID());
    }
}