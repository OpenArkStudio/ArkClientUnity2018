using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Collections;
using System.IO;

namespace AFCoreEx
{
    public class AFCElementManager : AFIElementManager
    {
        public AFCElementManager()
        {
            mhtObject = new Dictionary<string, AFIElement>();
        }

        #region Instance
        private static AFIElementManager _Instance = null;
        public static AFIElementManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new AFCElementManager();
                }
                return _Instance;
            }
        }
        #endregion
        public override bool Load(String strConfigPath)
        {
            mstrRootPath = strConfigPath;
            ClearInstanceElement();

            AFCLogicClassManager.Instance.LoadFromConfig(mstrRootPath);
            Dictionary<string, AFILogicClass> xTable = AFCLogicClassManager.Instance.GetElementList();
            foreach (KeyValuePair<string, AFILogicClass> kv in xTable)
            {
                LoadInstanceElement(kv.Value);
            }

            return false;
        }

        public override bool Clear()
        {
            return false;
        }

        public override bool ExistElement(string strConfigName)
        {
            if (mhtObject.ContainsKey(strConfigName))
            {
                return true;
            }

            return false;
        }

        public override Int64 QueryPropertyInt(string strConfigName, string strPropertyName)
        {
            AFIElement xElement = GetElement(strConfigName);
            if (null != xElement)
            {
                return xElement.QueryInt(strPropertyName);
            }

            return 0;
        }

        public override float QueryPropertyFloat(string strConfigName, string strPropertyName)
        {
            AFIElement xElement = GetElement(strConfigName);
            if (null != xElement)
            {
                 return xElement.QueryFloat(strPropertyName);
            }

            return 0;
        }

        public override double QueryPropertyDouble(string strConfigName, string strPropertyName)
        {
            AFIElement xElement = GetElement(strConfigName);
            if (null != xElement)
            {
                xElement.QueryDouble(strPropertyName);
            }

            return 0;
        }

        public override string QueryPropertyString(string strConfigName, string strPropertyName)
        {
            AFIElement xElement = GetElement(strConfigName);
            if (null != xElement)
            {
                return xElement.QueryString(strPropertyName);
            }

            return "";
        }

        public override bool AddElement(string strName, AFIElement xElement)
        {
            if (!mhtObject.ContainsKey(strName))
            {
                mhtObject.Add(strName, xElement);

                return true;
            }

            return false;
        }

        public override AFIElement GetElement(string strConfigName)
        {
            if (mhtObject.ContainsKey(strConfigName))
            {
                return (AFIElement)mhtObject[strConfigName];
            }

            return null;
        }

        private void ClearInstanceElement()
        {
            mhtObject.Clear();
        }

        private void LoadInstanceElement(AFILogicClass xLogicClass)
        {
            string strLogicPath = mstrRootPath;
            strLogicPath += xLogicClass.GetInstance();

            XmlDocument xmldoc = new XmlDocument();
            xmldoc.Load(strLogicPath);
            /////////////////////////////////////////////////////////////////

            XmlNode xRoot = xmldoc.SelectSingleNode("XML");

            XmlNodeList xNodeList = xRoot.SelectNodes("Entry");
            for (int i = 0; i < xNodeList.Count; ++i)
            {
                //AFCLog.Instance.Log("Class:" + xLogicClass.GetName());

                XmlNode xNodeClass = xNodeList.Item(i);
                XmlAttribute strID = xNodeClass.Attributes["Id"];

                //AFCLog.Instance.Log("ClassID:" + strID.Value);

                AFIElement xElement = GetElement(strID.Value);
                if (null == xElement)
                {
                    xElement = new AFCElement();
                    AddElement(strID.Value, xElement);
                    xLogicClass.AddConfigName(strID.Value);

                    XmlAttributeCollection xCollection = xNodeClass.Attributes;
                    for (int j = 0; j < xCollection.Count; ++j)
                    {
                        XmlAttribute xAttribute = xCollection[j];
                        AFIProperty xProperty = xLogicClass.GetPropertyManager().GetProperty(xAttribute.Name);
                        if (null != xProperty)
                        {
                            AFIDataList.VARIANT_TYPE eType = xProperty.GetDataType();
                            switch (eType)
                            {
                                case AFIDataList.VARIANT_TYPE.VTYPE_INT:
                                    {
                                        AFIDataList xValue = new AFCDataList();
                                        xValue.AddInt64(int.Parse(xAttribute.Value));
                                        xElement.GetPropertyManager().AddProperty(xAttribute.Name, xValue);
                                    }
                                    break;
                                case AFIDataList.VARIANT_TYPE.VTYPE_FLOAT:
                                    {
                                        AFIDataList xValue = new AFCDataList();
                                        xValue.AddFloat(float.Parse(xAttribute.Value));
                                        xElement.GetPropertyManager().AddProperty(xAttribute.Name, xValue);
                                    }
                                    break;
                                case AFIDataList.VARIANT_TYPE.VTYPE_DOUBLE:
                                    {
                                        AFIDataList xValue = new AFCDataList();
                                        xValue.AddDouble(double.Parse(xAttribute.Value));
                                        xElement.GetPropertyManager().AddProperty(xAttribute.Name, xValue);
                                    }
                                    break;
                                case AFIDataList.VARIANT_TYPE.VTYPE_STRING:
                                    {
                                        AFIDataList xValue = new AFCDataList();
                                        xValue.AddString(xAttribute.Value);
                                        AFIProperty xTestProperty = xElement.GetPropertyManager().AddProperty(xAttribute.Name, xValue);
                                    }
                                    break;
                                case AFIDataList.VARIANT_TYPE.VTYPE_OBJECT:
                                    {
                                        AFIDataList xValue = new AFCDataList();
                                        xValue.AddObject(new AFIDENTID(0, int.Parse(xAttribute.Value)));
                                        xElement.GetPropertyManager().AddProperty(xAttribute.Name, xValue);
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }
            }
        }

        /////////////////////////////////////////
        private Dictionary<string, AFIElement> mhtObject;
        private string mstrRootPath;
    }
}