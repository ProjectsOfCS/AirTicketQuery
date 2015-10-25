using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace AirTicketQuery.Modules.Code
{
    /// <summary>
    /// DataSet and DataTable Check
    /// </summary>
    public static class DataSetHelper
    {
        /// <summary>
        /// Check DataSet is null,no talbe or Records=0
        /// </summary>
        /// <param name="aDataSet">DataSet</param>
        /// <returns></returns>
        public static bool IsEmptyDataSet(DataSet aDataSet)
        {
            return aDataSet == null || aDataSet.Tables == null || aDataSet.Tables.Count <= 0 ||
                aDataSet.Tables[0] == null || aDataSet.Tables[0].Rows == null || aDataSet.Tables[0].Rows.Count <= 0;
        }

        /// <summary>
        /// Check DataSet is null,no talbe
        /// </summary>
        /// <param name="aDataSet">DataSet</param>
        /// <returns></returns>
        public static bool IsNullDataSet(DataSet aDataSet)
        {
            return aDataSet == null || aDataSet.Tables == null || aDataSet.Tables.Count <= 0 ||
                aDataSet.Tables[0] == null || aDataSet.Tables[0].Rows == null;
        }

        /// <summary>
        /// Check DataTable is null or Records=0
        /// </summary>
        /// <param name="aDataTable">DataTable</param>
        /// <returns></returns>
        public static bool IsEmptyDataTable(DataTable aDataTable)
        {
            return aDataTable == null || aDataTable.Rows == null || aDataTable.Rows.Count <= 0;
        }

        /// <summary>
        /// Change DataTable to DataSet
        /// </summary>
        /// <param name="aTable">DataTabl</param>
        /// <returns>DataSet</returns>
        public static DataSet DataTable2DataSet(DataTable aTable)
        {
            if (aTable == null)
                return null;

            if (aTable.DataSet != null)
                return aTable.DataSet;

            DataSet myDataSet = new DataSet();
            myDataSet.Tables.Add(aTable);
            return myDataSet;
        }
    }
}
