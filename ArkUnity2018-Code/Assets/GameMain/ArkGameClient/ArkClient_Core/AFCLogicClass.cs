using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;

namespace AFCoreEx
{
    public class AFCLogicClass : AFILogicClass
	{
        public AFCLogicClass()
        {
            mxPropertyManager = new AFCPropertyManager(new AFIDENTID());
            mxRecordManager = new AFCRecordManager(new AFIDENTID());
        }

        public override AFIPropertyManager GetPropertyManager()
        {
            return mxPropertyManager;
        }

        public override AFIRecordManager GetRecordManager()
        {
            return mxRecordManager;
        }

        public override ArrayList GetConfigNameList()
        {
            return mxIntansList;
        }

        public override bool AddConfigName(string strConfigName)
        {
            mxIntansList.Add(strConfigName);

            return true;
        }

        public override string GetName()
        {
            return mstrName;
        }

        public override void SetName(string strConfigName)
        {
            mstrName = strConfigName;
        }

        public override string GetPath()
        {
            return mstrPath;
        }

        public override void SetPath(string sPath)
        {
            mstrPath = sPath;
        }

        public override string GetInstance()
        {
            return mstrInstance;
        }

        public override void SetInstance(string strInstancePath)
        {
            mstrInstance = strInstancePath;
        }



        /////////////////////////////////////
        private AFIPropertyManager mxPropertyManager;
        private AFIRecordManager mxRecordManager;
        private ArrayList mxIntansList = new ArrayList();
        private string mstrName;
        private string mstrPath;
        private string mstrInstance;
	}
}
