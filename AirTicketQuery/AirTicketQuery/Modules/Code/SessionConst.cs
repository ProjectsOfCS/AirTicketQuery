using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;

namespace AirTicketQuery.Modules.Code
{
    public class SC
    {
        protected const string SessionPrefix = "AirTicketQuery_";
        public static string SystemName
        {
            get
            {
                string strKey = SessionPrefix + "SystemName";
                if (!SessionHelper.CheckValueOfSession(strKey))
                {
                    Assembly currAssembley = Assembly.GetExecutingAssembly();
                    string strName = ((AssemblyProductAttribute)Attribute.GetCustomAttribute(currAssembley, typeof(AssemblyProductAttribute))).Product;

                    SessionHelper.SetValutToSession<string>(strKey, strName);
                }

                return SessionHelper.GetValueFromSession<string>(strKey);
            }
        }
    }
}
