using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFCoreEx
{
    public abstract class AFIPropertyManager
    {
        public abstract AFIProperty AddProperty(string strPropertyName, AFIDataList varData);

        public abstract bool SetProperty(string strPropertyName, AFIDataList varData);

        public abstract AFIProperty GetProperty(string strPropertyName);
		
		public abstract AFIDataList GetPropertyList();
		
		public abstract void RegisterCallback(string strPropertyName, AFIProperty.PropertyEventHandler handler);
    }
}