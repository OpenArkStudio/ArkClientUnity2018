using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace AFCoreEx
{
	public class AFCKernel : AFIKernel
	{
		#region Instance
		private static AFIKernel _Instance = null;
        private static readonly object _syncLock = new object();
		public static AFIKernel Instance
		{
			get
			{
                lock (_syncLock)
                {
                    if (_Instance == null)
                    {
                        _Instance = new AFCKernel();
                    }
                    return _Instance;
                }
			}
		}
		#endregion

		public AFCKernel()
		{
            mhtObject = new Dictionary<AFIDENTID, AFIObject>();
            mhtClassHandleDel = new Dictionary<string, ClassHandleDel>();

            mxLogicClassManager = new AFCLogicClassManager();
            mxElementManager = new AFCElementManager();
		}

		~AFCKernel()
		{
			mhtObject = null;
            mxElementManager = null;
            mxLogicClassManager = null;
		}

		public override bool AddHeartBeat(AFIDENTID self, string strHeartBeatName, AFIHeartBeat.HeartBeatEventHandler handler, float fTime, AFIDataList valueList)
		{
            AFIObject xGameObject = GetObject(self);
            if (null != xGameObject)
            {
                xGameObject.GetHeartBeatManager().AddHeartBeat(strHeartBeatName, fTime, handler, valueList);
            }
			return true;
		}

		public override void RegisterPropertyCallback(AFIDENTID self, string strPropertyName, AFIProperty.PropertyEventHandler handler)
		{
			AFIObject xGameObject = GetObject(self);
			if (null != xGameObject)
			{
				xGameObject.GetPropertyManager().RegisterCallback(strPropertyName, handler);
			}
		}

		public override void RegisterRecordCallback(AFIDENTID self, string strRecordName, AFIRecord.RecordEventHandler handler)
		{
			AFIObject xGameObject = GetObject(self);
			if (null != xGameObject)
			{
				xGameObject.GetRecordManager().RegisterCallback(strRecordName, handler);
			}
		}

		public override void RegisterClassCallBack(string strClassName, AFIObject.ClassEventHandler handler)
		{
			if(mhtClassHandleDel.ContainsKey(strClassName))
			{
				ClassHandleDel xHandleDel = (ClassHandleDel)mhtClassHandleDel[strClassName];
				xHandleDel.AddDel(handler);
				
			}
			else
			{
				ClassHandleDel xHandleDel = new ClassHandleDel();
				xHandleDel.AddDel(handler);
				mhtClassHandleDel[strClassName] = xHandleDel;
			}
		}

		public override void RegisterEventCallBack(AFIDENTID self, int nEventID, AFIEvent.EventHandler handler, AFIDataList valueList)
		{
			AFIObject xGameObject = GetObject(self);
			if (null != xGameObject)
			{
				xGameObject.GetEventManager().RegisterCallback(nEventID, handler, valueList);
			}
		}

		public override bool FindHeartBeat(AFIDENTID self, string strHeartBeatName)
		{
			AFIObject xGameObject = GetObject(self);
			if (null != xGameObject)
			{
				//gameObject.GetHeartBeatManager().AddHeartBeat()
			}

			return false;
		}

		public override bool RemoveHeartBeat(AFIDENTID self, string strHeartBeatName)
		{
			return true;
		}

        public override bool UpDate(float fTime)
        {
            foreach (AFIDENTID id in mhtObject.Keys)
            {
                AFIObject xGameObject = (AFIObject)mhtObject[id];
                xGameObject.GetHeartBeatManager().Update(fTime);
            }

            return true;
        }
		/////////////////////////////////////////////////////////////

		//public override bool AddRecordCallBack( AFIDENTID self, string strRecordName, RECORD_EVENT_FUNC cb );

		//public override bool AddPropertyCallBack( AFIDENTID self, string strCriticalName, PROPERTY_EVENT_FUNC cb );

		//     public override bool AddClassCallBack( string strClassName, CLASS_EVENT_FUNC cb );
		//
		//     public override bool RemoveClassCallBack( string strClassName, CLASS_EVENT_FUNC cb );

		/////////////////////////////////////////////////////////////////

		public override AFIObject GetObject(AFIDENTID ident)
		{
            if (null != ident && mhtObject.ContainsKey(ident))
			{
				return (AFIObject)mhtObject[ident];
			}

			return null;
		}

		public override AFIObject CreateObject(AFIDENTID self, int nContainerID, int nGroupID, string strClassName, string strConfigIndex, AFIDataList arg)
		{
			if (!mhtObject.ContainsKey(self))
			{
				AFIObject xNewObject = new AFCObject(self, nContainerID, nGroupID, strClassName, strConfigIndex);
				mhtObject.Add(self, xNewObject);

                AFCDataList varConfigID = new AFCDataList();
                varConfigID.AddString(strConfigIndex);
                xNewObject.GetPropertyManager().AddProperty("ConfigID", varConfigID);

                AFCDataList varConfigClass = new AFCDataList();
                varConfigClass.AddString(strClassName);
                xNewObject.GetPropertyManager().AddProperty("ClassName", varConfigClass);

                if (arg.Count() % 2 == 0)
                {
                    for (int i = 0; i < arg.Count() - 1; i += 2)
                    {
                        string strPropertyName = arg.StringVal(i);
                        AFIDataList.VARIANT_TYPE eType = arg.GetType(i + 1);
                        switch (eType)
                        {
                            case AFIDataList.VARIANT_TYPE.VTYPE_INT:
                                {
                                    AFIDataList xDataList = new AFCDataList();
                                    xDataList.AddInt64(arg.Int64Val(i+1));
                                    xNewObject.GetPropertyManager().AddProperty(strPropertyName, xDataList);
                                }
                                break;
                            case AFIDataList.VARIANT_TYPE.VTYPE_FLOAT:
                                {
                                    AFIDataList xDataList = new AFCDataList();
                                    xDataList.AddFloat(arg.FloatVal(i + 1));
                                    xNewObject.GetPropertyManager().AddProperty(strPropertyName, xDataList);
                                }
                                break;
                            case AFIDataList.VARIANT_TYPE.VTYPE_DOUBLE:
                                {
                                    AFIDataList xDataList = new AFCDataList();
                                    xDataList.AddDouble(arg.DoubleVal(i + 1));
                                    xNewObject.GetPropertyManager().AddProperty(strPropertyName, xDataList);
                                }
                                break;
                            case AFIDataList.VARIANT_TYPE.VTYPE_STRING:
                                {
                                    AFIDataList xDataList = new AFCDataList();
                                    xDataList.AddString(arg.StringVal(i + 1));
                                    xNewObject.GetPropertyManager().AddProperty(strPropertyName, xDataList);
                                }
                                break;
                            case AFIDataList.VARIANT_TYPE.VTYPE_OBJECT:
                                {
                                    AFIDataList xDataList = new AFCDataList();
                                    xDataList.AddObject(arg.ObjectVal(i + 1));
                                    xNewObject.GetPropertyManager().AddProperty(strPropertyName, xDataList);
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                InitProperty(self, strClassName);
                InitRecord(self, strClassName);

                if (mhtClassHandleDel.ContainsKey(strClassName))
                {
                    ClassHandleDel xHandleDel = (ClassHandleDel)mhtClassHandleDel[strClassName];
                    if (null != xHandleDel && null != xHandleDel.GetHandler())
                    {
                        AFIObject.ClassEventHandler xHandlerList = xHandleDel.GetHandler();
                        xHandlerList(self, nContainerID, nGroupID, AFIObject.CLASS_EVENT_TYPE.OBJECT_CREATE, strClassName, strConfigIndex);
                        xHandlerList(self, nContainerID, nGroupID, AFIObject.CLASS_EVENT_TYPE.OBJECT_LOADDATA, strClassName, strConfigIndex);
                        xHandlerList(self, nContainerID, nGroupID, AFIObject.CLASS_EVENT_TYPE.OBJECT_CREATE_FINISH, strClassName, strConfigIndex);
                    }
                }

                //AFCLog.Instance.Log(AFCLog.LOG_LEVEL.DEBUG, "Create object: " + self.ToString() + " ClassName: " + strClassName + " SceneID: " + nContainerID + " GroupID: " + nGroupID);
				return xNewObject;
			}

			return null;
		}


		public override bool DestroyObject(AFIDENTID self)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];

				string strClassName = xGameObject.ClassName();

				ClassHandleDel xHandleDel = (ClassHandleDel)mhtClassHandleDel[strClassName];
                if (null != xHandleDel && null != xHandleDel.GetHandler())
                {
					AFIObject.ClassEventHandler xHandlerList = xHandleDel.GetHandler();
                    xHandlerList(self, xGameObject.ContainerID(), xGameObject.GroupID(), AFIObject.CLASS_EVENT_TYPE.OBJECT_DESTROY, xGameObject.ClassName(), xGameObject.ConfigIndex());
                }
				mhtObject.Remove(self);

                //AFCLog.Instance.Log(AFCLog.LOG_LEVEL.DEBUG, "Destroy object: " + self.ToString() + " ClassName: " + strClassName + " SceneID: " + xGameObject.ContainerID() + " GroupID: " + xGameObject.GroupID());

				return true;
			}

			return false;
		}

		public override bool FindProperty(AFIDENTID self, string strPropertyName)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.FindProperty(strPropertyName);
			}

			return false;
		}

        public override bool SetPropertyInt(AFIDENTID self, string strPropertyName, Int64 nValue)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.SetPropertyInt(strPropertyName, nValue);
			}

			return false;
		}

		public override bool SetPropertyFloat(AFIDENTID self, string strPropertyName, float fValue)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.SetPropertyFloat(strPropertyName, fValue);
			}

			return false;
		}

		public override bool SetPropertyDouble(AFIDENTID self, string strPropertyName, double dValue)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.SetPropertyDouble(strPropertyName, dValue);
			}

			return false;
		}

		public override bool SetPropertyString(AFIDENTID self, string strPropertyName, string strValue)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.SetPropertyString(strPropertyName, strValue);
			}

			return false;
		}

		public override bool SetPropertyObject(AFIDENTID self, string strPropertyName, AFIDENTID objectValue)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.SetPropertyObject(strPropertyName, objectValue);
			}

			return false;
		}


		public override Int64 QueryPropertyInt(AFIDENTID self, string strPropertyName)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.QueryPropertyInt(strPropertyName);
			}

			return 0;
		}

		public override float QueryPropertyFloat(AFIDENTID self, string strPropertyName)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.QueryPropertyFloat(strPropertyName);
			}

			return 0.0f;
		}

		public override double QueryPropertyDouble(AFIDENTID self, string strPropertyName)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.QueryPropertyDouble(strPropertyName);
			}

			return 0.0;
		}

		public override string QueryPropertyString(AFIDENTID self, string strPropertyName)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.QueryPropertyString(strPropertyName);
			}

			return "";
		}

		public override AFIDENTID QueryPropertyObject(AFIDENTID self, string strPropertyName)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.QueryPropertyObject(strPropertyName);
			}

			return new AFIDENTID();
		}


		public override AFIRecord FindRecord(AFIDENTID self, string strRecordName)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.GetRecordManager().GetRecord(strRecordName);
			}

			return null;
		}


        public override bool SetRecordInt(AFIDENTID self, string strRecordName, int nRow, int nCol, Int64 nValue)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.SetRecordInt(strRecordName, nRow, nCol, nValue);
			}

			return false;
		}

		public override bool SetRecordFloat(AFIDENTID self, string strRecordName, int nRow, int nCol, float fValue)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.SetRecordFloat(strRecordName, nRow, nCol, fValue);
			}

			return false;
		}

		public override bool SetRecordDouble(AFIDENTID self, string strRecordName, int nRow, int nCol, double dwValue)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.SetRecordDouble(strRecordName, nRow, nCol, dwValue);
			}

			return false;
		}

		public override bool SetRecordString(AFIDENTID self, string strRecordName, int nRow, int nCol, string strValue)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.SetRecordString(strRecordName, nRow, nCol, strValue);
			}

			return false;
		}

		public override bool SetRecordObject(AFIDENTID self, string strRecordName, int nRow, int nCol, AFIDENTID objectValue)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.SetRecordObject(strRecordName, nRow, nCol, objectValue);
			}

			return false;
		}


        public override Int64 QueryRecordInt(AFIDENTID self, string strRecordName, int nRow, int nCol)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.QueryRecordInt(strRecordName, nRow, nCol);
			}

			return 0;
		}

		public override float QueryRecordFloat(AFIDENTID self, string strRecordName, int nRow, int nCol)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.QueryRecordFloat(strRecordName, nRow, nCol);
			}

			return 0.0f;
		}

		public override double QueryRecordDouble(AFIDENTID self, string strRecordName, int nRow, int nCol)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.QueryRecordDouble(strRecordName, nRow, nCol);
			}

			return 0.0;
		}

		public override string QueryRecordString(AFIDENTID self, string strRecordName, int nRow, int nCol)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.QueryRecordString(strRecordName, nRow, nCol);
			}

			return "";
		}

		public override AFIDENTID QueryRecordObject(AFIDENTID self, string strRecordName, int nRow, int nCol)
		{
			if (mhtObject.ContainsKey(self))
			{
				AFIObject xGameObject = (AFIObject)mhtObject[self];
				return xGameObject.QueryRecordObject(strRecordName, nRow, nCol);
			}

			return new AFIDENTID();
		}
		
		public override AFIDataList GetObjectList()
		{
			AFIDataList varData = new AFCDataList();
            foreach (KeyValuePair<AFIDENTID, AFIObject> kv in mhtObject)
            {
                varData.AddObject(kv.Key);				
            }

			return varData;
		}
        public override int FindRecordRow(AFIDENTID self, string strRecordName, int nCol, int nValue)
        {
            if (mhtObject.ContainsKey(self))
            {
                AFIObject xGameObject = (AFIObject)mhtObject[self];
                AFCoreEx.AFIRecord xRecord = xGameObject.GetRecordManager().GetRecord(strRecordName);
                if (null != xRecord)
                {
                    return xRecord.FindInt(nCol, nValue);
                }
            }

            return -1;
        }

        public override int FindRecordRow(AFIDENTID self, string strRecordName, int nCol, float fValue)
        {
            if (mhtObject.ContainsKey(self))
            {
                AFIObject xGameObject = (AFIObject)mhtObject[self];
                AFCoreEx.AFIRecord xRecord = xGameObject.GetRecordManager().GetRecord(strRecordName);
                if (null != xRecord)
                {
                    return xRecord.FindFloat(nCol, fValue);
                }
            }

            return -1;
        }

        public override int FindRecordRow(AFIDENTID self, string strRecordName, int nCol, double fValue)
        {
            if (mhtObject.ContainsKey(self))
            {
                AFIObject xGameObject = (AFIObject)mhtObject[self];
                AFCoreEx.AFIRecord xRecord = xGameObject.GetRecordManager().GetRecord(strRecordName);
                if (null != xRecord)
                {
                    return xRecord.FindDouble(nCol, fValue);
                }
            }

            return -1;
        }

        public override int FindRecordRow(AFIDENTID self, string strRecordName, int nCol, string strValue)
        {
            if (mhtObject.ContainsKey(self))
            {
                AFIObject xGameObject = (AFIObject)mhtObject[self];
                AFCoreEx.AFIRecord xRecord = xGameObject.GetRecordManager().GetRecord(strRecordName);
                if (null != xRecord)
                {
                    return xRecord.FindString(nCol, strValue);
                }
            }

            return -1;
        }

        public override int FindRecordRow(AFIDENTID self, string strRecordName, int nCol, AFIDENTID nValue)
        {
            if (mhtObject.ContainsKey(self))
            {
                AFIObject xGameObject = (AFIObject)mhtObject[self];
                AFCoreEx.AFIRecord xRecord = xGameObject.GetRecordManager().GetRecord(strRecordName);
                if (null != xRecord)
                {
                    return xRecord.FindObject(nCol, nValue);
                }
            }

            return -1;
        }

        void InitProperty(AFIDENTID self, string strClassName)
        {
            AFILogicClass xLogicClass = AFCLogicClassManager.Instance.GetElement(strClassName);
            if (null == xLogicClass)
            {
                return;
            }

            AFIDataList xDataList = xLogicClass.GetPropertyManager().GetPropertyList();
            for (int i = 0; i < xDataList.Count(); ++i )
            {
                string strPropertyName = xDataList.StringVal(i);
                AFIProperty xProperty = xLogicClass.GetPropertyManager().GetProperty(strPropertyName);
  
                AFIObject xObject = GetObject(self);
                AFIPropertyManager xPropertyManager = xObject.GetPropertyManager();

                xPropertyManager.AddProperty(strPropertyName, xProperty.GetValue());
            }
        }

        void InitRecord(AFIDENTID self, string strClassName)
        {
            AFILogicClass xLogicClass = AFCLogicClassManager.Instance.GetElement(strClassName);
            if (null == xLogicClass)
            {
                return;
            }

            AFIDataList xDataList = xLogicClass.GetRecordManager().GetRecordList();
            for (int i = 0; i < xDataList.Count(); ++i)
            {
                string strRecordyName = xDataList.StringVal(i);
                AFIRecord xRecord = xLogicClass.GetRecordManager().GetRecord(strRecordyName);

                AFIObject xObject = GetObject(self);
                AFIRecordManager xRecordManager = xObject.GetRecordManager();

                xRecordManager.AddRecord(strRecordyName, xRecord.GetRows(), xRecord.GetColsData());
            }
        }

        Dictionary<AFIDENTID, AFIObject> mhtObject;
        Dictionary<string, ClassHandleDel> mhtClassHandleDel;
        AFIElementManager mxElementManager;
        AFILogicClassManager mxLogicClassManager;

		class ClassHandleDel
		{
			public ClassHandleDel()
			{
                mhtHandleDelList = new Dictionary<AFIObject.ClassEventHandler, string>();
			}
			
			public void AddDel(AFIObject.ClassEventHandler handler)
			{
				if (!mhtHandleDelList.ContainsKey(handler))
				{
					mhtHandleDelList.Add(handler, handler.ToString());
					mHandleDel += handler;
				}
			}
			
			public AFIObject.ClassEventHandler GetHandler()
			{
				return mHandleDel;
			}
			
			private AFIObject.ClassEventHandler mHandleDel;
            private Dictionary<AFIObject.ClassEventHandler, string> mhtHandleDelList;
		}
        
	}
}