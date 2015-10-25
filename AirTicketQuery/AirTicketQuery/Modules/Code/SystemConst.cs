using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;

namespace AirTicketQuery.Modules.Code
{
    public class SystemConst
    {
        private static DBConnectParams dbConnParam;
        private static string strSysKey = string.Empty;

        /// <summary>
        /// Property ConnectionString is used to get database connection string.
        /// </summary>
        public static string DBConnString
        {
            get
            {
                if (dbConnParam == null || dbConnParam.IsEmpty)
                {
                    AssignDBConnByFile();
                }

                return dbConnParam.ToString();
            }
            set
            {
                dbConnParam = new DBConnectParams(value);
            }
        }

        public static DBConnectParams DBConnParam
        {
            get
            {
                if (dbConnParam == null || dbConnParam.IsEmpty)
                    AssignDBConnByFile();

                return dbConnParam;
            }
            set
            {
                dbConnParam = value;
            }
        }

        public static string SysKey
        {
            get
            {
                return strSysKey;
            }
        }

        public static void AssignDBConnByFile()
        {
            string strNormalConn = string.Empty;
            strSysKey = ConfigurationManager.AppSettings["SysKey"];

            ////CHECK Web.config OR App.config
            ConnectionStringSettings connStrNormal = ConfigurationManager.ConnectionStrings["ConnectionString"];
            if (connStrNormal != null)
                strNormalConn = connStrNormal.ConnectionString;

            if (string.IsNullOrEmpty(strNormalConn))
                throw new Exception("Fail to get ConnectionString,Please check config file! ");

            dbConnParam = new DBConnectParams(strNormalConn);
        }
    }
}
