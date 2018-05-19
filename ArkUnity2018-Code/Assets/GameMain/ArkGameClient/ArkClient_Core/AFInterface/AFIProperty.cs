using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFCoreEx
{
    public abstract class AFIProperty
    {
	public delegate void PropertyEventHandler(AFIDENTID self, string strProperty, AFIDataList oldVar, AFIDataList newVar);

    public abstract void SetValue(AFIDataList varData);

    public abstract AFIDataList GetValue();

    public abstract string GetKey();
		
	public abstract AFIDataList.VARIANT_TYPE GetDataType();

    public abstract Int64 QueryInt();

    public abstract float QueryFloat();

    public abstract double QueryDouble();

    public abstract string QueryString();

    public abstract AFIDENTID QueryObject();

    public abstract AFIDataList.Var_Data QueryDataObject();

	public abstract bool SetInt(Int64 value);

	public abstract bool SetFloat(float value);

	public abstract bool SetDouble(double value);

	public abstract bool SetString(string value);

    public abstract bool SetObject(AFIDENTID value);

    public abstract bool SetDataObject(ref AFIDataList.Var_Data data);

	public abstract void RegisterCallback(PropertyEventHandler handler);
    }
}