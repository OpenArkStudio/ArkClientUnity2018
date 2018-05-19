using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace AFCoreEx
{
    public abstract class AFIRecordManager
    {
        public abstract AFIRecord AddRecord(string strRecordName,  int nRow, AFIDataList varData);
		public abstract AFIRecord GetRecord(string strRecordName);
		public abstract AFIDataList GetRecordList();
		
		public abstract void RegisterCallback(string strRecordName, AFIRecord.RecordEventHandler handler);
    }
}