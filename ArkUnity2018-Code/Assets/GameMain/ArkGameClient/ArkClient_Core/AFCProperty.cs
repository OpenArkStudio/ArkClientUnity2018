using System;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace AFCoreEx
{
    public class AFCProperty : AFIProperty
    {
        public AFCProperty( AFIDENTID self, string strPropertyName, AFIDataList varData)
        {
            mSelf = self;
            msPropertyName = strPropertyName;
            mVarProperty = new AFCDataList(varData);
        }

        public override void SetValue(AFIDataList varData)
        {
            mVarProperty = varData;
        }

        public override AFIDataList GetValue()
        {
            return mVarProperty;
        }

        public override string GetKey()
        {
            return msPropertyName;
        }
		
		public override AFIDataList.VARIANT_TYPE GetDataType()
		{
			return mVarProperty.GetType(0);
		}

        public override Int64 QueryInt()
        {
            return mVarProperty.Int64Val(0);
        }

        public override float QueryFloat()
        {
            return mVarProperty.FloatVal(0);
        }

        public override double QueryDouble()
        {
            return mVarProperty.DoubleVal(0);
        }

        public override string QueryString()
        {
            return mVarProperty.StringVal(0);
        }

        public override AFIDENTID QueryObject()
        {
            return mVarProperty.ObjectVal(0);
        }

        public override AFIDataList.Var_Data QueryDataObject()
        {
            return mVarProperty.VarVal(0);
        }

        public override bool SetInt(Int64 value)
		{
			if (mVarProperty.Int64Val(0) != value)
			{
				AFCDataList oldValue = new AFCDataList(mVarProperty);
				
				mVarProperty.SetInt64(0, value);

				AFCDataList newValue = new AFCDataList(mVarProperty);

                if (null != doHandleDel)
                {
                    doHandleDel(mSelf, msPropertyName, oldValue, newValue);
                }
				
			}

			return true;
		}

		public override bool SetFloat(float value)
		{
			if (mVarProperty.FloatVal(0) - value > 0.01f
				|| mVarProperty.FloatVal(0) - value < -0.01f)
			{
				AFCDataList oldValue = new AFCDataList(mVarProperty);

				mVarProperty.SetFloat(0, value);

				AFCDataList newValue = new AFCDataList(mVarProperty);

                if (null != doHandleDel)
                {
                    doHandleDel(mSelf, msPropertyName, oldValue, newValue);
                }
			}

			return true;
		}

		public override bool SetDouble(double value)
		{
            if (mVarProperty.DoubleVal(0) - value > 0.01f
                || mVarProperty.DoubleVal(0) - value < -0.01f)
            {
                AFCDataList oldValue = new AFCDataList(mVarProperty);

                mVarProperty.SetDouble(0, value);

                AFCDataList newValue = new AFCDataList(mVarProperty);

                if (null != doHandleDel)
                {
                    doHandleDel(mSelf, msPropertyName, oldValue, newValue);
                }
            }

			return true;
		}

		public override bool SetString(string value)
		{
            if (mVarProperty.StringVal(0) != value)
            {
                AFCDataList oldValue = new AFCDataList(mVarProperty);

                mVarProperty.SetString(0, value);

                AFCDataList newValue = new AFCDataList(mVarProperty);

                if (null != doHandleDel)
                {
                    doHandleDel(mSelf, msPropertyName, oldValue, newValue);
                }
            }

			return true;
		}

		public override bool SetObject(AFIDENTID value)
		{

            if (mVarProperty.ObjectVal(0) != value)
            {
                AFCDataList oldValue = new AFCDataList(mVarProperty);

                mVarProperty.SetObject(0, value);

                AFCDataList newValue = new AFCDataList(mVarProperty);

                if (null != doHandleDel)
                {
                    doHandleDel(mSelf, msPropertyName, oldValue, newValue);
                }
            }

			return true;
		}

        public override bool SetDataObject(ref AFIDataList.Var_Data value)
        {
            if (mVarProperty.GetType(0) != value.nType)
            {
                AFCDataList oldValue = new AFCDataList(mVarProperty);

                mVarProperty.SetDataObject(0, value);

                AFCDataList newValue = new AFCDataList(mVarProperty);

                if (null != doHandleDel)
                {
                    doHandleDel(mSelf, msPropertyName, oldValue, newValue);
                }
            }

            return true;
        }

		public override void RegisterCallback(PropertyEventHandler handler)
		{
			doHandleDel += handler;
		}

		PropertyEventHandler doHandleDel;

		AFIDENTID mSelf;
		string msPropertyName;
		AFIDataList mVarProperty;
    }
}