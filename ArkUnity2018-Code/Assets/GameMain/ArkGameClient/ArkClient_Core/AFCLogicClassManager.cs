using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace AFCoreEx
{
    public class AFCLogicClassManager : AFILogicClassManager
    {
        private String mstrRootPath = null;
        #region Instance
        private static AFCLogicClassManager _Instance = null;
        public static AFCLogicClassManager Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new AFCLogicClassManager();
                }

                return _Instance;
            }
        }
        #endregion

        public void LoadFromConfig(String strConfigPath)
        {
            mstrRootPath = strConfigPath;
            _Instance.Load();
        }

        private bool Load()
        {
            ClearLogicClass();

            XmlDocument xmldoc = new XmlDocument();

            string strLogicPath = mstrRootPath + "DataConfig/Struct/LogicClass.xml";
            xmldoc.Load(strLogicPath);
            /////////////////////////////////////////////////////////////////
            XmlNode root = xmldoc.SelectSingleNode("XML");

            LoadLogicClass(root);
            LoadLogicClassDataNodes();
            LoadLogicClassDataTables();

            return false;
        }

        public override bool Clear()
        {
            return false;
        }

        public override bool ExistElement(string strClassName)
        {
            if (mhtObject.ContainsKey(strClassName))
            {
                return true;
            }

            return false;
        }

        public override bool AddElement(string strName)
        {
            if (!mhtObject.ContainsKey(strName))
            {
                AFILogicClass xElement = new AFCLogicClass();
                xElement.SetName(strName);
                mhtObject.Add(strName, xElement);

                return true;
            }

            return false;
        }

        public override AFILogicClass GetElement(string strClassName)
        {
            if (mhtObject.ContainsKey(strClassName))
            {
                return (AFILogicClass)mhtObject[strClassName];
            }

            return null;
        }
        /////////////////////////////////////////
        private void LoadLogicClass(XmlNode xNode)
        {
            XmlNodeList xNodeList = xNode.SelectNodes("Class");
            for (int i = 0; i < xNodeList.Count; ++i)
            {
                XmlNode xNodeClass = xNodeList.Item(i);
                XmlAttribute strID = xNodeClass.Attributes["Id"];
                XmlAttribute strPath = xNodeClass.Attributes["Path"];
                XmlAttribute strInstancePath = xNodeClass.Attributes["InstancePath"];

                AFILogicClass xLogicClass = new AFCLogicClass();
                mhtObject.Add(strID.Value, xLogicClass);

                xLogicClass.SetName(strID.Value);
                xLogicClass.SetPath(strPath.Value);
                xLogicClass.SetInstance(strInstancePath.Value);

                XmlNodeList xNodeSubClassList = xNodeClass.SelectNodes("Class");
                if (xNodeSubClassList.Count > 0)
                {
                    LoadLogicClass(xNodeClass);
                }
            }
        }
        
        private void ClearLogicClass()
        {
            mhtObject.Clear();
        }

        private void LoadLogicClassDataNodes()
        {
            Dictionary<string, AFILogicClass> xTable = AFCLogicClassManager.Instance.GetElementList();
            foreach (KeyValuePair<string, AFILogicClass> kv in xTable)
            {
                LoadLogicClassDataNodes((string)kv.Key);
            }

            //再为每个类加载iobject的属性
            foreach (KeyValuePair<string, AFILogicClass> kv in xTable)
            {
                if (kv.Key != "IObject")
                {
                    AddBasePropertyFormOther(kv.Key, "IObject");
                }
            }
        }

        private void LoadLogicClassDataTables()
        {
            Dictionary<string, AFILogicClass> xTable = AFCLogicClassManager.Instance.GetElementList();
            foreach (KeyValuePair<string, AFILogicClass> kv in xTable)
            {
                LoadLogicClassDataTables(kv.Key);
            }
        }

        private void LoadLogicClassDataNodes(string strName)
        {
            AFILogicClass xLogicClass = GetElement(strName);
            if (null != xLogicClass)
            {
                string strLogicPath = mstrRootPath + xLogicClass.GetPath();
                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(strLogicPath);
                /////////////////////////////////////////////////////////////////

                XmlNode xRoot = xmldoc.SelectSingleNode("XML");
                XmlNode xNodePropertys = xRoot.SelectSingleNode("DataNodes");
                XmlNodeList xNodeList = xNodePropertys.SelectNodes("DataNode");
                for (int i = 0; i < xNodeList.Count; ++i)
                {
                    XmlNode xPropertyNode = xNodeList.Item(i);
                    XmlAttribute strID = xPropertyNode.Attributes["Id"];
                    XmlAttribute strType = xPropertyNode.Attributes["Type"];

                    switch (strType.Value)
                    {
                        case "int":
                            {
                                AFIDataList xValue = new AFCDataList();
                                xValue.AddInt64(0);
                                xLogicClass.GetPropertyManager().AddProperty(strID.Value, xValue);
                            }
                            break;
                        case "float":
                            {
                                AFIDataList xValue = new AFCDataList();
                                xValue.AddFloat(0.0f);
                                xLogicClass.GetPropertyManager().AddProperty(strID.Value, xValue);
                            }
                            break;
                        case "double":
                            {
                                AFIDataList xValue = new AFCDataList();
                                xValue.AddDouble(0.0f);
                                xLogicClass.GetPropertyManager().AddProperty(strID.Value, xValue);
                            }
                            break;
                        case "string":
                            {
                                AFIDataList xValue = new AFCDataList();
                                xValue.AddString("");
                                xLogicClass.GetPropertyManager().AddProperty(strID.Value, xValue);
                            }
                            break;
                        case "object":
                            {
                                AFIDataList xValue = new AFCDataList();
                                xValue.AddObject(new AFIDENTID(0, 0));
                                xLogicClass.GetPropertyManager().AddProperty(strID.Value, xValue);
                            }
                            break;
                        default:
                            break;

                    }
                }
            }
        }

        private void LoadLogicClassDataTables(string strName)
        {
            AFILogicClass xLogicClass = GetElement(strName);
            if (null != xLogicClass)
            {
                string strLogicPath = mstrRootPath + xLogicClass.GetPath();

                XmlDocument xmldoc = new XmlDocument();
                xmldoc.Load(strLogicPath);
                /////////////////////////////////////////////////////////////////
                XmlNode xRoot = xmldoc.SelectSingleNode("XML");
                XmlNode xNodePropertys = xRoot.SelectSingleNode("DataTables");
                if (null != xNodePropertys)
                {
                    XmlNodeList xNodeList = xNodePropertys.SelectNodes("DataTable");
                    if (null != xNodeList)
                    {
                        for (int i = 0; i < xNodeList.Count; ++i)
                        {
                            XmlNode xRecordNode = xNodeList.Item(i);

                            string strID = xRecordNode.Attributes["Id"].Value;
                            string strRow = xRecordNode.Attributes["Row"].Value;
                            AFIDataList xValue = new AFCDataList();

                            XmlNodeList xTagNodeList = xRecordNode.SelectNodes("Col");
                            for (int j = 0; j < xTagNodeList.Count; ++j)
                            {
                                XmlNode xColTagNode = xTagNodeList.Item(j);

                                XmlAttribute strTagID = xColTagNode.Attributes["Tag"];
                                XmlAttribute strTagType = xColTagNode.Attributes["Type"];


                                switch (strTagType.Value)
                                {
                                    case "int":
                                        {
                                            xValue.AddInt64(0);
                                        }
                                        break;
                                    case "float":
                                        {
                                            xValue.AddFloat(0.0f);
                                        }
                                        break;
                                    case "double":
                                        {
                                            xValue.AddDouble(0.0f);
                                        }
                                        break;
                                    case "string":
                                        {
                                            xValue.AddString("");
                                        }
                                        break;
                                    case "object":
                                        {
                                            xValue.AddObject(new AFIDENTID(0, 0));
                                        }
                                        break;
                                    default:
                                        break;

                                }
                            }

                            xLogicClass.GetRecordManager().AddRecord(strID, int.Parse(strRow), xValue);
                        }
                    }
                }
            }
        }

        void AddBasePropertyFormOther(string strName, string strOther)
        {
            AFILogicClass xOtherClass = GetElement(strOther);
            AFILogicClass xLogicClass = GetElement(strName);
            if (null != xLogicClass && null != xOtherClass)
            {
                AFIDataList xValue = xOtherClass.GetPropertyManager().GetPropertyList();
                for (int i = 0; i < xValue.Count(); ++i)
                {
                    AFIProperty xProperty = xOtherClass.GetPropertyManager().GetProperty(xValue.StringVal(i));
                    xLogicClass.GetPropertyManager().AddProperty(xValue.StringVal(i), xProperty.GetValue());
                }
            }
        }

        public override Dictionary<string, AFILogicClass> GetElementList()
        {
            return mhtObject;
        }
        /////////////////////////////////////////
        private Dictionary<string, AFILogicClass> mhtObject = new Dictionary<string, AFILogicClass>();
    }
}