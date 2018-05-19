using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace AFCoreEx
{
    public class AFCDataList : AFIDataList
    {


        private Dictionary<int, Var_Data> mValueObject = new Dictionary<int, Var_Data>();

        //==============================================

        public AFCDataList(string str, char c)
        {
            string[] strSub = str.Split(c);
            foreach (string strDest in strSub)
            {
                AddString(strDest);
            }
        }

        public AFCDataList(AFIDataList src)
        {
			for (int i = 0; i < src.Count(); i++ )
			{
				switch (src.GetType(i))
				{
					case VARIANT_TYPE.VTYPE_INT:
						AddInt64(src.Int64Val(i));
					break;
					case VARIANT_TYPE.VTYPE_FLOAT:
						AddFloat(src.FloatVal(i));
					break;
					case VARIANT_TYPE.VTYPE_DOUBLE:
						AddDouble(src.DoubleVal(i));
					break;
					case VARIANT_TYPE.VTYPE_STRING:
						AddString(src.StringVal(i));
					break;
					case VARIANT_TYPE.VTYPE_OBJECT:
						AddObject(src.ObjectVal(i));
					break;
						default:
					break;
				}
			}

        }

        public AFCDataList()
        {
        }

        public override bool AddInt64(Int64 value)
        {
            Var_Data data = new Var_Data();
            data.nType = VARIANT_TYPE.VTYPE_INT;
            data.mData = value;

			return AddDataObject(ref data);
        }

        public override bool AddFloat(float value)
        {
            Var_Data data = new Var_Data();
            data.nType = VARIANT_TYPE.VTYPE_FLOAT;
            data.mData = value;

			return AddDataObject(ref data);
        }

        public override bool AddDouble(double value)
        {
            Var_Data data = new Var_Data();
            data.nType = VARIANT_TYPE.VTYPE_DOUBLE;
            data.mData = value;

            return AddDataObject(ref data);
        }

        public override bool AddString(string value)
        {
            Var_Data data = new Var_Data();
            data.nType = VARIANT_TYPE.VTYPE_STRING;
            data.mData = value;

            return AddDataObject(ref data);
        }

        public override bool AddObject(AFIDENTID value)
        {
            Var_Data data = new Var_Data();
            data.nType = VARIANT_TYPE.VTYPE_OBJECT;
            data.mData = value;

			return AddDataObject(ref data);
        }

        public override bool SetInt64(int index, Int64 value)
        {
            Var_Data data = GetDataObject(index);
            if (data != null && data.nType == VARIANT_TYPE.VTYPE_INT)
            {
                data.mData = value;

                return true;
            }

            return false;
        }

        public override bool SetFloat(int index, float value)
        {
            Var_Data data = GetDataObject(index);
            if (data != null && data.nType == VARIANT_TYPE.VTYPE_FLOAT)
            {
                data.mData = value;

                return true;
            }

            return false;
        }

        public override bool SetDouble(int index, double value)
        {
            Var_Data data = GetDataObject(index);
            if (data != null && data.nType == VARIANT_TYPE.VTYPE_DOUBLE)
            {
                data.mData = value;

                return true;
            }

            return false;
        }

        public override bool SetString(int index, string value)
        {
            Var_Data data = GetDataObject(index);
            if (data != null && data.nType == VARIANT_TYPE.VTYPE_STRING)
            {
                data.mData = value;

                return true;
            }

            return false;
        }

        public override bool SetObject(int index, AFIDENTID value)
        {
            Var_Data data = GetDataObject(index);
            if (data != null && data.nType == VARIANT_TYPE.VTYPE_OBJECT)
            {
                data.mData = value;

                return true;
            }

            return false;
        }

        public override bool SetDataObject(int index, Var_Data value)
        {
            mValueObject[index] = value;
            return true;
        }

        public override Int64 Int64Val(int index)
        {
            Var_Data data = GetDataObject(index);
            if (data != null && data.nType == VARIANT_TYPE.VTYPE_INT)
            {
                return (Int64)data.mData;
            }

            return 0;
        }

        public override float FloatVal(int index)
        {
            Var_Data data = GetDataObject(index);
            if (data != null && data.nType == VARIANT_TYPE.VTYPE_FLOAT)
            {
                return (float)data.mData;
            }

            return 0.0f;
        }

        public override double DoubleVal(int index)
        {
            Var_Data data = GetDataObject(index);
            if (data != null && data.nType == VARIANT_TYPE.VTYPE_DOUBLE)
            {
                return (double)data.mData;
            }

            return 0.0;
        }

        public override string StringVal(int index)
        {
            Var_Data data = GetDataObject(index);
            if (data != null && data.nType == VARIANT_TYPE.VTYPE_STRING)
            {
                return (string)data.mData;
            }

            return "";
        }

        public override AFIDENTID ObjectVal(int index)
        {
            Var_Data data = GetDataObject(index);
            if (data != null && data.nType == VARIANT_TYPE.VTYPE_OBJECT)
            {
                return (AFIDENTID)data.mData;
            }

            return new AFIDENTID();
        }

        public override Var_Data VarVal(int index)
        {
            return GetDataObject(index);
        }


		public override int Count()
		{
			return mValueObject.Count;
		}

		public override void Clear()
		{
			mValueObject.Clear();
		}

        public override bool AddDataObject(ref Var_Data data)
        {
            int nCount = mValueObject.Count;
            mValueObject.Add(nCount, data);

            return true;
        }

		public override VARIANT_TYPE GetType(int index)
        {
			if (mValueObject.Count > index)
			{
				Var_Data data = (Var_Data)mValueObject[index];

				return data.nType;
			}

			return VARIANT_TYPE.VTYPE_UNKNOWN;
        }

        protected Var_Data GetDataObject(int index)
        {

            if (mValueObject.ContainsKey(index))
            {
                return (Var_Data)mValueObject[index];
            }

            return null;
        }


    }
}
