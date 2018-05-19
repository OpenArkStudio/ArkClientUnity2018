using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFCoreEx
{
    public abstract class AFIRecord
    {
		
        public enum eRecordOptype
        {
            Add = 0,
            Del,
            Swap,
            Updata,
            Create,
        };

		public delegate void RecordEventHandler(AFIDENTID self, string strRecordName, AFIRecord.eRecordOptype eType, int nRow, int nCol, AFIDataList oldVar, AFIDataList newVar);
		
		public abstract bool IsUsed(int nRow);
		public abstract int GetRows();
        public abstract int GetCols();
        public abstract AFIDataList.VARIANT_TYPE GetColType( int nCol);
        public abstract AFIDataList GetColsData();

        // add data
        public abstract int AddRow(int nRow);
        public abstract int AddRow(int nRow, AFIDataList var);

        // set data
        public abstract int SetValue(int nRow, AFIDataList var);

        public abstract bool SetInt(int nRow, int nCol, Int64 value);
        public abstract bool SetFloat(int nRow, int nCol, float value);
        public abstract bool SetDouble(int nRow, int nCol, double value);
        public abstract bool SetString(int nRow, int nCol, string value);
        public abstract bool SetObject(int nRow, int nCol, AFIDENTID value);
        public abstract bool SetDataObject(int nRow, int nCol, AFCoreEx.AFIDataList.Var_Data value);

        // query data
        public abstract AFIDataList QueryRow(int nRow);
        public abstract bool SwapRow(int nOriginRow, int nTargetRow);

        public abstract Int64 QueryInt(int nRow, int nCol);
        public abstract float QueryFloat(int nRow, int nCol);
        public abstract double QueryDouble(int nRow, int nCol);
        public abstract string QueryString(int nRow, int nCol);
        public abstract AFIDENTID QueryObject(int nRow, int nCol);
        public abstract AFCoreEx.AFIDataList.Var_Data QueryDataObject(int nRow, int nCol);

        //public abstract int FindRow( int nRow );
        public abstract int FindColValue(int nCol, AFIDataList var);

        public abstract int FindInt(int nCol, Int64 value);
        public abstract int FindFloat(int nCol, float value);
        public abstract int FindDouble(int nCol, double value);
        public abstract int FindString(int nCol, string value);
        public abstract int FindObject(int nCol, AFIDENTID value);

        public abstract bool Remove(int nRow);
        public abstract bool Clear();

		public abstract void RegisterCallback(RecordEventHandler handler);

        public abstract string GetName();
		public abstract void SetName(string strName);
    }
}