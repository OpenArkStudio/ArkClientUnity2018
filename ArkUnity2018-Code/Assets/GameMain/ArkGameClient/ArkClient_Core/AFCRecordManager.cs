using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace AFCoreEx
{
	public class AFCRecordManager : AFIRecordManager
	{
		public AFCRecordManager(AFIDENTID ident)
		{
			mSelf = ident;
            mhtRecord = new Dictionary<string, AFIRecord>();
		}

		public override void RegisterCallback(string strRecordName, AFIRecord.RecordEventHandler handler)
		{
			if (mhtRecord.ContainsKey(strRecordName))
			{
				AFIRecord record = (AFIRecord)mhtRecord[strRecordName];
				record.RegisterCallback(handler);
			}
		}

		public override AFIRecord AddRecord(string strRecordName, int nRow, AFIDataList varData)
		{
			AFIRecord record = new AFCRecord(mSelf, strRecordName, nRow, varData);
			mhtRecord.Add(strRecordName, record);

			return record;
		}

		public override AFIRecord GetRecord(string strPropertyName)
		{
			AFIRecord record = null;

			if (mhtRecord.ContainsKey(strPropertyName))
			{
				record = (AFIRecord)mhtRecord[strPropertyName];
			}

			return record;
		}
		
		public override AFIDataList GetRecordList()
		{
			AFIDataList varData = new AFCDataList();
            foreach (KeyValuePair<string, AFIRecord> de in mhtRecord) 
			{
				varData.AddString(de.Key);				
			}
			
			return varData;
		}
		
		AFIDENTID mSelf;
        //Hashtable mhtRecord;
        Dictionary<string, AFIRecord> mhtRecord;
	}
}