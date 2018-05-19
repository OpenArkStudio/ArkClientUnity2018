using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFCoreEx
{
    public class AFCObject : AFIObject
    {
		public AFCObject(AFIDENTID self, int nContainerID, int nGroupID, string strClassName, string strConfigIndex)
		{
			mSelf = self;
            mstrClassName = strClassName;
            mstrConfigIndex = strConfigIndex;
			mnContainerID = nContainerID;
			mnGroupID = nGroupID;
			Init();
		}

		~AFCObject()
		{
			Shut();
		}

        public override bool Init()
        {
			mRecordManager = new AFCRecordManager(mSelf);
			mPropertyManager = new AFCPropertyManager(mSelf);
			mHeartManager = new AFCHeartBeatManager(mSelf);
			mEventManager = new AFCEventManager(mSelf);

            return true;
        }

        public override bool Shut()
        {
            AFIDataList xRecordList = mRecordManager.GetRecordList();
            if (null != xRecordList)
            {
                for(int i = 0; i < xRecordList.Count(); ++i)
                {
                    string strRecordName = xRecordList.StringVal(i);
                    AFIRecord xRecord = mRecordManager.GetRecord(strRecordName);
                    if (null !=  xRecord)
                    {
                        xRecord.Clear();
                    }
                }
            }

			mRecordManager = null;
			mPropertyManager = null;
            mHeartManager = null;
            mEventManager = null;

            return true;
        }

        public override bool UpData(float fLastTime, float fAllTime)
        {
			//mHeartManager
            return true;
        }

        ///////////////////////////////////////////////////////////////////////
        public override AFIDENTID Self()
        {
			return mSelf;
        }
		
		public override int ContainerID()
        {
			return mnContainerID;
        }
		
		public override int GroupID()
        {
			return mnGroupID;
        }
		
        public override string ClassName()
        {
            return mstrClassName;
        }

        public override string ConfigIndex()
        {
            return mstrConfigIndex;
        }

        // public override bool AddHeartBeat(  string strHeartBeatName, HEART_BEAT_FUNC cb,  AFIDataList& var,  float fTime,  int nCount );

        public override bool FindHeartBeat(string strHeartBeatName)
        {
            return true;
        }

        public override bool RemoveHeartBeat(string strHeartBeatName)
        {
            return true;
        }

        //////////////////////////////////////////////////
        // 
        //     public override bool AddRecordCallBack(  string strRecordName,  RECORD_EVENT_FUNC cb );
        // 
        //     public override bool AddPropertyCallBack(  string strCriticalName,  PROPERTY_EVENT_FUNC cb );

        /////////////////////////////////////////////////////////////////

        public override bool FindProperty(string strPropertyName)
        {
			if (null != mPropertyManager.GetProperty(strPropertyName))
			{
				return true;
			}

            return false;
        }

        public override bool SetPropertyInt(string strPropertyName, Int64 nValue)
        {
			AFIProperty property = mPropertyManager.GetProperty(strPropertyName);
			if (null == property)
			{
                AFIDataList valueList = new AFCDataList();
                valueList.AddInt64(0);
                property = mPropertyManager.AddProperty(strPropertyName, valueList);
            }

			property.SetInt(nValue);
			return true;
        }

        public override bool SetPropertyFloat(string strPropertyName, float fValue)
        {
			AFIProperty property = mPropertyManager.GetProperty(strPropertyName);
			if (null == property)
			{
                AFIDataList valueList = new AFCDataList();
                valueList.AddFloat(0.0f);
                property = mPropertyManager.AddProperty(strPropertyName, valueList);
            }
				
            property.SetFloat(fValue);
			return true;
        }

        public override bool SetPropertyDouble(string strPropertyName, double dwValue)
        {
			AFIProperty property = mPropertyManager.GetProperty(strPropertyName);
			if (null == property)
			{
                AFIDataList valueList = new AFCDataList();
                valueList.AddDouble(0);
                property = mPropertyManager.AddProperty(strPropertyName, valueList);
            }
				
            property.SetDouble(dwValue);
            return true;
        }

        public override bool SetPropertyString(string strPropertyName, string strValue)
        {
			AFIProperty property = mPropertyManager.GetProperty(strPropertyName);
			if (null == property)
			{
                AFIDataList valueList = new AFCDataList();
                valueList.AddString("");
                property = mPropertyManager.AddProperty(strPropertyName, valueList);
            }

			property.SetString(strValue);
			return true;
        }

        public override bool SetPropertyObject(string strPropertyName, AFIDENTID obj)
        {
			AFIProperty property = mPropertyManager.GetProperty(strPropertyName);
			if (null == property)
			{
                AFIDataList valueList = new AFCDataList();
                valueList.AddObject(new AFIDENTID());
                property = mPropertyManager.AddProperty(strPropertyName, valueList);
            }

			property.SetObject(obj);
			return true;

        }

        public override Int64 QueryPropertyInt(string strPropertyName)
        {
			AFIProperty property = mPropertyManager.GetProperty(strPropertyName);
			if (null != property)
			{
				return property.QueryInt();
			}

            return 0;
        }

        public override float QueryPropertyFloat(string strPropertyName)
        {
			AFIProperty property = mPropertyManager.GetProperty(strPropertyName);
			if (null != property)
			{
				return property.QueryFloat();
			}

            return 0.0f;
        }

        public override double QueryPropertyDouble(string strPropertyName)
        {
			AFIProperty property = mPropertyManager.GetProperty(strPropertyName);
			if (null != property)
			{
				return property.QueryDouble();
			}

            return 0.0;
        }

        public override string QueryPropertyString(string strPropertyName)
        {
			AFIProperty property = mPropertyManager.GetProperty(strPropertyName);
			if (null != property)
			{
				return property.QueryString();
			}

            return "";
        }

        public override AFIDENTID QueryPropertyObject(string strPropertyName)
        {
			AFIProperty property = mPropertyManager.GetProperty(strPropertyName);
			if (null != property)
			{
				return property.QueryObject();
			}

            return new AFIDENTID();
        }

        public override bool FindRecord(string strRecordName)
        {
			AFIRecord record = mRecordManager.GetRecord(strRecordName);
			if (null != record)
			{
				return true;
			}
            return false;
        }

        public override bool SetRecordInt(string strRecordName, int nRow, int nCol, Int64 nValue)
        {
			AFIRecord record = mRecordManager.GetRecord(strRecordName);
			if (null != record)
			{
				record.SetInt(nRow, nCol, nValue);
				return true;
			}

			return false;
        }

        public override bool SetRecordFloat(string strRecordName, int nRow, int nCol, float fValue)
        {
			AFIRecord record = mRecordManager.GetRecord(strRecordName);
			if (null != record)
			{
				record.SetFloat(nRow, nCol, fValue);
				return true;
			}

			return false;
        }

        public override bool SetRecordDouble(string strRecordName, int nRow, int nCol, double dwValue)
        {
			AFIRecord record = mRecordManager.GetRecord(strRecordName);
			if (null != record)
			{
				record.SetDouble(nRow, nCol, dwValue);
				return true;
			}

			return false;
        }

        public override bool SetRecordString(string strRecordName, int nRow, int nCol, string strValue)
        {
			AFIRecord record = mRecordManager.GetRecord(strRecordName);
			if (null != record)
			{
				record.SetString(nRow, nCol, strValue);
				return true;
			}

			return false;
        }

        public override bool SetRecordObject(string strRecordName, int nRow, int nCol, AFIDENTID obj)
        {
			AFIRecord record = mRecordManager.GetRecord(strRecordName);
			if (null != record)
			{
				record.SetObject(nRow, nCol, obj);
				return true;
			}

			return false;
        }

        public override Int64 QueryRecordInt(string strRecordName, int nRow, int nCol)
        {
			AFIRecord record = mRecordManager.GetRecord(strRecordName);
			if (null != record)
			{
				return record.QueryInt(nRow, nCol);
			}

            return 0;
        }

        public override float QueryRecordFloat(string strRecordName, int nRow, int nCol)
        {
			AFIRecord record = mRecordManager.GetRecord(strRecordName);
			if (null != record)
			{
				return record.QueryFloat(nRow, nCol);
			}

            return 0.0f;
        }

        public override double QueryRecordDouble(string strRecordName, int nRow, int nCol)
        {
			AFIRecord record = mRecordManager.GetRecord(strRecordName);
			if (null != record)
			{
				return record.QueryDouble(nRow, nCol);
			}

            return 0.0;
        }

        public override string QueryRecordString(string strRecordName, int nRow, int nCol)
        {
			AFIRecord record = mRecordManager.GetRecord(strRecordName);
			if (null != record)
			{
				return record.QueryString(nRow, nCol);
			}

            return "";
        }

        public override AFIDENTID QueryRecordObject(string strRecordName, int nRow, int nCol)
        {
			AFIRecord record = mRecordManager.GetRecord(strRecordName);
			if (null != record)
			{
				return record.QueryObject(nRow, nCol);
			}

            return null;
        }

        public override AFIRecordManager GetRecordManager()
        {
			return mRecordManager;
        }

        public override AFIHeartBeatManager GetHeartBeatManager()
        {
			return mHeartManager;
        }

        public override AFIPropertyManager GetPropertyManager()
        {
			return mPropertyManager;
        }

		public override AFIEventManager GetEventManager()
		{
			return mEventManager;
		}

		AFIDENTID mSelf;
		int mnContainerID;
		int mnGroupID;
		
		string mstrClassName;
		string mstrConfigIndex;

		AFIRecordManager mRecordManager;
		AFIPropertyManager mPropertyManager;
		AFIHeartBeatManager mHeartManager;
		AFIEventManager mEventManager;
	}
}