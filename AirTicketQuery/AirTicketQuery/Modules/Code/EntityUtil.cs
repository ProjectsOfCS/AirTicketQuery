using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Web;

namespace AirTicketQuery.Modules.Code
{
    public class EntityUtil
    {
        /// <summary>
        /// Create a new Entity from http request params
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="drInit">DataRow from DataTable</param>
        /// <returns></returns>
        public static T Create<T>(HttpRequest req)
        {
            T pINs = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] piIns = pINs.GetType().GetProperties();
            for (int i = 0; i < piIns.Length; i++)
            {
                string piName = piIns[i].Name;
                if (piIns[i].CanWrite && req.Form.AllKeys.Contains(piName) && req.Form[piName] != null)
                {
                    Type propType = piIns[i].PropertyType;
                    if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propType = propType.GetGenericArguments()[0];
                    }

                    string val = req.Form[piName];
                    if (!string.IsNullOrEmpty(val))
                    {
                        if (propType == typeof(decimal))
                            piIns[i].SetValue(pINs, Convert.ToDecimal(val), null);
                        else if (propType == typeof(int))
                            piIns[i].SetValue(pINs, Convert.ToInt32(val), null);
                        else if (propType == typeof(DateTime))
                        {
                            DateTime dt;
                            if (DateTime.TryParse(val, out dt))
                                piIns[i].SetValue(pINs, dt, null);
                        }
                        else
                            piIns[i].SetValue(pINs, val.ToString().Trim(), null);
                    }
                }
            }

            return pINs;
        }
        
        /// <summary>
        /// Create a new Entity from datarow
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="drInit">DataRow from DataTable</param>
        /// <returns></returns>
        public static T Create<T>(DataRow drInit)
        {
            T pINs = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] piIns = pINs.GetType().GetProperties();
            for (int i = 0; i < piIns.Length; i++)
            {
                string piName = piIns[i].Name;
                if (piIns[i].CanWrite && drInit.Table.Columns.Contains(piName) && !drInit.IsNull(piName))
                {
                    Type propType = piIns[i].PropertyType;
                    if (propType.IsGenericType && propType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        propType = propType.GetGenericArguments()[0];
                    }

                    if (propType == typeof(string))
                        piIns[i].SetValue(pINs, drInit[piName].ToString().Trim(), null);
                    else if (propType == typeof(decimal))
                        piIns[i].SetValue(pINs, Convert.ToDecimal(drInit[piName]), null);
                    else
                        piIns[i].SetValue(pINs, drInit[piName], null);
                }
            }

            return pINs;
        }

        /// <summary>
        /// Create a List of Entity from DataTable
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="dtInit">DataTable</param>
        /// <returns></returns>
        public static List<T> Create<T>(DataTable dtInit)
        {
            List<T> result = new List<T>();

            foreach (DataRow drInit in dtInit.Rows)
            {
                result.Add(Create<T>(drInit));
            }

            return result;
        }

        /// <summary>
        /// Create DataTable from List of Entity
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="insT">ins Of Entity</param>
        /// <returns></returns>
        public static DataTable Create<T>(T insT)
        {
            List<T> listT = new List<T>();
            listT.Add(insT);
            return Create<T>(listT, true);
        }

        /// <summary>
        /// Create DataTable from List of Entity
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="listT">List Of Entity</param>
        /// <returns></returns>
        public static DataTable Create<T>(List<T> listT)
        {
            return Create<T>(listT, true);
        }

        /// <summary>
        /// Create DataTable from List of Entity
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="insT">ins Of Entity</param>
        /// <param name="filterColumnName">if true, only handle column name start with "C_"</param>
        /// <returns></returns>
        public static DataTable Create<T>(T insT, bool filterColumnName)
        {
            List<T> listT = new List<T>();
            listT.Add(insT);
            return Create<T>(listT, filterColumnName);
        }

        /// <summary>
        /// Create DataTable from List of Entity
        /// </summary>
        /// <typeparam name="T">Entity</typeparam>
        /// <param name="listT">List Of Entity</param>
        /// <param name="filterColumnName">if true, only handle column name start with "C_"</param>
        /// <returns></returns>
        public static DataTable Create<T>(List<T> listT, bool filterColumnName)
        {
            DataTable dtResult = new DataTable();
            T pINs = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] piIns = pINs.GetType().GetProperties();
            foreach (var item in piIns)
            {
                Type tmpT = item.PropertyType;
                if (tmpT.IsGenericType && tmpT.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    tmpT = tmpT.GetGenericArguments()[0];
                }

                if (item.Name.StartsWith("C_") || !filterColumnName)
                    dtResult.Columns.Add(new DataColumn(item.Name, tmpT));
            }

            foreach (var insSrc in listT)
            {
                DataRow drNew = dtResult.NewRow();
                foreach (var item in piIns)
                {
                    if (item.Name.StartsWith("C_") || !filterColumnName)
                        drNew[item.Name] = CheckNull(item.GetValue(insSrc, null));
                }

                dtResult.Rows.Add(drNew);
            }

            return dtResult;
        }

        public static T Clone<T>(T insSrc)
        {
            T insDest = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] piSrc = insSrc.GetType().GetProperties();
            PropertyInfo[] piDesc = insDest.GetType().GetProperties();

            for (int i = 0; i < piSrc.Length; i++)
            {
                string srcName = piSrc[i].Name;
                if (!srcName.StartsWith("C_"))
                    continue;
                if (piDesc[i].CanWrite)
                    piDesc[i].SetValue(insDest, piSrc[i].GetValue(insSrc, null), null);
            }

            return insDest;
        }

        public static List<SqlParameter> GenerateParams<T>(T insSrc)
        {
            List<SqlParameter> lstParam = new List<SqlParameter>();
            PropertyInfo[] piList = insSrc.GetType().GetProperties();
            for (int i = 0; i < piList.Length; i++)
            {
                string srcName = piList[i].Name;
                if (!srcName.StartsWith("C_"))
                    continue;
                if (piList[i].CanWrite)
                    lstParam.Add(new SqlParameter("@" + srcName, CheckNull(piList[i].GetValue(insSrc, null))));
            }

            return lstParam;
        }

        public static object CheckNull(object strValue)
        {
            if (strValue == null || string.IsNullOrEmpty(strValue.ToString()))
                return DBNull.Value;
            else
                return strValue;
        }

        public static T EntityClone<T>(T insSrc)
        {
            T insDest = (T)Activator.CreateInstance(typeof(T));
            PropertyInfo[] piSrc = insSrc.GetType().GetProperties();
            PropertyInfo[] piDesc = insDest.GetType().GetProperties();

            for (int i = 0; i < piSrc.Length; i++)
            {
                Type propType = piSrc[i].PropertyType;
                if (piDesc[i].CanWrite)
                {
                    if (propType.IsGenericType
                        && propType.GetGenericTypeDefinition().Name == typeof(System.Collections.Generic.Dictionary<string, string>).Name
                        && propType.GetGenericTypeDefinition().GUID == typeof(System.Collections.Generic.Dictionary<string, string>).GUID)
                    {
                        Dictionary<string, string> originalDic = piSrc[i].GetValue(insSrc, null) as Dictionary<string, string>;
                        var newDictionary = originalDic.ToDictionary(entry => entry.Key, entry => entry.Value);
                        piDesc[i].SetValue(insDest, newDictionary, null);
                    }
                    else if (propType == System.Type.GetType("System.Collections.Hashtable"))
                    {
                        System.Collections.Hashtable newTable = piSrc[i].GetValue(insSrc, null) as System.Collections.Hashtable;
                        piDesc[i].SetValue(insDest, newTable.Clone(), null);
                    }
                    else
                    {
                        piDesc[i].SetValue(insDest, piSrc[i].GetValue(insSrc, null), null);
                    }
                }
            }

            return insDest;
        }

        public static Dictionary<string, KeyValuePair<string, string>> GetDisplayAttr<T>(T pINS)
        {
            Dictionary<string, KeyValuePair<string, string>> result = new Dictionary<string, KeyValuePair<string, string>>();
            PropertyInfo[] piSrc = pINS.GetType().GetProperties();

            for (int i = 0; i < piSrc.Length; i++)
            {
                object objValue = piSrc[i].GetValue(pINS, null);

                foreach (Attribute attr in piSrc[i].GetCustomAttributes(true))
                {
                    DisplayAttr getAttr = attr as DisplayAttr;
                    if (null != getAttr)
                    {
                        string strValue = string.Empty;
                        if (objValue != null)
                            strValue = objValue.ToString();
                        result.Add(piSrc[i].Name, new KeyValuePair<string, string>(getAttr.Description, strValue));
                    }
                }
            }
            return result;
        }

        public static Dictionary<string, string> GetDisplayAttr<T>()
        {
            Dictionary<string, string> result = new Dictionary<string, string>();
            Type type = typeof(T);
            foreach (PropertyInfo field in type.GetProperties())
            {
                foreach (Attribute attr in field.GetCustomAttributes(true))
                {
                    DisplayAttr getAttr = attr as DisplayAttr;
                    if (null != getAttr)
                    {
                        result.Add(field.Name, getAttr.Description);
                    }
                }
            }

            return result;
        }

        public enum SortOrder
        {
            ASC,
            DESC
        }

        public static List<T> SortList<T>(List<T> list, string sortBy, SortOrder direction)
        {
            if (string.IsNullOrEmpty(sortBy))
                return list;

            PropertyInfo property = list.GetType().GetGenericArguments()[0].GetProperty(sortBy);
            if (direction == SortOrder.ASC)
            {
                return list.OrderBy(e => property.GetValue(e, null)).ToList<T>();
            }
            else
            {
                return list.OrderByDescending(e => property.GetValue(e, null)).ToList<T>();
            }
        }
    }

    internal class DisplayAttr : Attribute
    {
        public DisplayAttr(string descrition_in)
        {
            this.description = descrition_in;
        }

        private string description;
        public string Description
        {
            get
            {
                return this.description;
            }
        }
    }

    internal class BoolAttr : Attribute
    {
        public BoolAttr(bool pValue)
        {
            this.bValue = pValue;
        }

        private bool bValue;
        public bool B_Value
        {
            get
            {
                return this.bValue;
            }
        }
    }
}