using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AFCoreEx
{
	class AFCElement : AFIElement
	{
        public AFCElement()
        {
            mxPropertyManager = new AFCPropertyManager(new AFIDENTID());
        }

        public override AFIPropertyManager GetPropertyManager()
        {
            return mxPropertyManager;
        }


        public override Int64 QueryInt(string strName)
        {
            AFIProperty xProperty = GetPropertyManager().GetProperty(strName);
            if (null != xProperty)
            {
                return xProperty.QueryInt();
            }

            return 0;
        }

        public override float QueryFloat(string strName)
        {
            AFIProperty xProperty = GetPropertyManager().GetProperty(strName);
            if (null != xProperty)
            {
                return xProperty.QueryFloat();
            }

            return 0f;
        }

        public override double QueryDouble(string strName)
        {
            AFIProperty xProperty = GetPropertyManager().GetProperty(strName);
            if (null != xProperty)
            {
                return xProperty.QueryDouble();
            }

            return 0f;
        }

        public override string QueryString(string strName)
        {
            AFIProperty xProperty = GetPropertyManager().GetProperty(strName);
            if (null != xProperty)
            {
                return xProperty.QueryString();
            }

            return "";
        }

        public override AFIDENTID QueryObject(string strName)
        {
            AFIProperty xProperty = GetPropertyManager().GetProperty(strName);
            if (null != xProperty)
            {
                return xProperty.QueryObject();
            }

            return new AFIDENTID();
        }

        private AFIPropertyManager mxPropertyManager;
	}
}
