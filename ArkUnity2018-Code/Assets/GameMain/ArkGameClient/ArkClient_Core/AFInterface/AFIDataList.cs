using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFCoreEx
{
    public abstract class AFIDataList
    {
        public enum VARIANT_TYPE
        {
            VTYPE_UNKNOWN    ,
            VTYPE_BOOLEAN    ,
            VTYPE_INT        ,
            VTYPE_INT64      ,
            VTYPE_FLOAT      ,
            VTYPE_DOUBLE     ,
            VTYPE_STRING     ,
            VTYPE_OBJECT     ,
            VTYPE_POINTER    ,
            VTYPE_USERDATA   ,
            VTYPE_TABLE      ,
            VTYPE_MAX
        };
        public class Var_Data
        {
            public Var_Data()
            {
                mData = new Object();
                nType = VARIANT_TYPE.VTYPE_UNKNOWN;
            }

            public VARIANT_TYPE nType;
            public Object mData;
        }

        public abstract bool AddInt64(Int64 value);
        public abstract bool AddFloat(float value);
        public abstract bool AddDouble(double value);
        public abstract bool AddString(string value);
        public abstract bool AddObject(AFIDENTID value);
        public abstract bool AddDataObject(ref Var_Data data);

        public abstract bool SetInt64(int index, Int64 value);
        public abstract bool SetFloat(int index, float value);
        public abstract bool SetDouble(int index, double value);
        public abstract bool SetString(int index, string value);
        public abstract bool SetObject(int index, AFIDENTID value);
        public abstract bool SetDataObject(int index, Var_Data value);

        public abstract Int64 Int64Val(int index);
        public abstract float FloatVal(int index);
        public abstract double DoubleVal(int index);
        public abstract string StringVal(int index);
        public abstract AFIDENTID ObjectVal(int index);
        public abstract Var_Data VarVal(int index);

		public abstract int Count();
		public abstract void Clear();
		public abstract VARIANT_TYPE GetType(int index);
    }
}

