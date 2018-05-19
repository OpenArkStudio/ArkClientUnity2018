using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFCoreEx
{
    public abstract class AFIKernel
    {
		public abstract bool AddHeartBeat(AFIDENTID self, string strHeartBeatName, AFIHeartBeat.HeartBeatEventHandler handler, float fTime, AFIDataList valueList);

        public abstract bool FindHeartBeat(AFIDENTID self, string strHeartBeatName);

        public abstract bool RemoveHeartBeat(AFIDENTID self, string strHeartBeatName);

        public abstract bool UpDate(float fTime);
		
		public abstract AFIDataList GetObjectList();
			
        /////////////////////////////////////////////////////////////
		public abstract void RegisterPropertyCallback(AFIDENTID self, string strPropertyName, AFIProperty.PropertyEventHandler handler);
		
        public abstract void RegisterRecordCallback(AFIDENTID self, string strRecordName, AFIRecord.RecordEventHandler handler);

        public abstract void RegisterClassCallBack(string strClassName, AFIObject.ClassEventHandler handler);

		public abstract void RegisterEventCallBack(AFIDENTID self, int nEventID, AFIEvent.EventHandler handler, AFIDataList valueList);
        /////////////////////////////////////////////////////////////////

        public abstract AFIObject GetObject(AFIDENTID ident);

        public abstract AFIObject CreateObject(AFIDENTID self, int nContainerID, int nGroupID, string strClassName, string strConfigIndex, AFIDataList arg);

        public abstract bool DestroyObject(AFIDENTID self);

        public abstract bool FindProperty(AFIDENTID self, string strPropertyName);

        public abstract bool SetPropertyInt(AFIDENTID self, string strPropertyName, Int64 nValue);
        public abstract bool SetPropertyFloat(AFIDENTID self, string strPropertyName, float fValue);
        public abstract bool SetPropertyDouble(AFIDENTID self, string strPropertyName, double dValue);
        public abstract bool SetPropertyString(AFIDENTID self, string strPropertyName, string strValue);
        public abstract bool SetPropertyObject(AFIDENTID self, string strPropertyName, AFIDENTID objectValue);

        public abstract Int64 QueryPropertyInt(AFIDENTID self, string strPropertyName);
        public abstract float QueryPropertyFloat(AFIDENTID self, string strPropertyName);
        public abstract double QueryPropertyDouble(AFIDENTID self, string strPropertyName);
        public abstract string QueryPropertyString(AFIDENTID self, string strPropertyName);
        public abstract AFIDENTID QueryPropertyObject(AFIDENTID self, string strPropertyName);

        public abstract AFIRecord FindRecord(AFIDENTID self, string strRecordName);

        public abstract bool SetRecordInt(AFIDENTID self, string strRecordName, int nRow, int nCol, Int64 nValue);
        public abstract bool SetRecordFloat(AFIDENTID self, string strRecordName, int nRow, int nCol, float fValue);
        public abstract bool SetRecordDouble(AFIDENTID self, string strRecordName, int nRow, int nCol, double dwValue);
        public abstract bool SetRecordString(AFIDENTID self, string strRecordName, int nRow, int nCol, string strValue);
        public abstract bool SetRecordObject(AFIDENTID self, string strRecordName, int nRow, int nCol, AFIDENTID objectValue);

        public abstract Int64 QueryRecordInt(AFIDENTID self, string strRecordName, int nRow, int nCol);
        public abstract float QueryRecordFloat(AFIDENTID self, string strRecordName, int nRow, int nCol);
        public abstract double QueryRecordDouble(AFIDENTID self, string strRecordName, int nRow, int nCol);
        public abstract string QueryRecordString(AFIDENTID self, string strRecordName, int nRow, int nCol);
        public abstract AFIDENTID QueryRecordObject(AFIDENTID self, string strRecordName, int nRow, int nCol);

        public abstract int FindRecordRow(AFIDENTID self, string strRecordName, int nCol, int nValue);
        public abstract int FindRecordRow(AFIDENTID self, string strRecordName, int nCol, float fValue);
        public abstract int FindRecordRow(AFIDENTID self, string strRecordName, int nCol, double fValue);
        public abstract int FindRecordRow(AFIDENTID self, string strRecordName, int nCol, string strValue);
        public abstract int FindRecordRow(AFIDENTID self, string strRecordName, int nCol, AFIDENTID nValue);


    }
}