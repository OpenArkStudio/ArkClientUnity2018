using System.Collections;

namespace AFCoreEx
{
	public class AFCPropertyManager : AFIPropertyManager
	{
		public AFCPropertyManager(AFIDENTID self)
		{
			mSelf = self;
			mhtProperty = new Hashtable();
		}
		
		public override AFIProperty AddProperty(string strPropertyName, AFIDataList varData)
		{
			AFIProperty xProperty = null;
			if (!mhtProperty.ContainsKey(strPropertyName))
			{
				xProperty  = new AFCProperty(mSelf, strPropertyName, varData);
				mhtProperty[strPropertyName] = xProperty;
			}

			return xProperty;
		}

		public override bool SetProperty(string strPropertyName, AFIDataList varData)
		{
			if (mhtProperty.ContainsKey(strPropertyName))
			{
				AFIProperty xProperty = (AFCProperty)mhtProperty[strPropertyName];
				if (null != xProperty)
				{
					xProperty.SetValue(varData);
				} 
			}
			return true;
		}

		public override AFIProperty GetProperty(string strPropertyName)
		{
			AFIProperty xProperty = null;
			if (mhtProperty.ContainsKey(strPropertyName))
			{
				xProperty = (AFCProperty)mhtProperty[strPropertyName];
				return xProperty;
			}

			return xProperty;
		}

		public override void RegisterCallback(string strPropertyName, AFIProperty.PropertyEventHandler handler)
		{
			if (mhtProperty.ContainsKey(strPropertyName))
			{
				AFIProperty xProperty = (AFCProperty)mhtProperty[strPropertyName];
				xProperty.RegisterCallback(handler);
			}
		}
		
		public override AFIDataList GetPropertyList()
		{
			AFIDataList varData = new AFCDataList();
			foreach( DictionaryEntry de in mhtProperty) 
			{
				varData.AddString(de.Key.ToString());				
			}
			
			return varData;
		}
		
		AFIDENTID mSelf;
		Hashtable mhtProperty;
	}
}